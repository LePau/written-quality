using System.Collections.Generic;

namespace WrittenQuality.Models
{

    public class NlpEntity
    {
        public string Name { get; set; }
        public string Nickname { get; set; }
        public float Salience { get; set; }
        public string ReferenceUrl { get; set; }
        public string Language { get; set; }
        // Unknown = 0, Person = 1, Location = 2, Organization = 3, Event = 4, WorkOfArt = 5, ConsumerGood = 6, Other = 7,
        // PhoneNumber = 9, Address = 10, Date = 11, Number = 12, Price = 13
        public string Type { get; set; }
    }

    public class NlpOccurence
    {
        public string Word { get; set; }
        public long Count { get; set; }
    }

    public class NlpImageAnnotation
    {
        public string DetectedText { get; set; }
        public int DetectedTextSpamRating { get; set; }
        public int DetectedTextNsfwRating { get; set; }
        public int AdultRating { get; set; }
        public int MedicalRating { get; set; }
        public int NsfwRating { get; set; }
        public int RacyRating { get; set; }
        public int SpoofRating { get; set; }
        public int ViolenceRating { get; set; }
        public int CombinedSpamRating { get; set; }
        public int CombinedNsfwRating { get; set; }
    }


    public class NlpWordAnalysis
    {
        public List<NlpOccurence> Words { get; set; }
        public List<NlpOccurence> WordsInSummary { get; set; }
        public int ProbabilityOfExposure { get; set; } = 0; // min 0, max 100.  i.e. higher probability if in summary
        public int ProbabilityOfExposureInSummary { get; set; } = 0; // min 0, max 100.  i.e. higher probability if in summary
        public int ProbabilityOfExposureElsewhere { get; set; } = 0; // min 0, max 100.  i.e. higher probability if in summary
        public bool Exists { get; set; } = false;
        public long WordCount { get; set; } = 0;
    }
}