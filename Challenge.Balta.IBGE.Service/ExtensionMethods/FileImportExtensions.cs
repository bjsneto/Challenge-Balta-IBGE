using Challenge.Balta.IBGE.Domain.Entities;
using System.Security.Cryptography;

namespace Challenge.Balta.IBGE.Service.ExtensionMethods
{
    public static class FileImportExtensions
    {
        public static string CalculateFileHash(this MemoryStream stream)
        {
            byte[] hashBytes = MD5.HashData(stream.ToArray());
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}

