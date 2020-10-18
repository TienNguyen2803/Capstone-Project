using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EntityConfiguration
{
    public class PositionDetailConfig : IEntityTypeConfiguration<PositionDetail>
    {
        public void Configure(EntityTypeBuilder<PositionDetail> builder)
        {
            builder.ToTable("PositionDetails").HasKey(_ => _.Id);

            builder.HasOne(_ => _.Employee)
                .WithMany(_ => _.PositionDetails)
                .HasForeignKey(_ => _.EmpCode);

            builder.HasOne(_ => _.Position)
                .WithMany(_ => _.PositionDetails)
                .HasForeignKey(_ => _.PositionId);
        }
    }
}
