using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EntityConfiguration
{
    public class DocumentConfig : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.ToTable("Documents").HasKey(_ => _.Id);


            builder.Property(p => p.Code).HasMaxLength(50).IsUnicode();
            builder.Property(p => p.Description).IsUnicode();
            builder.Property(p => p.Status).HasDefaultValue(DocStatus.Deactive);
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.DocumentUrl);
            builder.Property(p => p.FormulaId);

            //0 - 1 formula
            //1 - n payroll

        }
    }
}
