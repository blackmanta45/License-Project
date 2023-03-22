class SearchForQuerySave:
    def __init__(self, searchForQuery):
        self.documentsWithLabels = searchForQuery.documentsWithLabels
        self.dictionary = searchForQuery.dictionary
        self.lsi = searchForQuery.lsi
        self.indexList = searchForQuery.indexList
        self.clf = searchForQuery.clf
    

