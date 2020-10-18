using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EntityConfiguration
{
    public class ReferenceTableTypeConfig : IEntityTypeConfiguration<ReferenceTableType>
    {
        public void Configure(EntityTypeBuilder<ReferenceTableType> builder)
        {
            builder.ToTable("ReferenceTableTypes").HasKey(_ => _.FormulaDetailId);

            builder.HasOne(_ => _.ReferenceTable)
                .WithMany(_ => _.ReferenceTableTypes)
                .HasForeignKey(_ => _.RefenceTableTypeId);
        }
    }
}
