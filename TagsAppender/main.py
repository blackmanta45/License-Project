from flask import Flask, request, make_response, jsonify
from functools import wraps
# from waitress import serve
from Brain import Brain
import os
import configparser

try:
    config = configparser.ConfigParser()
    if 'PRODUCTION' in os.environ:
        config.read('production.cfg')
    else:
        config.read('development.cfg')
    app = Flask(__name__)

    brain = Brain(config)
except Exception as e:
    raise e

# serve(app, host="0.0.0.0", port=5000)

def auth_required(f):
    @wraps(f)
    def decorated(*args, **kwargs):
        auth = request.authorization
        if auth and auth.username == config['SECRET']['api-username'] and auth.password == config['SECRET']['api-password']:
            return f(*args, **kwargs)

        return make_response('Could not verify your login!', 401, {'WWW-Authenticate': 'Basic realm="Login Required"'})

    return decorated
    
@app.route('/tag/transcript', methods=['GET'])
@auth_required
def tagTranscript():
    transcript = request.json['transcript']
    keywords = brain.computeKeywordsForTranscript(transcript)
    return keywords

@app.route('/tag/buildWikiTags', methods=['GET'])
@auth_required
def buildWikiTags():
    brain.buildWikiTags()
    return ""

@app.route('/tag/remodel', methods=['GET'])
@auth_required
def remodel():
    brain.remodel()
    return ""

@app.route('/test', methods=['GET'])
@auth_required
def test():
    return "1"

app.run(host="0.0.0.0", port=5004)