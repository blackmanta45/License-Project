import numpy as np
import dill as pickle
from sklearn.decomposition._online_lda import LatentDirichletAllocation
import sklearn.decomposition._online_lda 
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.preprocessing import MultiLabelBinarizer
from sklearn.model_selection import train_test_split

class MLModel():
    def __init__(self, config, mongoRepository, helpers, pData):
        self.config = config
        self.mongoRepository = mongoRepository
        self.helpers = helpers
        self.pData = pData
        
        self.lda_model = None
        self.vectorizer_text = None
    
    def __extractData(self):
        df = self.mongoRepository.getExcelByName(self.config['MONGODB']['top-wiki-tags-name'])
        print(df)
        for i in range(2):
            yield df[i]

    def load(self):
        ldamodel = self.mongoRepository.getEntityByName(self.config['MONGODB']['lda-model-name'])
        print(type(ldamodel))
        self.lda_model = pickle.loads(ldamodel)
        print(type(self.lda_model))
        self.vectorizer_text = pickle.loads(self.mongoRepository.getEntityByName(self.config['MONGODB']['vectorizer-text-name']))

    def train(self):
        articles, tags = self.__extractData()

        vectorizer_X = TfidfVectorizer(analyzer='word', min_df=0.0, max_df = 1.0, 
                                   strip_accents = None, encoding = 'utf-8', 
                                   preprocessor=None, 
                                   token_pattern=r"(?u)\S\S+", # Need to repeat token pattern
                                   max_features=1000)

        multilabel_binarizer = MultiLabelBinarizer()
        y_target = multilabel_binarizer.fit_transform(tags)

        X_train, X_test, y_train, y_test = train_test_split(articles, y_target, test_size=0.2,train_size=0.8, random_state=0)

        X_tfidf_train = vectorizer_X.fit_transform(X_train)
        X_tfidf_test = vectorizer_X.transform(X_test)

        self.lda(vectorizer_X, X_tfidf_train, X_tfidf_test)

        best_lda = LatentDirichletAllocation(n_components=10, max_iter=5,
                                        learning_method='online',
                                        learning_offset=50.,
                                        random_state=0).fit(X_tfidf_train)
        
        n_topics = 10

        self.lda_model = LatentDirichletAllocation(n_components=n_topics, max_iter=5,
                                                learning_method='online',
                                                learning_offset=50.,
                                                random_state=0).fit(X_tfidf_train)

        self.vectorizer_text = TfidfVectorizer(analyzer='word', min_df=0.0, max_df = 1.0, 
                                            strip_accents = None, encoding = 'utf-8', 
                                            preprocessor=None, 
                                            token_pattern=r"(?u)\S\S+", # Need to repeat token pattern
                                            max_features=1000)
        self.vectorizer_text.fit(X_train)

    def lda(self, vectorizer, data_train, data_test):
        ''' Showing the perplexity score for several LDA models with different values
        for n_components parameter, and printing the top words for the best LDA model
        (the one with the lowest perplexity)

        Parameters:

        vectorizer: TF-IDF convertizer                                              
        data_train: data to fit the model with
        data_test: data to test
        '''

        # number of topics 
        n_top_words = 20
        best_perplexity = np.inf
        best_lda = 0
        perplexity_list = []
        n_topics_list = []
        print("Extracting term frequency features for LDA...")

        for n_topics in np.linspace(10, 50, 5, dtype='int'):
            self.lda_model = LatentDirichletAllocation(n_components=n_topics, max_iter=5,
                                            learning_method='online',
                                            learning_offset=50.,
                                            random_state=0).fit(data_train)
            n_topics_list.append(n_topics)
            perplexity = self.lda_model.perplexity(data_test)
            perplexity_list.append(perplexity)

            # Perplexity is defined as exp(-1. * log-likelihood per word)
            # Perplexity: The smaller the better
            if perplexity <= best_perplexity:
                best_perplexity = perplexity
                best_lda = self.lda_model
                                    
        # plt.title("Evolution of perplexity score depending on number of topics")
        # plt.xlabel("Number of topics")
        # plt.ylabel("Perplexity")
        # plt.plot(n_topics_list, perplexity_list)
        # plt.show()

    def recommend_tags_lda(self, text):
        ''' returns up to 5 tags.
        Parameters:
        text: the transcript
        X_train: data to fit the model with
        '''
        n_topics = 10
        threshold = 0.008 #works fine with this threshold, all trancripts have tags
        list_scores = []
        list_words = []
        used = set()
        
        text = self.helpers.clean_text(text)
        text = self.pData.preprocess(text)

        text_tfidf = self.vectorizer_text.transform([text])
        
        text_projection = self.lda_model.transform(text_tfidf)
        feature_names = self.vectorizer_text.get_feature_names()
        lda_components = self.lda_model.components_ / self.lda_model.components_.sum(axis=1)[:, np.newaxis] # normalization

        for topic in range(n_topics):
            topic_score = text_projection[0][topic]

            for (word_idx, word_score) in zip(lda_components[topic].argsort()[:-10:-1], sorted(lda_components[topic])[:-10:-1]):
                score = word_score * topic_score

                if score >= threshold:
                    list_scores.append(score)
                    list_words.append(feature_names[word_idx])
                    used.add(feature_names[word_idx])

        results = [tag for (y,tag) in sorted(zip(list_scores,list_words), key=lambda pair: pair[0], reverse=True)]
        
        tags = " ".join(results[:20])

        return tags