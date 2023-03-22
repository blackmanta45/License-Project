import pickle
import pandas as pd
import json
import serialize_json as sj

class PrepareToSave:
    def __init__(self, videosTranscripts, classification):
        self.videosTranscripts = videosTranscripts
        self.classification = classification

    def serialize(self, d):
        for k, v in d.items():
            d[k] = sj.data_to_json(v)
        return d

    def clfToJson(self):
        return self.serialize(self.classification.clf.__dict__)

    def buildExcelDataFrameJson(self):
        ldaDataFrame = pd.DataFrame(columns = ["0", "1", "2", "3", "4"])
        for transcriptGabi_key in self.videosTranscripts.dictTranscriptTitle:
                if(self.videosTranscripts.dictTranscriptTitle[transcriptGabi_key] in self.classification.dictTitleCluster):
                    newLine = pd.DataFrame(
                                            [
                                                [
                                                    self.videosTranscripts.dictTranscriptTitle[transcriptGabi_key], 
                                                    transcriptGabi_key,
                                                    self.classification.dictTitleCluster[self.videosTranscripts.dictTranscriptTitle[transcriptGabi_key]],
                                                    self.videosTranscripts.dictTranscriptId[transcriptGabi_key],
                                                    self.videosTranscripts.dictTranscriptHidden[transcriptGabi_key]
                                                ]
                                            ],
                                            columns =  ["0", "1", "2", "3", "4"]
                                        )
                    ldaDataFrame = ldaDataFrame.append(newLine,ignore_index = True)

        return ldaDataFrame.to_json()
