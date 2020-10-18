using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EntityConfiguration
{
    public class FormulaDetailConfig : IEntityTypeConfiguration<FormulaDetail>
    {
        public void Configure(EntityTypeBuilder<FormulaDetail> builder)
        {
            builder.ToTable("FormulaDetails").HasKey(_ => _.Id);

            builder.Property(p => p.Id).IsRequired();

            //1-n formula
            builder.HasOne(z => z.Formula)
                .WithMany(c => c.FormulaDetails)
                .HasForeignKey(z => z.FormulaId).OnDelete(DeleteBehavior.Restrict);

            //1-1 fieldtype
            builder.HasOne(_ => _.FieldType)
                .WithOne(_ => _.FormulaDetail)
                .HasForeignKey<FieldType>(_ => _.FormulaDetailId);

            //1-1 referencetabletype
            builder.HasOne(_ => _.ReferenceTableType)
                .WithOne(_ => _.FormulaDetail)
                .HasForeignKey<ReferenceTableType>(_ => _.FormulaDetailId);

            //1-1 FormulaType
            builder.HasOne(_ => _.FormulaType)
                .WithOne(_ => _.FormulaDetail)
                .HasForeignKey<FormulaType>(_ => _.FormulaDetailId);

            //1-1 ConstantType
            builder.HasOne(_ => _.ConstantType)
                .WithOne(_ => _.FormulaDetail)
                .HasForeignKey<ConstantType>(_ => _.FormulaDetailId);
        }
    }
}
