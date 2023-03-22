from pymongo.mongo_client import MongoClient
from pymongo.server_api import ServerApi

import gridfs
import pandas as pd

class MongoRepository():
    def __init__(self, config):
        try:
            self.config = config
            client = MongoClient(self.config['MONGODB']['connection-string'], server_api=ServerApi('1'), serverSelectionTimeoutMS=15000)
            upvDb = client[self.config['MONGODB']['database-name']]
            self.fs = gridfs.GridFS(upvDb)
        except Exception as e:
            return e

    def deleteModel(self):
        try:
            self.deleteFirst(self.config['MONGODB']['model-save-name'])
        except Exception as e:
            return e
        
    def deleteLabeledTranscripts(self):
        try:
            self.deleteFirst(self.config['MONGODB']['labeled-transcripts-save-name'])
        except Exception as e:
            return e

    def deleteFirst(self, name):
        try:
            file = self.fs.find_one({"filename": name})
            self.fs.delete(file._id)
        except Exception as e:
            return e
    
    def addModel(self, json):
        try:
            self.addLargeJson(json, self.config['MONGODB']['model-save-name'])
        except Exception as e:
            return e
    
    def addLabeledTranscripts(self, dataFrame):
        try:
            self.addLargeExel(dataFrame, self.config['MONGODB']['labeled-transcripts-save-name'])
        except Exception as e:
            return e

    def addLargeJson(self, json, name):
        try:
            self.fs.put(str(json), encoding = 'utf-8', filename = name)
        except Exception as e:
            return e

    def addLargeExel(self, dataFrame, name):
        self.fs.put(dataFrame, encoding = 'utf-8', filename = name)

    def getExcelByName(self, name):
        try:
            dbEntity = self.fs.get_last_version(name).read()
            df = pd.read_json(dbEntity)
            return df
        except Exception as e:
            return e