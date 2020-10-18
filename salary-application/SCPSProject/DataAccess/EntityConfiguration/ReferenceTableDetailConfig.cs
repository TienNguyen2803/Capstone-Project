using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EntityConfiguration
{
    public class ReferenceTableDetailConfig : IEntityTypeConfiguration<ReferenceTableDetail>
    {
        public void Configure(EntityTypeBuilder<ReferenceTableDetail> builder)
        {
            builder.ToTable("ReferenceTableDetails").HasKey(_ => _.Id);

            builder.Property(p => p.Key).IsUnicode().HasMaxLength(50);
            builder.Property(p => p.Value).IsUnicode().HasMaxLength(50);

            builder.HasOne(z => z.ReferenceTable)
                .WithMany(x => x.ReferenceTableDetails)
                .HasForeignKey(z => z.ReferenceTableId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
