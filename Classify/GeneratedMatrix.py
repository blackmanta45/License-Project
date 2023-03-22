#TensorFlow is tested and supported on the following 64-bit systems: Python 3.5–3.8 'https://www.tensorflow.org/install' -  30.11.2020
import tensorflow.compat.v1 as tf
import tensorflow_hub as hub
import numpy as np

class GeneratedMatrix:
    def __init__(self, wikiTrainer, videosTranscripts, config):
        tf.compat.v1.disable_v2_behavior()
        # Sometimes data loaded have problems. You have to go in Temp files delete tf_hub folder where this is downloaded.
        self.wikiTrainer = wikiTrainer
        self.videosTranscripts = videosTranscripts
        self.config = config

        self.X = []
        self.Z = []
        self.Q = []
        self.T = []
        self.K = []

    def chunkBy(self, lst, n):
        """Yield successive n-sized chunks from lst."""
        for i in range(0, len(lst), n):
            yield lst[i:i + n]

    def embed1dchunk(self, text):
        embeddedReturn = None
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
            embeddedReturn = session.run(embedded_text, feed_dict={text_input: text})
            session.close()
        del g, session, text_input, embed, embedded_text, init_op
        tf.keras.backend.clear_session()
        tf.reset_default_graph()
        return embeddedReturn

    def embed1dTensor(self, text):
        chunks = list(self.chunkBy(text, 100))
        toReturn = self.embed1dchunk(chunks[0])
        for chunk in chunks[1:]:
            toReturn = np.concatenate((toReturn, self.embed1dchunk(chunk)), axis = 0)

        return toReturn

    def configure(self):
        self.X = self.embed1dchunk(self.wikiTrainer.wikiTrain)
        self.Z = self.embed1dchunk(self.wikiTrainer.wikiEvalTrain)
        self.Q = self.embed1dchunk(self.wikiTrainer.wikiFinalTrain)
        self.T = self.embed1dTensor(self.videosTranscripts.transcripts)
        self.K = self.embed1dTensor(self.videosTranscripts.transcriptsKeywords)

        counter = 0
        for i in range(0, len(self.T)):
            self.videosTranscripts.dictTranscriptEmbed[self.videosTranscripts.dictIndexTranscript[counter]] = self.T[i]
            counter = counter + 1