using Chanllenge.Balta.IBGE.Domain.Entities;

namespace Chanllenge.Balta.IBGE.Domain.Interfaces
{
    public interface ILocalityRepository
    {
        Task Insert(Ibge ibge);
        Task Update(Ibge ibge);
        Task Delete(string id);
        Task<int> InsertBatch(IEnumerable<Ibge> ibges);
        Task<IList<Ibge>> Search(string search);
    }
}
