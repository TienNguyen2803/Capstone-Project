using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;


namespace DataAccess.EntityConfiguration
{
    public class ReferenceTableConfig : IEntityTypeConfiguration<ReferenceTable>
    {
        public void Configure(EntityTypeBuilder<ReferenceTable> builder)
        {
            builder.ToTable("ReferenceTables").HasKey(k => k.Id);

            builder.Property(p => p.Name).HasMaxLength(50).IsUnicode();
            builder.Property(p => p.ReturnType).HasMaxLength(20).IsUnicode();
            builder.Property(p => p.Description).IsUnicode();
            builder.Property(p => p.Status).HasDefaultValue(true);
            builder.Property(p => p.Id).IsRequired();

            builder.HasIndex(_ => _.Name).IsUnique();

            //0..1 - M FormulaDetail
            //0..1 - M DecisionDetail
        }
    }
}
