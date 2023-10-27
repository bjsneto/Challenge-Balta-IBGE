using Challenge.Balta.IBGE.Domain.Interfaces;
using Challenge.Balta.IBGE.Service.Services;
using Chanllenge.Balta.IBGE.Domain.Entities;
using Chanllenge.Balta.IBGE.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Challenge.Balta.IBGE.Tests
{
    [TestFixture]
    public class LocalityServiceTests
    {
        private LocalityService _ibgeService;
        private Mock<ILocalityRepository> _ibgeRepository;
        private Mock<IFileImportRepository> _fileImportRepository;
        private string _filePath;
        private IFormFile _fakeExcelFile;

        [SetUp]
        public void SetUp()
        {
            _ibgeRepository = new Mock<ILocalityRepository>();
            _fileImportRepository = new Mock<IFileImportRepository>();
            _ibgeService = new LocalityService(_ibgeRepository.Object, _fileImportRepository.Object);

            var sourceFilePath = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Municipios", "insert_sql.xlsx");

            _filePath = Path.GetTempFileName();
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
            File.Copy(sourceFilePath, _filePath);
        }


        [TearDown]
        public void Cleanup()
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }

        }

        [Test]
        public async Task Should_Insert_CallsRepositoryInsert_WithCorrectArguments()
        {
            // Arrange
            var ibgeToInsert = new Ibge { Id = "4319307", State = "SP", City = "São Paulo" };

            // Act
            await _ibgeService.Insert(ibgeToInsert);

            // Assert
            _ibgeRepository.Verify(r => r.Insert(It.Is<Ibge>(i => i.Id == "4319307" && i.State == "SP" && i.City == "São Paulo")), Times.Once);
        }

        [Test]
        public async Task Should_Update_RepositoryUpdate_WithCorrectArguments()
        {
            // Arrange
            var ibgeToUpdate = new Ibge { Id = "4319307", State = "SP", City = "São Paulo" };

            // Act
            await _ibgeService.Update(ibgeToUpdate);

            // Assert
            _ibgeRepository.Verify(r => r.Update(It.Is<Ibge>(i => i.Id == "4319307" && i.State == "SP" && i.City == "São Paulo")), Times.Once);
        }


        [Test]
        public async Task Should_Delete_Calls_RepositoryDelete()
        {
            // Arrange
            string code = "4319307"; 

            // Act
            await _ibgeService.Delete(code);

            // Assert
            _ibgeRepository.Verify(r => r.Delete(code), Times.Once);
        }

        [Test]
        public async Task ProcessExcelFileAsync_ValidFile_ProcessesDataCorrectly()
        {
            using (var stream = new FileStream(_filePath, FileMode.Open))
            {
                _fakeExcelFile = new FormFile(stream, 0, stream.Length, "localidade", "SQL INSERTS - API de localidades IBGE.xlsx");

                await _ibgeService.ProcessExcelFileAsync(_fakeExcelFile);

                _ibgeRepository.Verify(r => r.InsertBatch(It.IsAny<IEnumerable<Ibge>>()), Times.Once);

            }
        }

    }
}