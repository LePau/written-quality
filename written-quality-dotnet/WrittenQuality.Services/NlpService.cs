using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.IO;
using Google.Cloud.Language.V1;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WrittenQuality.Models;
using WrittenQuality.Services;

namespace WrittenQuality.Services
{

    public interface INlpService
    {
        Task<AnnotateTextResponse> Analyze(string text);
        double CalculateMagnitudeScoreAverage(AnnotateTextResponse response, bool ignoreZeros = true);
        double CalculateSentimentScoreAverage(AnnotateTextResponse response, bool ignoreZeros = true);
    }

    public class NlpService : INlpService
    {
        private ILogger<NlpService> _logger;
        private LanguageServiceClient _client;
        private IScrapeService _scrapeService;
        public NlpService(ILogger<NlpService> logger)
        {
            _logger = logger;

            _client = LanguageServiceClient.Create();
        }


        public async Task<AnnotateTextResponse> Analyze(string text)
        {

            if (text.Split().Count() < 20)
            {
                throw new Exception("NlpService.AnalyzeEntities: GCP requires count to be more than 20 tokens.  Returning empty list");
            }

            var response = await _client.AnnotateTextAsync(new AnnotateTextRequest()
            {
                Document = new Document()
                {
                    Content = text,
                    Type = Document.Types.Type.PlainText
                },
                Features = new AnnotateTextRequest.Types.Features()
                {
                    ClassifyText = false,
                    ExtractDocumentSentiment = true,
                    ExtractEntities = false,
                    ExtractEntitySentiment = true,
                    ExtractSyntax = true
                }
            });

            // foreach (var s in response.Sentences)
            // {
            //     Console.WriteLine($"s: {s.Sentiment.Score}, m: {s.Sentiment.Magnitude}, t: {s.Text}");
            // }
            return response;
        }

        public double CalculateSentimentScoreAverage(AnnotateTextResponse response, bool ignoreZeros = true)
        {
            var sentences = response.Sentences.ToList();

            if (ignoreZeros)
            {
                sentences = sentences.Where(s => s.Sentiment.Score > 0 && s.Sentiment.Magnitude > 0).ToList();
            }

            var count = sentences.Count;
            var totalSentiment = sentences.Sum(s => s.Sentiment.Score);

            return totalSentiment / count;
        }

        public double CalculateMagnitudeScoreAverage(AnnotateTextResponse response, bool ignoreZeros = true)
        {
            var sentences = response.Sentences.ToList();

            if (ignoreZeros)
            {
                sentences = sentences.Where(s => s.Sentiment.Score > 0 && s.Sentiment.Magnitude > 0).ToList();
            }

            var count = sentences.Count;
            var totalMagnitude = sentences.Sum(s => s.Sentiment.Magnitude);

            return totalMagnitude / count;
        }
    }
}
