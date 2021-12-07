using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PFR.ApplicationServices.Interfaces;
using PFR.Core.Entity;
using PFR.Core.Models;

namespace PFRAppAngular.Controllers
{
    [Route("organization")]
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
        
        [HttpGet("get-all")]
        public async Task<List<OrganizationModel>> GetOrganizations()
        {
            return await _organizationService.GetOrganizations();
        }

        [HttpPost("add")]
        public async Task<List<OrganizationModel>> AddOrganization(string organizationName)
        {
            return await _organizationService.AddOrganization(organizationName);
        }
    }
}