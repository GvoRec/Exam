using PFR.Core.Models;

namespace PFR.Core.Entity
{
    public class Document
    {
        private Document()
        {
            
        }
        public Document(DocumentModel documentModel)
        {
            Name = documentModel.Name;
            Placeholders = string.Join(ApplicationConstants.PlaceholdersSeparator, documentModel.Placeholders);
        }

        public  int Id { get; set; }
        
        public string Name { get; set; }

        public string Placeholders { get; set; }
    }
}