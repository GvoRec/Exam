using System.Collections.Generic;

namespace PFR.ApplicationServices
{
    public static class FileHelper
    {
        public static string GetContentType()
        {
            var types = GetMimeTypes();
            var ext = ".docx";
            return types[ext];
        }

        private static Dictionary<string, string> GetMimeTypes()
        {
            return new()
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
                {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}