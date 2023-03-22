from pymongo.mongo_client import MongoClient
from pymongo.server_api import ServerApi
from ast import literal_eval

import gridfs
import pandas as pd
import dill as pickle

class MongoRepository():
    def __init__(self, config):
        try:
            self.config = config
            client = MongoClient(self.config['MONGODB']['connection-string'], server_api=ServerApi('1'), serverSelectionTimeoutMS=15000)
            upvDb = client[self.config['MONGODB']['database-name']]
            self.fs = gridfs.GridFS(upvDb)
        except Exception as e:
            return e

    def deleteFirst(self, name):
        try:
            file = self.fs.find_one({"filename": name})
            self.fs.delete(file._id)
        except Exception as e:
            return e

    def replaceSearchForQuerySave(self, searchForQuerySave):
        try:
            name = self.config['MONGODB']['search-for-query-save-name']
            self.deleteFirst(name)
            self.fs.put(searchForQuerySave, filename = name)
        except Exception as e:
            return e

    def getSearchForQuerySave(self):
        try:
            return pickle.loads(self.getEntityByName(self.config['MONGODB']['search-for-query-save-name']))
        except Exception as e:
            return e

    def getExcelByName(self, name):
        try:
            dbEntity = self.fs.get_last_version(name).read()
            df = pd.read_json(dbEntity)
            return df
        except Exception as e:
            return e

    def getJsonByName(self, name):
        try:
            dbEntity = self.fs.get_last_version(name).read()
            json = literal_eval(dbEntity.decode('utf8'))
            return json
        except Exception as e:
            return e

    def getEntityByName(self, name):
        try:
            return self.fs.get_last_version(name).read()
        except Exception as e:
            return e