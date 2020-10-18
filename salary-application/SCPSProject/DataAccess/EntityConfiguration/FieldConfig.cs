using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EntityConfiguration
{
    public class FieldConfig : IEntityTypeConfiguration<Field>
    {
        public void Configure(EntityTypeBuilder<Field> builder)
        {
            builder.ToTable("Fields").HasKey(_ => _.Id);

            builder.Property(p => p.Name).IsUnicode().HasMaxLength(50);
            builder.Property(p => p.LongName).IsUnicode().HasMaxLength(255);
            builder.Property(p => p.DataType).IsUnicode().HasMaxLength(20);
            builder.Property(p => p.Description).IsUnicode();
            builder.Property(p => p.CellMapping).HasMaxLength(10);
            builder.Property(p => p.Status).HasDefaultValue(true);
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.SampleValue).HasDefaultValue("0");

            builder.HasIndex(_ => _.Name).IsUnique();
            //0-n formula detail
            //1-n salary component
            //1-n monthly salary component
        }
    }
}
