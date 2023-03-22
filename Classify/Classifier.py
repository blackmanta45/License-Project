from WikiTranscripts import WikiTranscripts
from VideosTranscripts import VideosTranscripts
from GeneratedMatrix import GeneratedMatrix
from Classification import Classification
from PrepareToSave import PrepareToSave
from MongoRepository import MongoRepository
from VideosRepository import VideosRepository
import tensorflow_hub as hub

class Classifier:
    def __init__(self, config):
        self.config = config

        self.mongoRepository = MongoRepository(self.config)
        self.videosRepository = VideosRepository(self.config)

    def classifyFirstNVideos(self, articleVersion, n):
        wikiTranscripts = WikiTranscripts(self.mongoRepository, self.config, articleVersion)
        wikiTranscripts.configure()

        videosTranscripts = VideosTranscripts(wikiTranscripts, self.videosRepository)
        videosTranscripts.processFirstNVideos(n)
        
        return self.classifyVideos(videosTranscripts, wikiTranscripts)

    def classifyAllVideos(self, articleVersion):
        wikiTranscripts = WikiTranscripts(self.mongoRepository, self.config, articleVersion)
        wikiTranscripts.configure()

        videosTranscripts = VideosTranscripts(wikiTranscripts, self.videosRepository)
        videosTranscripts.processAllTranscripts()
        
        return self.classifyVideos(videosTranscripts, wikiTranscripts)

    def classifyVideos(self, videosTranscripts, wikiTranscripts):
        try:
            generatedMatrix = GeneratedMatrix(wikiTranscripts.wikiTrainer, videosTranscripts, self.config)
            generatedMatrix.configure()

            classification = Classification(wikiTranscripts.wikiTrainer, videosTranscripts, generatedMatrix)
            classification.start()

            prepareToSave = PrepareToSave(videosTranscripts, classification)
            excel = prepareToSave.buildExcelDataFrameJson()
            modelJson = prepareToSave.clfToJson()

            self.mongoRepository.deleteModel()
            self.mongoRepository.deleteLabeledTranscripts()

            self.mongoRepository.addModel(modelJson)
            self.mongoRepository.addLabeledTranscripts(excel)

            return "1"
        except Exception as e:
            return e
