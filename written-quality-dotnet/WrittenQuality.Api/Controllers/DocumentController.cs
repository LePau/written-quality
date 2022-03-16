using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WrittenQuality;

namespace WrittenQuality.Api.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class DocumentController : ControllerBase
    {

        private readonly ILogger<DocumentController> _logger;
        private readonly Logic.IWrittenDocument _writtenDocumentLogic;

        public DocumentController(ILogger<DocumentController> logger, Logic.IWrittenDocument writtenDocumentLogic)
        {
            _logger = logger;
            _writtenDocumentLogic = writtenDocumentLogic;
        }

        [HttpPost]
        [Route("Analyze")]
        public async Task<Models.WrittenDocumentAnalysis> Analyze([FromBody] Models.WrittenDocument document)
        {
            _logger.LogInformation($"Controller.Analyze");

            return await _writtenDocumentLogic.Analyze(document);
        }
    }
}
