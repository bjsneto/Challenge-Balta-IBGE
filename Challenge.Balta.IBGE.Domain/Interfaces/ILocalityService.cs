using Chanllenge.Balta.IBGE.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Chanllenge.Balta.IBGE.Domain.Interfaces
{
    public interface ILocalityService
    {
        Task Insert(Ibge ibge);
        Task Update(Ibge ibge);
        Task Delete(string id);
        Task<IList<Ibge>> Search(string search);
        Task<int> ProcessExcelFileAsync(IFormFile excelFile);

    }
}
