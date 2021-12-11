using System.Threading.Tasks;
using PFR.Core.Entity;
using PFR.Core.Models;

namespace PFR.ApplicationServices.Interfaces
{
    public interface IDocumentService
    {
        Task AddDocument(DocumentModel documentModel);

        Task<Document> GenerateDocument(int documentId, string placeHoldersValues);

        Task<DocumentModel[]> ListDocuments(string searchTerm);
    }
}