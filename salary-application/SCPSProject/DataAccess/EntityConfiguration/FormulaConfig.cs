using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EntityConfiguration
{
    public class FormulaConfig : IEntityTypeConfiguration<Formula>
    {
        public void Configure(EntityTypeBuilder<Formula> builder)
        {
            builder.ToTable("Formulas").HasKey(_ => _.Id);

            builder.Property(p => p.Description).IsUnicode();
            builder.Property(p => p.Name).HasMaxLength(50).IsUnicode();
            builder.Property(p => p.IsSalaryFormula).HasDefaultValue(false);
            builder.Property(p => p.Status).HasDefaultValue(true);
            builder.Property(p => p.Type).HasDefaultValue(1);
            builder.Property(p => p.Id).IsRequired();

            builder.HasIndex(_ => _.Name).IsUnique();
            //1-1 document
            builder.HasOne(z => z.Document)
                .WithOne(c => c.Formula)
                .HasForeignKey<Document>(z => z.FormulaId);

            //1-n formula detail (formulaID)
            //1-n formula detail (refId)

        }
    }
}
