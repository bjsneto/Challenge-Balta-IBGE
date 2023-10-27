using Chanllenge.Balta.IBGE.Domain.Entities;

namespace Challenge.Balta.IBGE.Domain.Entities
{
    public class FileImport : BaseEntity
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Hash { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
