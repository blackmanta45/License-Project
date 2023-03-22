import requests
import configparser
import time
import os
import base64
import json
from datetime import datetime

try:
    config = configparser.ConfigParser()
    if 'PRODUCTION' in os.environ:
        config.read('production.cfg')
    else:
        config.read('development.cfg')
except Exception as e:
    raise e

# time.sleep(180)

recsys_content = {'username' : config['RECSYSAPI']['username'], 'password' : config['RECSYSAPI']['password']}

classify_headers = {'Authorization': 'Basic ' + base64.urlsafe_b64encode((config['CLASSIFY']['username'] + ':' + config['CLASSIFY']['password']).encode('ascii')).decode("ascii"),
            'Connection': 'keep-alive'}
sfq_headers = {'Authorization': 'Basic ' + base64.urlsafe_b64encode((config['SFQ']['username'] + ':' + config['SFQ']['password']).encode('ascii')).decode("ascii"),
            'Connection': 'keep-alive'}
while(1):
    try:
        time.sleep(120)
        token = requests.post(config['RECSYSAPI']['login'], data = json.dumps(recsys_content), headers = {'Accept': '*/*', 'Content-Type': 'application/json'}).content.decode("utf-8")
        recsys_headers = {'Authorization': 'Bearer ' + str(token),
                    'Connection': 'keep-alive'}
        requests.get(config['RECSYSAPI']['update-videos'], headers = recsys_headers)

        time.sleep(120)
        requests.post(config['CLASSIFY']['classify'], headers = classify_headers)

        time.sleep(120)
        requests.post(config['SFQ']['remodel'], headers = sfq_headers)
    except Exception as e:
        time.sleep(60)

    
