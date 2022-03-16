using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WrittenQuality.Models;
using WrittenQuality.Services;

namespace WrittenQuality.Api.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class ScrapeController : ControllerBase
    {

        private readonly ILogger<ScrapeController> _logger;
        private readonly IScrapeService _scrapeService;


        public ScrapeController(ILogger<ScrapeController> logger, IScrapeService scrapeService)
        {
            _logger = logger;
            _scrapeService = scrapeService;
        }

        [HttpGet]
        [Route("ScrapeUrl")]
        public async Task<UrlMetadata> ScrapeUrl([FromQuery, Required] string url)
        {
            _logger.LogInformation($"Controller.ScrapeUrl: {url}");

            var urlMetadata = await _scrapeService.ScrapeUrl(url);

            return urlMetadata;
        }
    }
}
