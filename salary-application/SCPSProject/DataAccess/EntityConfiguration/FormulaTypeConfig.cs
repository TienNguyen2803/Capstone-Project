using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EntityConfiguration
{
    public class FormulaTypeConfig : IEntityTypeConfiguration<FormulaType>
    {
        public void Configure(EntityTypeBuilder<FormulaType> builder)
        {
            builder.ToTable("FormulaTypes").HasKey(_ => _.FormulaDetailId);

            builder.HasOne(_ => _.Formula)
                .WithMany(_ => _.FormulaTypes)
                .HasForeignKey(_ => _.FormulaId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
