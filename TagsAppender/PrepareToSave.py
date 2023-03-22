import dill as pickle
import pandas as pd
import json

class PrepareToSave:
    def __init__(self, wikiTags, mlModel):
        self.wikiTags = wikiTags
        self.mlModel = mlModel

    def buildExcelDataFrameJson(self):
        ldaDataFrame = pd.DataFrame(columns = ["0", "1"])
        for key in self.wikiTags.taggedWiki:
            newLine = pd.DataFrame([[key, self.wikiTags.taggedWiki[key]]], columns = ['0', '1'])
            ldaDataFrame = ldaDataFrame.append(newLine, ignore_index = True)
    
        return ldaDataFrame.to_json()

    def pickleLda(self):
        return pickle.dumps(self.mlModel.lda_model)
    
    def pickleVectorizer(self):
        return pickle.dumps(self.mlModel.vectorizer_text)

