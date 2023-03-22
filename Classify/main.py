from flask import Flask, request, make_response, jsonify
from functools import wraps
# from waitress import serve
from Classifier import Classifier
import os
import configparser

try:
    config = configparser.ConfigParser()
    if 'PRODUCTION' in os.environ:
        config.read('production.cfg')
    else:
        config.read('development.cfg')

    app = Flask(__name__)

    classifier = Classifier(config)
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


@app.route('/classify/all', methods=['POST'])
@auth_required
def classifyAllVideos():
    try:
        articleVersion = request.args.get('wiki-version')
        return classifier.classifyAllVideos(articleVersion)
    except Exception as e:
        return str(e)

@app.route('/classify', methods=['POST'])
@auth_required
def classifyFirstNValidVideos():
    # try:
    videosNumber = request.args.get('videos-number')
    articleVersion = request.args.get('wiki-version')
    return classifier.classifyFirstNVideos(articleVersion, videosNumber)
    # except Exception as e:
    #     return str(e)

@app.route('/test', methods=['GET'])
@auth_required
def test():
    return "1"

app.run(host="0.0.0.0", port=5000)