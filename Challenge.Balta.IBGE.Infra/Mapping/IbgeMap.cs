using Chanllenge.Balta.IBGE.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Challenge.Balta.IBGE.Infra.Mapping
{
    public class IbgeMap : IEntityTypeConfiguration<Ibge>
    {
        public void Configure(EntityTypeBuilder<Ibge> builder)
        {
            builder.ToTable("ibge"); 

            builder.HasKey(prop => prop.Id);

            builder.Property(prop => prop.Id)
                .IsRequired()
                .HasColumnName("id")
                .HasColumnType("CHAR(7)");

            builder.Property(prop => prop.State)
                .HasColumnName("state")
                .HasColumnType("CHAR(2)");

            builder.Property(prop => prop.City)
                .HasColumnName("city")
                .HasColumnType("VARCHAR(80)");

            builder.HasIndex(prop => prop.Id).IsUnique().HasName("ix_ibge_id");
            builder.HasIndex(prop => prop.City).HasName("ix_ibge_city");
            builder.HasIndex(prop => prop.State).HasName("ix_ibge_state");
        }

    }

}
