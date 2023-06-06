using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAMPSServer.Models
{

    public class PredictionQuery
    {
        public String text { get; set; }

        public PredictionQuery() 
        {
        }
    }

    public class Prediction
    {
        public String CitationSentence { get; set; }
        public String EvidenceSentence { get; set; }
        public String FindingSentence { get; set; }
        public String LegalRuleSentence { get; set; }
        public String ReasoningSentence { get; set; }
        public String Sentence { get; set; }
        public Prediction() 
        {
        }
    }

    public class PredictionResult
    {
        public String text { get; set; }
        public String name { get; set; }
        public String classification { get; set; }
        public Prediction predictions { get; set; }

        public PredictionResult() 
        {
            predictions = new Prediction();
        }
    }
}
