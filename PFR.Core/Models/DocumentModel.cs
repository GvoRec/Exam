namespace PFR.Core.Models
{
    public class DocumentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string[] Placeholders { get; set; }
    }
}