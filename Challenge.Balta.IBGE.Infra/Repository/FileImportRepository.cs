using Challenge.Balta.IBGE.Domain.Entities;
using Challenge.Balta.IBGE.Domain.Interfaces;
using Challenge.Balta.IBGE.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Challenge.Balta.IBGE.Infra.Repository
{
    public class FileImportRepository : IFileImportRepository
    {
        private readonly AppDbContext _context;

        public FileImportRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> HasFileImport(string hash)
        {
            return await _context.FileImports.AnyAsync(x => hash.Equals(x.Hash));
        }

        public async Task InsertFileHash(FileImport fileImport)
        {
            await _context.FileImports.AddAsync(fileImport);
            await _context.SaveChangesAsync();
        }
    }
}
