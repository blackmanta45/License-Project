# -*- coding: utf-8 -*-
import dill as pickle
from stanfordcorenlp import StanfordCoreNLP

from SearchForQuery import SearchForQuery
from MongoRepository import MongoRepository
from PreprocessData import PreprocessData
from SearchForQuerySave import SearchForQuerySave


nlp = StanfordCoreNLP("stanford-server", lang = 'es')
mongoRepository = MongoRepository()
preprocessData = PreprocessData(mongoRepository, nlp)

# searchForQueryObj = SearchForQuery(mongoRepository, preprocessData, 3)
# searchForQueryToSave = SearchForQuerySave(searchForQueryObj)

# SFQpickled = pickle.dumps(searchForQueryToSave)

# mongoRepository.replaceSearchForQuerySave(SFQpickled, "SFQversion")

searchForQuerySaveFromDB = mongoRepository.getSearchForQuerySave("SFQversion")

preprocessData = PreprocessData(mongoRepository, None)
searchForQuery = SearchForQuery(mongoRepository, preprocessData, 3, searchForQuerySaveFromDB)


while(True):
    print("Ready for input")
    query = input()
    if(query == "STOP"):
        nlp.close()
        break
    print(str(searchForQuery.resultForQuery(query)))