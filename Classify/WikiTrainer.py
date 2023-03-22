from sklearn.model_selection import train_test_split

class WikiTrainer:
    def __init__(self, proccessedArticles, processedLabels):
        self.wikiTrain = None
        self.wikiEvalTrain = None
        self.wikiFinalTrain = None
        self.wikiTrainTest = None
        self.wikiEvalTest = None
        self.wikiFinalTest = None
        self.__train(proccessedArticles, processedLabels)

    def __train(self, processedArticles, processedLabels):
        self.wikiTrain, self.wikiEvalTrain, self.wikiTrainTest, self.wikiEvalTest = train_test_split(processedArticles, processedLabels, test_size=0.30, random_state=42)
        self.wikiEvalTrain, self.wikiFinalTrain, self.wikiEvalTest, self.wikiFinalTest =  train_test_split(self.wikiEvalTrain, self.wikiEvalTest, test_size=0.50, random_state=42)
