using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EntityConfiguration
{
    public class FieldTypeConfig : IEntityTypeConfiguration<FieldType>
    {
        public void Configure(EntityTypeBuilder<FieldType> builder)
        {
            builder.ToTable("FieldTypes").HasKey(_ => _.FormulaDetailId);

            builder.HasOne(_ => _.Field)
                .WithMany(_ => _.FieldTypes)
                .HasForeignKey(_ => _.FieldId);
        }
    }
}
