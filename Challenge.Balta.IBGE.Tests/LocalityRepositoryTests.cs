using Challenge.Balta.IBGE.Infra.Data.Context;
using Challenge.Balta.IBGE.Infra.Repository;
using Chanllenge.Balta.IBGE.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Challenge.Balta.IBGE.Tests
{
    [TestFixture]
    public class IbgeRepositoryIntegrationTests
    {
        private AppDbContext _dbContext;
        private LocalityRepository _ibgeRepository;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
           .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
           .Options;

            _dbContext = new AppDbContext(options);
            _dbContext.Database.EnsureCreated();

            // Configuração em memória
           var  _configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();


            _ibgeRepository = new LocalityRepository(_dbContext, _configuration);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }


        [Test]
        public async Task Should_Remove_EntityById()
        {
            // Arrange
            var ibge = new Ibge { Id = "4319307", State = "SP", City = "São Paulo"};
            await _dbContext.Ibge.AddAsync(ibge);
            await _dbContext.SaveChangesAsync();

            // Act
            await _ibgeRepository.Delete(ibge.Id);

            // Assert
            var deletedIbge = await _dbContext.Ibge.FirstOrDefaultAsync(x => "4319307".Equals(x.Id));
            Assert.That(deletedIbge, Is.Null, "A entidade deveria ter sido excluída");
        }

        [Test]
        public async Task Should_Inserts_Ibge_Entity()
        {
            // Arrange
            var ibge = new Ibge { Id = "4319307", State = "SP", City = "São Paulo" };

            // Act
            await _ibgeRepository.Insert(ibge);

            // Assert
            var insertedIbge = await _dbContext.Ibge.FirstOrDefaultAsync(x => "4319307".Equals(x.Id));
            Assert.That(insertedIbge, Is.EqualTo(ibge), "A entidade deveria ter sindo inserida");
        }

        [Test]
        public async Task Search_ShouldReturnMatchingIbgeRecords()
        {
            // Arrange
            var ibgeData = new List<Ibge>
            {
                new Ibge { Id = "4319307", State = "SP", City = "Sao Paulo" },
                new Ibge { Id = "3304557", State = "RJ", City = "Rio de Janeiro" },
            };

            await _dbContext.Ibge.AddRangeAsync(ibgeData);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _ibgeRepository.Search("4319307");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.Multiple(() =>
            {
                Assert.That(result[0].Id, Is.EqualTo("4319307"));
                Assert.That(result[0].State, Is.EqualTo("SP"));
                Assert.That(result[0].City, Is.EqualTo("Sao Paulo"));
            });
        }

        [Test]
        public async Task Should_Update_IbgeEntity()
        {
            // Arrange
            var ibge = new Ibge { Id = "4319307", State = "SP", City = "Sao Paulo" };
            await _dbContext.Ibge.AddAsync(ibge);
            await _dbContext.SaveChangesAsync();

            // Modificar o objeto Ibge
            ibge.City = "São Paulo";

            // Act
            await _ibgeRepository.Update(ibge);

            // Assert
            var updatedIbge = await _dbContext.Ibge.FirstOrDefaultAsync(x => "4319307".Equals(x.Id));
            Assert.That(updatedIbge, Is.Not.Null, "Deveria exister um registro.");
            Assert.That(updatedIbge.City, Is.EqualTo("São Paulo"), "Deveria ter atualizado de Sao Paulo para São Paulo.");
        }

        //[Test]
        //public async Task InsertBatch_InsertsLocalities()
        //{
        //    // Arrange
        //    var localities = new List<Ibge>
        //    {
        //        new Ibge { Id = "4319307", State = "SP", City = "Sao Paulo" },
        //        new Ibge { Id = "3304557", State = "RJ", City = "Rio de Janeiro" },
        //        // Add more localities as needed
        //    };

        //    // Act
        //    await _ibgeRepository.InsertBatch(localities);

        //    // Assert
        //    foreach (var locality in localities)
        //    {
        //        var insertedLocality = await _dbContext.Ibge.FindAsync(locality.Id);
        //        Assert.IsNotNull(insertedLocality);
        //        Assert.AreEqual(locality.State, insertedLocality.State);
        //        Assert.AreEqual(locality.City, insertedLocality.City);
        //    }
        //}

    }
}
