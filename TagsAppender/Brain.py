from MLModel import MLModel
from WikiTags import WikiTags
from PrepareToSave import PrepareToSave
from MongoRepository import MongoRepository
from PreprocessData import PreprocessData
from Helpers import Helpers

from stanfordcorenlp import StanfordCoreNLP

class Brain():
    def __init__(self, config):
        self.config = config

        self.mongoRepository = MongoRepository(self.config)
        self.helpers = Helpers()

        nlp = StanfordCoreNLP(self.config['FOLDERLOCATIONS']['stanford-server'], lang = 'es')
        self.pData = PreprocessData(self.mongoRepository, nlp, self.config)

        self.mlModel = MLModel(self.config, self.mongoRepository, self.helpers, self.pData)
        self.mlModel.load()

    def computeKeywordsForTranscript(self, text):
        keywords = self.mlModel.recommend_tags_lda(text)
        return keywords

    def buildWikiTags(self):
        wikiTags = WikiTags(self.config, self.mongoRepository, self.pData, self.helpers)
        wikiTags.buildTopWikiTags()
        prepareToSave = PrepareToSave(wikiTags, None)
        topWikiTagsExcelJson = prepareToSave.buildExcelDataFrameJson()
        print("WE FINISHED EVERYTHING EXCEPT UPDATE DB")
        self.mongoRepository.deleteTopTags()
        self.mongoRepository.addTopTags(topWikiTagsExcelJson)
        print("WE UPDATED DB")

    def remodel(self):
        self.mlModel = MLModel(self.config, self.mongoRepository, self.helpers, self.pData)
        self.mlModel.train()
        prepareToSave = PrepareToSave(None, self.mlModel)
        lda_model_pickled = prepareToSave.pickleLda()
        vectorizer_text_pickled = prepareToSave.pickleVectorizer()
        print("WE FINISHED REMODEL AND PICKLE")
        self.mongoRepository.replaceLdaModel(lda_model_pickled)
        self.mongoRepository.replaceVectorizerText(vectorizer_text_pickled)

        