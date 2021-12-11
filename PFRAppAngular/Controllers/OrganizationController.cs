using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PFR.ApplicationServices.Interfaces;
using PFR.Core.Models;

namespace PFRAppAngular.Controllers
{
    [Route("api/organization")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpGet("{id}")]
        public async Task<OrganizationModel> GetOrganization(Guid id)
        {
            return await _organizationService.GetOrganization(id);
        }

        [HttpPost("{id}/add-employee")]
        public async Task AddEmployee(Guid id, [FromBody] AddEmployeeModel model)
        {
            await _organizationService.AddEmployee(id, model);
        }

        [HttpGet("get-all")]
        public async Task<List<OrganizationModel>> GetOrganizations(string searchTerm)
        {
            return await _organizationService.GetOrganizations(searchTerm);
        }

        [HttpPost("add")]
        public async Task<List<OrganizationModel>> AddOrganization(string organizationName)
        {
            return await _organizationService.AddOrganization(organizationName);
        }
    }
}