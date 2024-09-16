using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
using Microsoft.Extensions.Logging;
using WebApi.Models;


namespace WebApi.Controllers
{
    public class CompanyController : ApiController
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(ICompanyService companyService, IMapper mapper, ILogger<CompanyController> logger)
        {
            _companyService = companyService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET api/<controller>
        [HttpGet]
        public async Task<IHttpActionResult> GetAllAsync()
        {
            try
            {
                var items = await _companyService.GetAllCompaniesAsync();
                return Ok(_mapper.Map<IEnumerable<CompanyDto>>(items));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all companies");
                return InternalServerError(ex);
            }
        }

        // GET api/<controller>/5
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync(string code)
        {
            try
            {
                var item = await _companyService.GetCompanyByCodeAsync(code);
                if (item == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<CompanyDto>(item));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting company by code {code}");
                return InternalServerError(ex);
            }
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IHttpActionResult> PostAsync([FromBody] CompanyDto companyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var company = _mapper.Map<CompanyInfo>(companyDto);
                await _companyService.AddCompanyAsync(company);
                return CreatedAtRoute("DefaultApi", new { code = company.CompanyCode }, companyDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new company");
                return InternalServerError(ex);
            }
        }

        // PUT api/<controller>/5
        [HttpPut]
        public async Task<IHttpActionResult> PutAsync(string code, [FromBody] CompanyDto companyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var company = await _companyService.GetCompanyByCodeAsync(code);
                if (company == null)
                {
                    return NotFound();
                }
                _mapper.Map(companyDto, company);
                await _companyService.UpdateCompanyAsync(company);
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating company with code {code}");
                return InternalServerError(ex);
            }
        }



        // DELETE api/<controller>/5
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteAsync(string code)
        {
            try
            {
                var company = await _companyService.GetCompanyByCodeAsync(code);
                if (company == null)
                {
                    return NotFound();
                }

                await _companyService.DeleteCompanyAsync(code);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting company with code {code}");
                return InternalServerError(ex);
            }
        }
    }
}