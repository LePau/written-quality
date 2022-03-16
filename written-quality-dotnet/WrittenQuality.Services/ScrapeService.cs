using System;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using WrittenQuality.Models;
using Microsoft.Extensions.Logging;

namespace WrittenQuality.Services
{
    public interface IScrapeService
    {
        Task<UrlMetadata> ScrapeUrl(string url);
    }

    public class ScrapeService : IScrapeService
    {
        private static HttpClient _httpClient = new HttpClient();
        private ILogger<ScrapeService> _logger;
        private string _metadataServiceDomain = null;
        private string _metadataServiceApiKey = null;
        public ScrapeService(ILogger<ScrapeService> logger, string metadataServiceDomain, string metadataServiceApiKey)
        {
            _logger = logger;
            _metadataServiceApiKey = metadataServiceApiKey;
            _metadataServiceDomain = metadataServiceDomain;
        }

        public async Task<UrlMetadata> ScrapeUrl(string url)
        {
            _logger.LogInformation($"Scraping URL: {url} {_metadataServiceDomain} {_metadataServiceApiKey}");

            UrlMetadata urlMetadata = new UrlMetadata();
            var escapedUrl = Uri.EscapeDataString(url);
            var host = url.HostName().ToLower();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{_metadataServiceDomain}/metadata-from-url?url={escapedUrl}&key={_metadataServiceApiKey}");
            HttpResponseMessage response = null;
            try
            {
                response = await _httpClient.SendAsync(request);
            }
            catch (Exception e)
            {
                _logger.LogWarning("The metadata program seems to have crashed because the response was terminated abruptly, or something.  See following stack trace.");
                _logger.LogError("Here is the error", e);
            }

            if (response?.IsSuccessStatusCode == true)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                urlMetadata = JsonSerializer.Deserialize<UrlMetadata>(jsonString, options);

            }


            return urlMetadata;
        }

    }
}
