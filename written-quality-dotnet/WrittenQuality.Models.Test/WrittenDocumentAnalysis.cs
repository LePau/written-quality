using System;
using System.Collections.Generic;
using Xunit;

namespace WrittenQuality.Models.Test
{
    public class WrittenDocumentAnalysis
    {
        [Theory]
        [InlineData(10, new double[] { 10, 1 }, new double[] { 10, 1 })]
        [InlineData(3.12, new double[] { 9, 2 }, new double[] { 3, 100 })]
        [InlineData(3, new double[] { 9, 0 }, new double[] { 3, 100 })]
        [InlineData(0, new double[] { 9, 0 }, new double[] { 0, 100 })]
        public void ComputeAverage(double expected, params double[][] qualityValues)
        {
            var qualities = new List<WrittenQuality.Models.WrittenDocumentQuality>();
            foreach (var couple in qualityValues)
            {
                qualities.Add(new WrittenQuality.Models.WrittenDocumentQuality() { Rank = couple[0], Weight = (int)couple[1] });
            }

            var analysis = new WrittenQuality.Models.WrittenDocumentAnalysis() { Qualities = qualities };
            Assert.Equal(analysis.ComputeAverage(), expected);

        }
    }
}
