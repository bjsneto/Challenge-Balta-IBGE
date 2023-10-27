using Challenge.Balta.IBGE.Domain.Entities;

namespace Challenge.Balta.IBGE.Domain.Interfaces
{
    public interface IFileImportRepository
    {
        Task<bool> HasFileImport(string hash);
        Task InsertFileHash(FileImport fileImport);
    }
}
