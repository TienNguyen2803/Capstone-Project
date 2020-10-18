using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EntityConfiguration
{
    public class PayslipConfig : IEntityTypeConfiguration<Payslip>
    {
        public void Configure(EntityTypeBuilder<Payslip> builder)
        {
            builder.ToTable("Payslips").HasKey(_ => _.Id);

            builder.Property(p => p.Id).IsRequired();
            builder.Property(_ => _.Status).HasDefaultValue(true);

            //1-n payroll
            builder.HasOne(z => z.Payroll)
               .WithMany(c => c.Payslips)
               .HasForeignKey(z => z.PayrollId)
               .OnDelete(DeleteBehavior.Cascade);

            //1-n employee
            builder.HasOne(z => z.Employee)
               .WithMany(c => c.Payslips)
               .HasForeignKey(z => z.EmpId)
               .OnDelete(DeleteBehavior.Cascade);

            //1-n monthly
        }
    }
}
