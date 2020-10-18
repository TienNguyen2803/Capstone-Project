using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EntityConfiguration
{
    public class SalaryComponentConfig : IEntityTypeConfiguration<SalaryComponent>
    {
        public void Configure(EntityTypeBuilder<SalaryComponent> builder)
        {
            builder.ToTable("SalaryComponents").HasKey(_ => _.Id);

            builder.Property(p => p.EmpId).IsRequired();
            builder.Property(p => p.FieldId).IsRequired();
            builder.Property(p => p.Value).IsUnicode().HasMaxLength(255);
            builder.Property(p => p.Id).IsRequired();

            //1-n employee
            builder.HasOne(z => z.Employee)
                .WithMany(c => c.SalaryComponents)
                .HasForeignKey(z => z.EmpId)
                .OnDelete(DeleteBehavior.Cascade);

            //1-n field
            builder.HasOne(z => z.Field)
                .WithMany(c => c.SalaryComponents)
                .HasForeignKey(z => z.FieldId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
