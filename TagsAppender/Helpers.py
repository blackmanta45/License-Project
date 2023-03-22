from nltk import RegexpTokenizer
import re

class Helpers():
    def __init__(self):
        self.nothing = None

    def clean_text(self, text):
        ''' Lowering text and removing undesirable marks

        Parameter:
        
        text: document to be cleaned    
        '''
        
        text = text.lower()
        text = re.sub(r'[^\w\s]','',text)
        text = re.sub("(\s\d+)","",text)
        text = ' '.join( [w for w in text.split() if len(w)>3] )
        text = re.sub('\s+', ' ', text) # matches all whitespace characters
        text = text.strip(' ')
        return text
        
    def processTranscript(self, transcript):
        regex = RegexpTokenizer(r"\b\w+\b");
        words = regex.tokenize(transcript)
        processedTranscript = ""
        for word in words:
            word = word.lower()
            processedTranscript = processedTranscript + word + " "
        return processedTranscript