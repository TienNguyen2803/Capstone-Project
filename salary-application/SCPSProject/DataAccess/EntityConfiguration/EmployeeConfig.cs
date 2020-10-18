using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EntityConfiguration
{
    public class EmployeeConfig : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees").HasKey(_ => _.Code);

            builder.Property(p => p.Code).IsRequired().HasMaxLength(20);
            builder.Property(p => p.Fullname).IsUnicode().HasMaxLength(255);
            builder.Property(p => p.Gender).IsUnicode().HasMaxLength(50);
            builder.Property(p => p.Phone).HasMaxLength(15);
            builder.Property(p => p.Email).HasMaxLength(320);
            builder.Property(p => p.Address).IsUnicode();
            builder.Property(p => p.IsWorking).HasDefaultValue(true);
            builder.Property(p => p.StartDate);
            builder.Property(p => p.IsForeigner).HasDefaultValue(false);

            builder.HasOne(_ => _.Department)
                .WithMany(_ => _.Employees)
                .HasForeignKey(_ => _.DepartmentId);

            builder.HasOne(_ => _.Account)
               .WithOne(_ => _.Employee)
               .HasForeignKey<Account>(_ => _.Code);

            //1-n Monthly salary component
            //1-n salary component
            //1-1 account
            //1-n payslip

        }
    }
}
