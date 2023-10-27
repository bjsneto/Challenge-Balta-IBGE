using Challenge.Balta.IBGE.Domain.Entities;
using Challenge.Balta.IBGE.Domain.Interfaces;
using Challenge.Balta.IBGE.Service.ExtensionMethods;
using Chanllenge.Balta.IBGE.Domain.Entities;
using Chanllenge.Balta.IBGE.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

namespace Challenge.Balta.IBGE.Service.Services
{
    public class LocalityService : ILocalityService
    {
        private readonly ILocalityRepository _localityRepository;
        private readonly IFileImportRepository _fileImportRepository;
        public LocalityService(ILocalityRepository localityRepository, IFileImportRepository fileImportRepository)
        {
            _localityRepository = localityRepository;
            _fileImportRepository = fileImportRepository;
        }

        #region Location CRUD
        public async Task Delete(string id)
        {
            await _localityRepository.Delete(id);
        }

        public async Task<IList<Ibge>> Search(string search)
        {
            return await _localityRepository.Search(search);
        }

        public async Task Insert(Ibge ibge)
        {
            await _localityRepository.Insert(ibge);
        }

        public async Task Update(Ibge ibge)
        {
            await _localityRepository.Update(ibge);
        }
        #endregion

        #region Process Excel File EPPLUS
        public async Task<int> ProcessExcelFileAsync(IFormFile excelFile)
        {
            using var stream = new MemoryStream();
            await excelFile.CopyToAsync(stream);

            string hash = stream.CalculateFileHash();

            string fileName = excelFile.FileName;

            var hasFileImport = await _fileImportRepository.HasFileImport(hash);

            if (hasFileImport)
                throw new Exception("File already imported. Import another data source.");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage(stream);

            var worksheetStates = package.Workbook.Worksheets[0];
            var worksheetCitys = package.Workbook.Worksheets[1];

            var states = ParseDataStates(worksheetStates);

            var citys = ParseDataCitys(worksheetCitys);

            var localities = from state in states
                             join city in citys on state.CodeUf equals city.CodeUf
                             select new Ibge
                             {
                                 Id = city.CodeCounty,
                                 City = city.NameCounty,
                                 State = state.AcronymUf
                             };

           int rowsInserted = await _localityRepository.InsertBatch(localities);

            var file = new FileImport
            {
                FileName = fileName,
                Hash = hash,
                UploadedAt = DateTime.UtcNow,
            };

            await _fileImportRepository.InsertFileHash(file);

            return rowsInserted;
        }
        #endregion

        #region Private Methods
        private static IEnumerable<dynamic> ParseDataStates(ExcelWorksheet worksheetStates)
        {
            return Enumerable.Range(2, worksheetStates.Dimension.Rows - 1)
               .Select(row => new
               {
                   CodeUf = worksheetStates.Cells[row, 1].Text,
                   AcronymUf = worksheetStates.Cells[row, 2].Text
               });
        }

        private static IEnumerable<dynamic> ParseDataCitys(ExcelWorksheet worksheetCounties)
        {
            return Enumerable.Range(2, worksheetCounties.Dimension.Rows - 1)
                .Select(row => new
                {
                    CodeCounty = worksheetCounties.Cells[row, 1].Text,
                    NameCounty = worksheetCounties.Cells[row, 2].Text,
                    CodeUf = worksheetCounties.Cells[row, 3].Text
                });
        }
        #endregion
    }
}
