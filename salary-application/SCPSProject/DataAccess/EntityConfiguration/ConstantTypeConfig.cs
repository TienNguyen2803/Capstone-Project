using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EntityConfiguration
{
    public class ConstantTypeConfig : IEntityTypeConfiguration<ConstantType>
    {
        public void Configure(EntityTypeBuilder<ConstantType> builder)
        {
            builder.ToTable("ConstantTypes").HasKey(_ => _.FormulaDetailId);
        }
    }
}
