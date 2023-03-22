from WikiTranscripts import WikiTranscripts

class VideosTranscripts:
    def __init__(self, wikiTranscripts, videosRepository):
        self.wikiTranscripts = wikiTranscripts
        self.videosRepository = videosRepository

        self.transcripts = []
        self.transcriptsKeywords = []

        self.dictIndexTranscript = {}
        self.dictTranscriptTitle = {}
        self.dictTranscriptEmbed = {}
        self.dictTranscriptId = {}
        self.dictTranscriptHidden = {}

    def getAllValidVideos(self):
        return self.videosRepository.getAllValidVideos()
        
    def getFirstNValidVideos(self, n):
        return self.videosRepository.getFirstNValidVideos(n)

    def processAllTranscripts(self):
        videosDb = self.getAllValidVideos()
        self.processTranscripts(videosDb)

    def processFirstNVideos(self, n):
        videosDb = self.getFirstNValidVideos(n)
        self.processTranscripts(videosDb)

    def processTranscripts(self, videosDb):
        print("Started Processing")
        counter = 0
        for video in videosDb:
            internal_id = video[0]
            transcript = video[2]
            keywords = video[3]
            hidden = video[4]
                
            processedTr = self.wikiTranscripts.processTranscript(transcript)
            
        
            if(processedTr != ""):
                self.transcripts.append(processedTr)
                self.dictIndexTranscript[counter] = processedTr
                self.dictTranscriptTitle[processedTr] = video[1]
                self.dictTranscriptId[processedTr] = internal_id
                self.dictTranscriptHidden[processedTr] = hidden
                
                self.transcriptsKeywords.append(keywords)
                counter = counter + 1
            
        print("Finished Processing")