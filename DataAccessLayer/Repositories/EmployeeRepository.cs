using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDbWrapper<Employee> _employeeDbWrapper;

        public EmployeeRepository(IDbWrapper<Employee> employeeDbWrapper)
        {
            _employeeDbWrapper = employeeDbWrapper;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _employeeDbWrapper.FindAllAsync();
        }

        public async Task<Employee> GetByCodeAsync(string companyCode)
        {
            var employeeByCompanyCode = await _employeeDbWrapper.FindAsync(t => t.CompanyCode.Equals(companyCode));
            return employeeByCompanyCode.FirstOrDefault();

        }

        public async Task<bool> SaveEmployeeAsync(Employee employee)
        {

            var employees = await _employeeDbWrapper.FindAsync(t =>
               t.SiteId.Equals(employee.SiteId) && t.CompanyCode.Equals(employee.CompanyCode));

            var itemRepo = employees.FirstOrDefault();

            if (itemRepo != null)
            {
                itemRepo.EmployeeName = employee.EmployeeName;
                itemRepo.Occupation = employee.Occupation;
                itemRepo.EmployeeStatus = employee.EmployeeStatus;
                itemRepo.EmailAddress = employee.EmailAddress;
                itemRepo.Phone = employee.Phone;
                itemRepo.LastModified = employee.LastModified;
                return await _employeeDbWrapper.UpdateAsync(itemRepo);
            }

            // Insert new employee if it doesn't exist
            return await _employeeDbWrapper.InsertAsync(employee);
        }
    }
}
