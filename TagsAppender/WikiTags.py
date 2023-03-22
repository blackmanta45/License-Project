from rake_nltk import Rake
import nltk
import numpy as np

class WikiTags():
    def __init__(self, config, mongoRepository, pData, helpers):
        self.config = config
        self.mongoRepository = mongoRepository
        self.pData = pData
        self.helpers = helpers
        nltk.download('stopwords')
        nltk.download('punkt')

        self.pData.getStopWords()
        self.taggedWiki = {}

    def buildTopWikiTags(self):
        self.loadAndProcessArticles()
        self.getTopTags()

    def loadAndProcessArticles(self):
        title, description = self.__extractData()
        testArticles = title + description
        initialWikipediaArticles = np.concatenate((title, description), axis = 0)
        print(initialWikipediaArticles)
        print("---------------------------------------------------------------")
        print(testArticles)
        wikipediaArticles = []
        index = 0
        for article_list in initialWikipediaArticles:
            print(index)
            index += 1
            try:
                article_list = self.helpers.clean_text(article_list)
                wikipediaArticles.append(article_list)
            except Exception as e:
                print(e)
                continue
            try:
                article_list = self.pData.preprocess(article_list)
                if article_list != "":
                    wikipediaArticles.append(article_list)
            except Exception as e:
                print(e)
                continue
        print("Finished processing articles")
        #use rake tool for spanish
        r = Rake(language="spanish")

        tags = [] 

        #extract tags for all the articles and add them in a list
        for article_list in wikipediaArticles:
            r.extract_keywords_from_text(article_list)
            raw_rake_keywords =  r.get_ranked_phrases()
            to_add = []
            for i in range(0, 5):
                if(i < len(raw_rake_keywords)):
                    preprocessed = self.pData.preprocess(raw_rake_keywords[i])
                    for word in preprocessed.split():
                        if((len(word) > 3) and (any(char.isdigit() for char in word) == False)):
                            to_add.append(word)
            tags.append(to_add)

        index = 0
        for article_list in wikipediaArticles:
            self.taggedWiki[article_list] = tags[index]
            index += 1
        print("Finish loadAndProcessArticles")
        print(self.taggedWiki[wikipediaArticles[0]])


    def getTopTags(self):
        print("START  getTopTags")
        set_tags = set()
        for key in self.taggedWiki:
            set_tags = set_tags.union(self.taggedWiki[key])

        print("START count_tag")
        keyword_occurences = self.count_tag(set_tags)
        print("FINISH count_tag")
        trunc_occurences = keyword_occurences[1:5001]
        top_tags = [i[0] for i in trunc_occurences]
        print("START most_common")
        for key in self.taggedWiki:
            self.taggedWiki[key] = self.most_common(self.taggedWiki[key], top_tags)
        print("FINISH most_common")

        delete = [key for key in self.taggedWiki if len(self.taggedWiki[key]) == 0]
        for key in delete: 
            del self.taggedWiki[key]
        print("Finish getTopTags")

    def __extractData(self):
        df = self.mongoRepository.getExcelByName(self.config['MONGODB']['wikipedia-name'] + "3.0")
        for i in range(2):
            yield df[i]

    def count_tag(self, list_words): 
        ''' Count the number of occurrences and the average score for each tag '''
        
        keyword_count = dict()
        for s in list_words: 
            keyword_count[s] = []
            keyword_count[s].append(0)
        
        index = 0
        for key in self.taggedWiki: 
            
            if (len(self.taggedWiki[key]) == 0): 
                continue
            index += 1
            for s in [s for s in self.taggedWiki[key] if s in list_words]: 
                    keyword_count[s][0] += 1
                    
        # conversion of our dictionary into a list
        keyword_occurences = []
        for tag, item in keyword_count.items():
            if(item[0] == 0):
                keyword_occurences.append([tag, item[0], 0])
            else:
                keyword_occurences.append([tag, item[0], item[0]/index])
            
        keyword_occurences.sort(key = lambda x:x[2], reverse = True)
        
        return keyword_occurences

    def most_common(self, tags, top_tags):
    
        tags_filtered = []
        
        for tag in tags:
            
            if tag in top_tags:
                tags_filtered.append(tag)
                
        return tags_filtered
