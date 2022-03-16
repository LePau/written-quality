using System;
using System.Collections.Generic;
using System.Linq;

namespace WrittenQuality.Models
{
    public class WrittenDocumentQuality
    {
        public int Weight { get; set; } = 0;
        public double Rank { get; set; } = 0;
        public string Name { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
    }

    public class WrittenDocumentAnalysis
    {
        public List<WrittenDocumentQuality> Qualities { get; set; } = new List<WrittenDocumentQuality>();
        public string Summary {get;set;} = "";
        public WrittenDocumentQuality Overall
        {
            get => ComputeOverallQuality();
        }

        public double ComputeAverage()
        {
            var totalRank = this.Qualities
                .Select(q => q.Rank * q.Weight)
                .Sum();

            var totalWeight = this.Qualities
                .Select(q => q.Weight)
                .Sum();

            return Math.Round((totalRank / totalWeight) * 100) / 100.0;
        }

        private WrittenDocumentQuality ComputeOverallQuality()
        {
            var rank = this.ComputeAverage();
            var description = "This document has good sentiment and readability overall.";
            if (rank < 0.33)
            {
                description = "This document has poor sentiment (and/or readability).";
            }
            else if (rank < 0.66)
            {
                description = "This document has average sentiment or readability";
            }

            return new WrittenDocumentQuality()
            {
                Name = "overall",
                Title = "Overall",
                Description = description,
                Weight = 1,
                Rank = rank
            };
        }
    }
}