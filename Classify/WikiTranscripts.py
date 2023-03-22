from nltk import RegexpTokenizer
from WikiTrainer import WikiTrainer

import pandas as pd

class WikiTranscripts():
    def __init__(self, mongoRepository, config, articleVersion):
        self.mongoRepository = mongoRepository
        self.config = config
        self.articleVersion = articleVersion

        self.processedArticles = []
        self.processedLabels = []
        self.wikiTrainer = None

    def configure(self):
        excelName = str(self.config['MONGODB']['wikipedia-name']) + self.articleVersion
        excel = self.mongoRepository.getExcelByName(excelName)
        articles = excel[1]
        labels = excel[2]

        for i in range(0,len(articles)):
            art = str(articles[i])
            if len(art) < 50:
                continue
                
            self.processedArticles.append(self.processTranscript(art))
            self.processedLabels.append(labels[i])
        
        self.wikiTrainer = WikiTrainer(self.processedArticles, self.processedLabels)

    def processTranscript(self, transcript):
        regex = RegexpTokenizer(r"\b\w+\b");
        words = regex.tokenize(transcript)
        processedTranscript = ""
        for word in words:
            word = word.lower()
            processedTranscript = processedTranscript + word + " "
        return processedTranscript

