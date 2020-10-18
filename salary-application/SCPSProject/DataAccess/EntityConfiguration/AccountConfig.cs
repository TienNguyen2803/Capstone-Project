using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EntityConfiguration
{
    public class AccountConfig : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Accounts").HasKey(_ => _.Code);

            builder.Property(p => p.Code).IsRequired().HasMaxLength(20);
            builder.Property(_ => _.Password).HasMaxLength(20);
            builder.Property(_ => _.RoleId).HasDefaultValue(3); //default là role emp

            builder.HasOne(_ => _.Role)
                .WithMany(_ => _.Accounts)
                .HasForeignKey(_ => _.RoleId);

           
        }
    }
}
