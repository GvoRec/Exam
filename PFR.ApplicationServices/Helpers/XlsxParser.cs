using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using PFR.Core.Models;

namespace PFR.ApplicationServices.Helpers
{
    public class XlsxParser
    {
        private readonly byte[] _fileAsBytes;
        public XlsxParser(byte[] fileAsBytes)
        {
            _fileAsBytes = fileAsBytes;
        }

        public AddEmployeeModel[] ParseEmployeesFromExcel(out Guid organizationGuid)
        {
            using var stream = new MemoryStream(_fileAsBytes);
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.First();
            var rowsCount = worksheet.Rows().Count();
            var result = new List<AddEmployeeModel>();
            
            var organizationId = Guid.Empty;

            for (var rowNumber = 2; rowNumber < rowsCount; rowNumber++)
            {
                
                var currentRow = worksheet.Row(rowNumber);
                var surname = currentRow.Cell(2).Value.ToString();
                var name = currentRow.Cell(3).Value.ToString();
                var guid = currentRow.Cell(1).Value.ToString();
                organizationId = Guid.Parse(guid);
                var patronomic = currentRow.Cell(4).Value.ToString();
                var department = currentRow.Cell(5).Value.ToString();
                var division = currentRow.Cell(6).Value.ToString();
                var position = currentRow.Cell(7).Value.ToString();
                var code = int.Parse(currentRow.Cell(8).Value.ToString());
                result.Add(new AddEmployeeModel
                {
                    Code = code,
                    Department = department,
                    Division = division,
                    Name = name,
                    Patronymic = patronomic,
                    Position = position,
                    Surname = surname
                });
            }

            organizationGuid = organizationId;
            return result.ToArray();
        }
    }
}
