using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PFR.ApplicationServices;
using PFR.ApplicationServices.Interfaces;
using PFR.Core.Models;

namespace PFRAppAngular.Controllers
{
    [Route("api/document")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DocumentController(IDocumentService documentService, IWebHostEnvironment webHostEnvironment)
        {
            _documentService = documentService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("add")]
        public async Task AddDocument([FromBody] DocumentModel model)
        {
            await _documentService.AddDocument(model);
        }

        [HttpGet("list")]
        public async Task<DocumentModel[]> ListDocuments(string searchTerm)
        {
            return await _documentService.ListDocuments(searchTerm);
        }

        [HttpGet("generate/{documentId}")]
        public async Task<FileStreamResult> GenerateDocument(int documentId, string placeholdersValues)
        {
            var document = await _documentService.GenerateDocument(documentId, placeholdersValues);
            var path = $"generated/{document.Name}";
            var memory = new MemoryStream();
            await using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            return File(memory, FileHelper.GetContentType(), Path.GetFileName(path));
        }
    }
}