from pymongo.mongo_client import MongoClient
from pymongo.server_api import ServerApi

import pyodbc 
import gridfs
import pandas as pd

class VideosRepository():
    def __init__(self, config):
        self.conn = pyodbc.connect(driver = config['MYSQL']['driver'],
                                server    = config['MYSQL']['server'],
                                database  = config['MYSQL']['database'],
                                user      = config['MYSQL']['admin-user'],
                                password  = config['MYSQL']['admin-password'],
                                )
        self.cursor = self.conn.cursor()
        self.table = config['MYSQL']['table-videos']

    def save(self):
        self.conn.commit()

    def getAllVideos(self):
        try:
            videos = self.cursor.execute(f'''SELECT * FROM {self.table}''')
            return videos.fetchall()
        except Exception as e:
            return e

    def getAllValidVideos(self):
        try:
            videos = self.cursor.execute(f'''SELECT id, title, transcription, keywords, hidden FROM {self.table}
                                        WHERE   hasTranscription = 1 AND
                                                keywords IS NOT NULL AND
                                                keywords <> '' AND
                                                deletionDate IS NULL''')
            videosDb = videos.fetchall()
            print("We fetched ALL")
            return videosDb
        except Exception as e:
            print("We failed!")
            print(e)
            return e

    def getFirstNValidVideos(self, n):
        try:
            videos = self.cursor.execute(f'''SELECT TOP {n} id, title, transcription, keywords, hidden FROM {self.table}
                                        WHERE   hasTranscription = 1 AND
                                                keywords IS NOT NULL AND
                                                keywords <> '' AND
                                                deletionDate IS NULL''')
            return videos.fetchall()
        except Exception as e:
            return e
                        