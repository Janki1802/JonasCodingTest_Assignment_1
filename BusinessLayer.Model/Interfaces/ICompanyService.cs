using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Model.Models;

namespace BusinessLayer.Model.Interfaces
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyInfo>> GetAllCompaniesAsync();
        Task<CompanyInfo> GetCompanyByCodeAsync(string companyCode);
        Task AddCompanyAsync(CompanyInfo companyInfo);
        Task UpdateCompanyAsync(CompanyInfo companyInfo);
        Task<bool> DeleteCompanyAsync(string companyCode);
    }
}
