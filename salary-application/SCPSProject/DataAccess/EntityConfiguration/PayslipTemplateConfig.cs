using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EntityConfiguration
{
    public class PayslipTemplateConfig : IEntityTypeConfiguration<PayslipTemplate>
    {
        public void Configure(EntityTypeBuilder<PayslipTemplate> builder)
        {
            builder.ToTable("PayslipTemplates").HasKey(_ => _.Id);

            builder.Property(_ => _.Status).HasDefaultValue(false);

            builder.HasOne(_ => _.Document)
                .WithMany(_ => _.PayslipTemplates)
                .HasForeignKey(_ => _.DocId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
