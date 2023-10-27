using Challenge.Balta.IBGE.Infra.Data.Context;
using Chanllenge.Balta.IBGE.Domain.Entities;
using Chanllenge.Balta.IBGE.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace Challenge.Balta.IBGE.Infra.Repository
{
    public class LocalityRepository : ILocalityRepository
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public LocalityRepository(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        #region Location CRUD
        public async Task Delete(string id)
        {
            Ibge? ibge = await Select(id) ?? throw new Exception("Location not registered");

            try
            {
                _context.Ibge.Remove(ibge);
                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {
                throw new Exception("An internal error has occurred");
            }
        }

        public async Task<IList<Ibge>> Search(string search)
        {
            try
            {
                return await _context.Ibge
                    .AsNoTracking()
                    .Where(x => search.Equals(x.Id) || search.Equals(x.State)  || search.Equals(x.City))
                    .ToListAsync();

            }
            catch (Exception)
            {
                throw new Exception("An internal error has occurred");
            }
        }

        public async Task Insert(Ibge ibge)
        {
            Ibge? ibgeRegistered = await Select(ibge.Id);

            if (ibgeRegistered is not null)
                throw new Exception("There is already a locality registered with this code.");

            try
            {
                await _context.Ibge.AddAsync(ibge);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("An internal error has occurred");
            }
        }

        public async Task Update(Ibge ibge)
        {
            try
            {
                _context.Entry(ibge).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("An internal error has occurred");
            }
        }
        #endregion

        #region Inserting locations in batch
        public async Task<int> InsertBatch(IEnumerable<Ibge> localities)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using NpgsqlConnection connection = new(connectionString);
            await connection.OpenAsync();

            await CreateTempTable(connection);

            try
            {
                using var writer = await CopyBinaryImport(connection);

                foreach (var locality in localities)
                {
                    await writer.StartRowAsync();
                    await writer.WriteAsync(locality.Id, NpgsqlDbType.Text);
                    await writer.WriteAsync(locality.State, NpgsqlDbType.Text);
                    await writer.WriteAsync(locality.City, NpgsqlDbType.Text);
                }

                await writer.CompleteAsync();

                await writer.DisposeAsync();

                return await InsertIntoLocality(connection);
            }
            catch (Exception)
            {
                throw new Exception($"An internal error has occurred");
            }
        }
        #endregion

        #region Private Methods
        private static async Task<int> InsertIntoLocality(NpgsqlConnection connection)
        {
            using var cmd = new NpgsqlCommand("INSERT INTO ibge (id, state, city) SELECT id, state, city FROM ibge_temp WHERE NOT EXISTS (SELECT 1 FROM ibge WHERE ibge.id = ibge_temp.id)", connection);
            return await cmd.ExecuteNonQueryAsync();
        }
        private static async Task<NpgsqlBinaryImporter> CopyBinaryImport(NpgsqlConnection connection)
        {
            return await connection.BeginBinaryImportAsync("COPY ibge_temp (id, state, city) FROM STDIN (FORMAT BINARY)");
        }
        private static async Task CreateTempTable(NpgsqlConnection connection)
        {
            using var cmd = new NpgsqlCommand("CREATE TEMP TABLE ibge_temp (id text, state text, city text)", connection);
            await cmd.ExecuteNonQueryAsync();
        }
        private async Task<Ibge?> Select(string id) => await _context.Ibge.FirstOrDefaultAsync(x => id.Equals(x.Id));
        #endregion
    }
}
