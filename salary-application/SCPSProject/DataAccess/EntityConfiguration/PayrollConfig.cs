using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EntityConfiguration
{
    public class PayrollConfig : IEntityTypeConfiguration<Payroll>
    {
        public void Configure(EntityTypeBuilder<Payroll> builder)
        {
            builder.ToTable("Payrolls").HasKey(_ => _.Id);
            builder.Property(p => p.Status).HasDefaultValue(PayrollStatus.New);
            builder.Property(p => p.Id).IsRequired();

            //1-n monthly salary component
            //1-n payslip
            //n-1 Document
            builder.HasOne(z => z.Document)
               .WithMany(c => c.Payrolls)
               .HasForeignKey(z => z.DocId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
