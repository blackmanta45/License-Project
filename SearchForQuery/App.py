import dill as pickle
import copy

from flask import Flask, request, make_response, jsonify
from functools import wraps

from stanfordcorenlp import StanfordCoreNLP

from SearchForQuery import SearchForQuery
from MongoRepository import MongoRepository
from PreprocessData import PreprocessData
from SearchForQuerySave import SearchForQuerySave 

import os
import configparser

try:
    config = configparser.ConfigParser()
    if 'PRODUCTION' in os.environ:
        config.read('production.cfg')
    else:
        config.read('development.cfg')

    app = Flask(__name__)

    nlp = StanfordCoreNLP(config['FOLDERLOCATIONS']['stanford-server'], lang = 'es')
    mongoRepository = MongoRepository(config)

    preprocessDataWithNLP = PreprocessData(mongoRepository, nlp, config)
    preprocessDataWithoutNLP = PreprocessData(mongoRepository, None, config)
except Exception as e:
    raise e

def auth_required(f):
    @wraps(f)
    def decorated(*args, **kwargs):
        auth = request.authorization
        if auth and auth.username == config['SECRET']['api-username'] and auth.password == config['SECRET']['api-password']:
            return f(*args, **kwargs)

        return make_response('Could not verify your login!', 401, {'WWW-Authenticate': 'Basic realm="Login Required"'})

    return decorated

@app.route('/SFQ/paginated', methods=['GET'])
@auth_required
def SFQPaginated():
    try:
        query = request.args.get('query')
        page = request.args.get('page')
        chunk_size = request.args.get('chunk-size')
        tempSFQ = SearchForQuery(mongoRepository, preprocessDataWithoutNLP, 3, config, mongoRepository.getSearchForQuerySave())
        result = tempSFQ.resultForQueryPaginated(str(query), int(page), int(chunk_size))
        return jsonify(result)
    except Exception as e:
        return str(e)

@app.route('/SFQ/all', methods=['GET'])
@auth_required
def SFQAll():
    try:
        query = request.args.get('query')
        tempSFQ = SearchForQuery(mongoRepository, preprocessDataWithoutNLP, 3, config, mongoRepository.getSearchForQuerySave())
        result = tempSFQ.resultForQuery(str(query))
        return jsonify(result)
    except Exception as e:
        return str(e)

@app.route('/SFQ/remodel', methods=['POST'])
@auth_required
def remodel():
    try:
        searchForQuery = SearchForQuery(mongoRepository, preprocessDataWithNLP, 3, config)
        searchForQueryMainSave = SearchForQuerySave(searchForQuery)
        SFQpickled = pickle.dumps(searchForQueryMainSave)
        mongoRepository.replaceSearchForQuerySave(SFQpickled)
        return "1"
    except Exception as e:
        return str(e)

app.run(host="0.0.0.0", port = 5001)