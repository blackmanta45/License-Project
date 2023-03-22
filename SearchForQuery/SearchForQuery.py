"""
Created on Thu Nov 28 11:58:22 2019

@author: Diana
"""

import tensorflow.compat.v1 as tf
import tensorflow_hub as hub

import gensim
from gensim import models
from gensim import similarities
from sklearn.svm import SVC

import pandas as pd
import re
import json
import serialize_json as sj

import wiki

class SearchForQuery:
    def __init__(self, mongoRepository, pData, noOfClusters, config, searchForQuerySave = None):
        tf.disable_v2_behavior()
        self.mongoRepository = mongoRepository
        self.pData = pData
        self.__noOfClusters = noOfClusters
        self.config = config

        if searchForQuerySave is None:
            self.initAll()
        else:
            self.initFromSave(searchForQuerySave)

    def initAll(self):
        self.documentsWithLabels = [[],[],[]]
        self.preprocessedListForDictionary = [[],[],[]]
        self.dictionary = []
        self.lsi = [None] * self.__noOfClusters
        self.indexList = [None] * self.__noOfClusters
        self.clf = None

        self.__processTranscripts()
        self.__LSI()
        self.__loadModel()

    def initFromSave(self, searchForQuerySave):
        self.documentsWithLabels = searchForQuerySave.documentsWithLabels
        self.dictionary = searchForQuerySave.dictionary
        self.lsi = searchForQuerySave.lsi
        self.indexList = searchForQuerySave.indexList
        self.clf = searchForQuerySave.clf

    def __extractData(self):
        df = self.mongoRepository.getExcelByName(self.config['MONGODB']['labeled-transcripts-save-name'])
        for i in range(5):
            yield df[i]

    def deserialize(self, class_init, attr):
        for k, v in attr.items():
            setattr(class_init, k, sj.json_to_data(v))
        return class_init
    
    #Process all transcripts
    def __processTranscripts(self):
        videosName,transcripts,labels,ids,hiddens = self.__extractData()
        preprocessedDocumentsList = []
        documents = []

        nrOfTranscriptsToProcess = len(videosName)
        print("STARTED PROCESSING TRANSCRIPTS")

        for i in range(0, nrOfTranscriptsToProcess):
            transcript = transcripts[i]
            videoName = videosName[i]
            label = labels[i]
            internal_id = ids[i]
            hidden = hiddens[i]
            # print("PROCESSING VIDEO: Transcript = " + str(transcript) + ", Title = " + str(videoName) + ", Label = " + str(label) + ", InternalId = " + str(internal_id))
            if transcript !=  "" and len(transcript)>6000:      #keep only valid transcripts for preprocessing
                # print(videoName)
                print("PROCESSING VIDEO: InternalId = " + str(ids[i]))
                preprocessedTranscript = self.pData.preprocess(transcript)   

                #keep all preprocessed transcripts in a container for each label
                self.documentsWithLabels[label].append((preprocessedTranscript,videoName,internal_id,hidden))
                
                #keep all preprocessed transcripts together
                documents.append([preprocessedTranscript, videoName, label])

                #keep all words from transcripts
                preprocessedDocumentsList.append(preprocessedTranscript.split())
        
        index = 0
        for transcript, videoName, label in documents:
            self.preprocessedListForDictionary[label].append(preprocessedDocumentsList[index])
            index += 1

    def __LSI(self):
        for i in range(self.__noOfClusters):
            self.dictionary.append(gensim.corpora.Dictionary(self.preprocessedListForDictionary[i]))

        bow_corpus = [None] * self.__noOfClusters  

        for i in range(self.__noOfClusters):
            if( len(self.dictionary[i]) != 0):
                #we have to create a bag of words( BoW )
                bow_corpus[i] = [self.dictionary[i].doc2bow(doc) for doc in self.preprocessedListForDictionary[i]]
                
                #we will transform it in a tf-idf vector
                tfidf = models.TfidfModel(bow_corpus[i]) 
                corpus_tfidf = tfidf[bow_corpus[i]]

                self.lsi[i] = models.LsiModel(corpus = corpus_tfidf, id2word = self.dictionary[i], num_topics = 5)
                #we will compute a similarity matrix, which it will help us later, for query
                self.indexList[i] = similarities.MatrixSimilarity(self.lsi[i][corpus_tfidf])
                
                #print(indexList[0])
                # print(self.lsi[i].print_topics(num_topics = 5, num_words = 10))
        

    def __loadModel(self):
        new_clf = self.mongoRepository.getJsonByName(self.config['MONGODB']['model-save-name'])
        self.clf = self.deserialize(SVC(), new_clf)
    
    def singularizeQuery(self, query):
        query = query.lower().split()
        return [self.singularizator.singularize(word) for word in query]

    def resultForQueryPaginated(self, query, page, chunk_size):
        result = self.resultForQuery(query)
        left = (page - 1) * chunk_size
        right = page * chunk_size

        if (left > result['TotalVideosForQuery']):
            result[1] = None
        else: 
            if (right > result['TotalVideosForQuery']):
                right = result['TotalVideosForQuery']
                result['Videos'] = result['Videos'][left:right]
            else:
                result['Videos'] = result['Videos'][left:right]
        
        return result


    def resultForQuery(self, query):
        ans = wiki.wikipedia_search(query)     #search a sequence on wiki pages
        if(len(ans["itemList"]) != 0 ):             #if we have a result
                queryWiki =  (ans["itemList"][0]["description"])       #assign as query this sequence
        else:   
                queryWiki = query                  #else assign just the query

        embeddedQueryWiki = None
        g = tf.Graph()
        with g.as_default(), tf.Session(graph = g) as session:
        # We will be feeding 1D tensors of text into the graph.
            tf.disable_eager_execution()
            text_input = tf.placeholder(dtype=tf.string, shape=[None])
            embed = hub.load(self.config['FOLDERLOCATIONS']['nnlm'])
            embedded_text = embed(text_input)
            init_op = tf.group([tf.global_variables_initializer(), tf.tables_initializer()])

            session.run(init_op)
            session.graph.finalize()
            embeddedQueryWiki = session.run(embedded_text, feed_dict={text_input: [queryWiki]})
            session.close()

        #make a predict for the query for its cluster
        queryClusterWiki = self.clf.predict(embeddedQueryWiki)[0]
        
        del g, session, text_input, embed, embedded_text, init_op, embeddedQueryWiki
        tf.keras.backend.clear_session()
        tf.reset_default_graph()

        #search in assigned container
        cluster = queryClusterWiki
        
        #transform in a bow corpus the query
        vec_bow = self.dictionary[cluster].doc2bow(self.pData.singularizeQuery(query))
        # convert the query to LSI space
        vec_lsi = self.lsi[cluster][vec_bow]
        
        # perform a similarity query against the corpus
        sims = self.indexList[cluster][vec_lsi]  
        
        sims = sorted(enumerate(sims), key=lambda item: -item[1])
        
        result = {}
        result['TotalVideosForQuery'] = len(sims)
        result['Videos'] = []
        # #print top 10 results
        for s in sims:
            result['Videos'].append({'Id':self.documentsWithLabels[cluster][s[0]][2], 'Title':self.documentsWithLabels[cluster][s[0]][1]})
        return result