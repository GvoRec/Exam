using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PFR.ApplicationServices.Helpers;
using PFR.ApplicationServices.Interfaces;
using PFR.Core.Models;

namespace PFRAppAngular.Controllers
{
    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("list/{organizationId}")]
        public async Task<List<EmployeeModel>> GetAllEmployees(Guid organizationId)
        {
            return await _employeeService.GetAllEmployees(organizationId);
        }

        [HttpPost("import-from-excel")]
        public async Task ImportEmployeesFromExcel(IFormFile formFile)
        {
            await using var fileAsStream = formFile.OpenReadStream();
            await using var byteArrayFileStream = new MemoryStream();
            await fileAsStream.CopyToAsync(byteArrayFileStream);
            var parser = new XlsxParser(byteArrayFileStream.ToArray());
            var employees = parser.ParseEmployeesFromExcel(out var organizationGuid);
            await _employeeService.AddEmployeeBatch(organizationGuid, employees);
        }
    }
}