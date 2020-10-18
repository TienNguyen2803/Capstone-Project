using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EntityConfiguration
{
    public class MonthlySalaryComponentConfig : IEntityTypeConfiguration<MonthlySalaryComponent>
    {
        public void Configure(EntityTypeBuilder<MonthlySalaryComponent> builder)
        {
            builder.ToTable("MonthlySalaryComponents").HasKey(_ => _.Id);


            builder.Property(p => p.Value).HasMaxLength(255).IsUnicode();
            builder.Property(p => p.Id).IsRequired();

            //1-n field
            builder.HasOne(z => z.Field)
               .WithMany(c => c.MonthlySalaryComponents)
               .HasForeignKey(z => z.FieldId)
               .OnDelete(DeleteBehavior.Cascade);

            //1-n payroll
            builder.HasOne(z => z.Payslip)
               .WithMany(c => c.MonthlySalaryComponents)
               .HasForeignKey(z => z.PayslipId)
               .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
