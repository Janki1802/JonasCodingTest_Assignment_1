using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class EmployeeController : ApiController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper, ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET api/<controller>
        [HttpGet]
        public async Task<IHttpActionResult> GetAllAsync()
        {
            try
            {
                var items = await _employeeService.GetAllEmployeesAsync();
                return Ok(_mapper.Map<IEnumerable<EmployeeDto>>(items));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all employees");
                return InternalServerError(ex);
            }
        }

        // GET api/<controller>/5
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync(string code)
        {
            try
            {
                var item = await _employeeService.GetEmployeeByCodeAsync(code);
                if (item == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<EmployeeDto>(item));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting employee by company code {code}");
                return InternalServerError(ex);
            }
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IHttpActionResult> PostAsync([FromBody] EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var employee = _mapper.Map<EmployeeInfo>(employeeDto);
                await _employeeService.AddEmployeeAsync(employee);
                return CreatedAtRoute("DefaultApi", new { code = employee.EmployeeCode }, employeeDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new employee");
                return InternalServerError(ex);
            }
        }
    }
}