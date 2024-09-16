using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;

namespace DataAccessLayer.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
	    private readonly IDbWrapper<Company> _companyDbWrapper;

	    public CompanyRepository(IDbWrapper<Company> companyDbWrapper)
	    {
		    _companyDbWrapper = companyDbWrapper;
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            return await _companyDbWrapper.FindAllAsync();
        }

        public async Task<Company> GetByCodeAsync(string companyCode)
        {
            var companies = await _companyDbWrapper.FindAsync(t => t.CompanyCode.Equals(companyCode));
            return companies.FirstOrDefault();
        }

        public async Task<bool> DeleteCompanyAsync(string companyCode)
        {
            if (string.IsNullOrWhiteSpace(companyCode))
                throw new ArgumentException("Company code cannot be null or empty.", nameof(companyCode));

            // Attempt to delete the company by its code
            var isDeleted = await _companyDbWrapper.DeleteAsync(t => t.CompanyCode.Equals(companyCode));

            // Return true if the company was successfully deleted, otherwise false
            return isDeleted;
        }



        public async Task<bool> SaveCompanyAsync(Company company)
        {
            // Find existing company based on the code and site ID
            var companies = await _companyDbWrapper.FindAsync(t =>
                t.SiteId.Equals(company.SiteId) && t.CompanyCode.Equals(company.CompanyCode));

            var itemRepo = companies.FirstOrDefault();

            if (itemRepo != null)
            {
                itemRepo.CompanyName = company.CompanyName;
                itemRepo.AddressLine1 = company.AddressLine1;
                itemRepo.AddressLine2 = company.AddressLine2;
                itemRepo.AddressLine3 = company.AddressLine3;
                itemRepo.Country = company.Country;
                itemRepo.EquipmentCompanyCode = company.EquipmentCompanyCode;
                itemRepo.FaxNumber = company.FaxNumber;
                itemRepo.PhoneNumber = company.PhoneNumber;
                itemRepo.PostalZipCode = company.PostalZipCode;
                itemRepo.LastModified = company.LastModified;
                return await _companyDbWrapper.UpdateAsync(itemRepo);
            }

            // Insert new company if it doesn't exist
            return await _companyDbWrapper.InsertAsync(company);
        }
    }
}
