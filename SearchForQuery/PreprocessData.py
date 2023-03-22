# -*- coding: utf-8 -*-
"""
Created on Thu Nov 28 12:22:41 2019

@author: Diana
"""
import json
import inflector

class PreprocessData:
    def __init__(self, mongoRepository, nlp, config):
        self.mongoRepository = mongoRepository
        self.singularizator = inflector.Spanish()
        self.config = config

        self.nlp = nlp
        if nlp != None:
            self.spanish_stopwords = []
            self.getStopWords()

    #preprocess the query
    def singularizeQuery(self, query):
        query = query.lower().split()
        return [self.singularizator.singularize(word) for word in query]

    #get all stopwords from an input file and put their in a list
    def getStopWords(self):
        stopwordsJson = self.mongoRepository.getEntityByName(self.config['MONGODB']['stopwords-name'])
        self.spanish_stopwords = json.loads(stopwordsJson)


    def ifIsFromStopWords(self, word):
        return word in self.spanish_stopwords


    #keep from transcripts just the nouns in singular form
    def preprocess(self, text):
        result = " "
        try:
            tag = self.nlp.pos_tag(text)
        except Exception as e:
            tag = ""
            print(e)
        for word, pos in tag:
            if pos == 'NOUN' :
                if( self.ifIsFromStopWords(word) == False ):
                    result += self.singularizator.singularize(word) + " "
        return result