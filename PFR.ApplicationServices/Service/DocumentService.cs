using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using PFR.ApplicationServices.Interfaces;
using PFR.Core;
using PFR.Core.Models;
using PFR.Infrastructure.EntityFramework;
using Document = PFR.Core.Entity.Document;

namespace PFR.ApplicationServices.Service
{
    public class DocumentService : IDocumentService
    {
        private readonly PfrContextDbContextFactory _dbContextFactory;

        public DocumentService(PfrContextDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task AddDocument(DocumentModel documentModel)
        {
            var document = new Document(documentModel);
            var dbContext = _dbContextFactory.GetContext();
            dbContext.Add(document);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Document> GenerateDocument(int documentId,
            string placeHoldersKeyValues)
        {
            var dbContext = _dbContextFactory.GetContext();
            var document = await dbContext.GetDocument(documentId);
            string docText;
            var path = $"{Environment.CurrentDirectory}/templates/{document.Name}";
            using (var wordDoc = WordprocessingDocument.Open(path, true))
            {
                using (var sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = await sr.ReadToEndAsync();
                }
            }

            var documentPlaceholders = document.Placeholders.Split(ApplicationConstants.PlaceholdersSeparator);
            SplitIntoDictionary(placeHoldersKeyValues, out var placeHolders);

            foreach (var documentPlaceholder in documentPlaceholders)
            {
                var value = placeHolders[documentPlaceholder];
                docText = docText.Replace(documentPlaceholder, value);
            }

            var pathToWrite = $"./generated/{document.Name}";
            EnsureFileExists(pathToWrite, document);
            using var generatedWordDoc = WordprocessingDocument.Open(pathToWrite, true);
            await using (var sw = new StreamWriter(generatedWordDoc.MainDocumentPart.GetStream(FileMode.Create)))
            {
                await sw.WriteAsync(docText);
            }

            return document;
        }

        public async Task<DocumentModel[]> ListDocuments()
        {
            var dbContext = _dbContextFactory.GetContext();
            var documents = await dbContext.GetAllDocuments();
            return documents.Select(doc => new DocumentModel()
            {
                Id = doc.Id,
                Name = doc.Name,
                Placeholders = doc.Placeholders.Split(ApplicationConstants.PlaceholdersSeparator).ToArray()
            }).ToArray();
        }

        private static void SplitIntoDictionary(string placeHoldersKeyValues,
            out Dictionary<string, string> placeHolders)
        {
            var keyValuePairs = placeHoldersKeyValues.Split(ApplicationConstants.PlaceholdersSeparator);
            placeHolders = keyValuePairs.Select(keyValuePair =>
                    keyValuePair.Split(ApplicationConstants.KeyValueSeparator))
                .ToDictionary(s => s[0], s => s[1]);
        }

        private static void EnsureFileExists(string pathToWrite, Document document)
        {
            if (!Directory.Exists("generated"))
            {
                Directory.CreateDirectory("generated");
            }

            if (!File.Exists(pathToWrite))
            {
                File.Copy($"./templates/{document.Name}", pathToWrite);
            }
        }
    }
}