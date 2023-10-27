using Challenge.Balta.IBGE.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Challenge.Balta.IBGE.Infra.Mapping
{
    public class FileImportMap : IEntityTypeConfiguration<FileImport>
    {
        public void Configure(EntityTypeBuilder<FileImport> builder)
        {
            builder.ToTable("file_import");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .UseIdentityColumn();
            builder.Property(e => e.FileName)
                .HasColumnName("file_name")
                .IsRequired(); 
            builder.Property(e => e.Hash)
                .HasColumnName("hash")
                .IsRequired(); 
            builder.Property(e => e.UploadedAt)
                .HasColumnName("uploaded_at")
                .IsRequired(); 

            builder.HasIndex(e => e.Hash).IsUnique();
        }
    }
}
