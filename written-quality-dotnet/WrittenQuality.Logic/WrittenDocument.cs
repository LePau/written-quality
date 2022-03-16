using System;
using WrittenQuality.Services;
using WrittenQuality;
using System.Threading.Tasks;
using System.Collections.Generic;
using WrittenQuality.Models;

namespace WrittenQuality.Logic
{
    public interface IWrittenDocument
    {
        Task<Models.WrittenDocumentAnalysis> Analyze(Models.WrittenDocument document);
    }

    public class WrittenDocument : IWrittenDocument
    {
        private readonly INlpService _nlpService;

        public WrittenDocument(INlpService nlpService)
        {
            _nlpService = nlpService;
        }

        public async Task<Models.WrittenDocumentAnalysis> Analyze(Models.WrittenDocument document)
        {
            var simplifiedText = document.Markdown.MarkdownToText().FullSentencesOnly();
            var response = await _nlpService.Analyze(simplifiedText);
            var analysis = new WrittenQuality.Models.WrittenDocumentAnalysis();

            analysis.Qualities = new List<WrittenQuality.Models.WrittenDocumentQuality>();
            analysis.Summary = simplifiedText;

            var overallDocumentSentiment = response.DocumentSentiment.Score;

            analysis.Qualities.Add(new WrittenQuality.Models.WrittenDocumentQuality()
            {
                Name = "sentiment",
                Title = "Document sentiment",
                Description = "Sentiment is generally detected off the use of negative words or hositility.  It is not always accurate, especially when averaged and with longer reads.",
                Rank = (1 + overallDocumentSentiment) / 2.0,
                Weight = 3
            });

            analysis.Qualities.Add(new WrittenQuality.Models.WrittenDocumentQuality()
            {
                Name = "length",
                Title = "Document length",
                Description = "Quality of the document length for tl;dr purposes.",
                Rank = 1 - Math.Min(1, simplifiedText.Split().Length / 10000.0), // very rough estimate algo
                Weight = 1
            });

            for (var i = 0; i < response.Sentences.Count; i++)
            {
                var sentence = response.Sentences[i];

                analysis.Qualities.Add(new WrittenQuality.Models.WrittenDocumentQuality()
                {
                    Name = $"sentence{i}",
                    Title = $"Sentence {i} sentiment",
                    Description = sentence.Text.Content,
                    Rank = (1 + sentence.Sentiment.Score) / 2.0,
                    Weight = 0
                });
            }

            return analysis;

        }
    }
}
