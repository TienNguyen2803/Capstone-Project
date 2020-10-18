using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EntityConfiguration
{
    public class DepartmentConfig : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments").HasKey(_ => _.Id);

            builder.Property(_ => _.DepName).HasMaxLength(255).IsUnicode();
            builder.Property(_ => _.DepOffice).HasMaxLength(255).IsUnicode();
        }
    }
}
