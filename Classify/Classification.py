import time
import numpy as np
            
from sklearn import svm
from sklearn.metrics import classification_report
from sklearn.model_selection import cross_val_score 
from collections import defaultdict 

class Classification:
    def __init__(self, wikiTrainer, videosTranscripts, generatedMatrix):
        self.wikiTrainer = wikiTrainer
        self.videosTranscripts = videosTranscripts
        self.generatedMatrix = generatedMatrix
        
        self.dictTitleCluster = {}
        self.clf = None

    def start(self):
        print("WE GOT HERE")
        self.wikiTrainer.wikiTrainTest = np.array(self.wikiTrainer.wikiTrainTest)

        validTranscriptsIndices = defaultdict(list)
        wrongTranscriptsIndices = range(0,len(self.videosTranscripts.transcripts))

        TranscriptsX = []
        TranscriptsXLabels =[]

        self.clf = svm.SVC(gamma=0.001, decision_function_shape='ovo', kernel='rbf', C=60)
        start = time.time()

        while True:
            nr = 0
            #intre processed articles si labels
            # scores = cross_val_score(self.clf, self.generatedMatrix.X, self.wikiTrainer.wikiTrainTest.ravel(), cv=10)
            # print("Accuracy: %0.2f (+/- %0.2f)" % (scores.mean(), scores.std()))
            
            #fit intre proccessed articles si labels
            self.clf.fit(self.generatedMatrix.X, self.wikiTrainer.wikiTrainTest)
            
            #prezice si pt articles de test 
            # result = self.clf.predict(self.generatedMatrix.Z)
            
            # #cat de bine prezice label pt articles de test
            # print(classification_report( (np.asarray(self.wikiTrainer.wikiEvalTest)).ravel() , result))
            
            resultTrLabels = self.clf.predict(self.generatedMatrix.T)
            resultKeyLabels = self.clf.predict(self.generatedMatrix.K)
            
            validTranscriptsX = []
            validTranscriptsLabels = []
            
            invalidTranscriptsX = []
            invalidTranscriptsLabels = []
            invalidKeywordsX =[]
            
            wrongIndices =[]
            for i in range(0,len(resultTrLabels)):
                if(resultTrLabels[i] == resultKeyLabels[i]):
                    validTranscriptsX.append(self.generatedMatrix.T[i])
                    for key_transcript in self.videosTranscripts.dictTranscriptEmbed:
                        ok = 1
                        for k in range(0, len(self.videosTranscripts.dictTranscriptEmbed[key_transcript])):
                            if(self.videosTranscripts.dictTranscriptEmbed[key_transcript][k] != self.generatedMatrix.T[i][k]):
                                ok = 0
                                break
                    
                        if(ok == 1):
                            self.dictTitleCluster[self.videosTranscripts.dictTranscriptTitle[key_transcript]] = resultTrLabels[i]
                            
                    TranscriptsX.append(self.generatedMatrix.T[i])
                    TranscriptsXLabels.append(resultTrLabels[i])    
                    validTranscriptsLabels.append(resultTrLabels[i])
                    validTranscriptsIndices[resultTrLabels[i]].append(wrongTranscriptsIndices[i])
                    nr+=1
                else:
                    invalidTranscriptsX.append(self.generatedMatrix.T[i])
                    invalidTranscriptsLabels.append(resultTrLabels[i])
                    invalidKeywordsX.append(self.generatedMatrix.K[i])
                    wrongIndices.append(wrongTranscriptsIndices[i])
            
            wrongTranscriptsIndices = wrongIndices
                

            print("Valid transcripts:" + str(nr) + " / " + str(len(resultTrLabels)) )
            
            if(nr < 20):
                for j in range(0,len(invalidTranscriptsX)):
                    TranscriptsX.append(invalidTranscriptsX[j])
                    TranscriptsXLabels.append(invalidTranscriptsLabels[j])
                break
        
            self.generatedMatrix.X = np.append(self.generatedMatrix.X, np.array(validTranscriptsX), axis = 0)
            self.wikiTrainer.wikiTrainTest = np.append(self.wikiTrainer.wikiTrainTest, np.array(validTranscriptsLabels))
            
            self.generatedMatrix.T = np.array(invalidTranscriptsX)
            self.generatedMatrix.K = np.array(invalidKeywordsX)
            
            if(nr == len(resultTrLabels)):
                #print(nr)
                #print(resultTrLabels)
                break

        self.clf = svm.SVC(gamma=0.001, decision_function_shape='ovo', kernel='rbf', C=60)
        # scores = cross_val_score(self.clf, np.array(TranscriptsX), np.array(TranscriptsXLabels).ravel(), cv=10)
        # print("Accuracy: %0.2f (+/- %0.2f)" % (scores.mean(), scores.std()))
        self.clf.fit(np.array(TranscriptsX), np.array(TranscriptsXLabels).ravel())
        # result = self.clf.predict(self.generatedMatrix.Q)

        end = time.time()   
        # print(classification_report((np.asarray(self.wikiTrainer.wikiFinalTest)).ravel(), result))

        #print(start - end)