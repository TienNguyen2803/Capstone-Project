using DataAccess.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DataAccess.Context
{
    using DataAccess.Entities;
    using DataAccess.EntityConfiguration;
    public class CapstoneContext : DbContext, IEntityContext
    {

        public CapstoneContext(DbContextOptions<CapstoneContext> options)
            : base(options)
        {
        }

        public DbSet<ConstantType> ConstantTypes { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<FieldType> FieldTypes { get; set; }
        public DbSet<Formula> Formulas { get; set; }
        public DbSet<FormulaDetail> FormulaDetails { get; set; }
        public DbSet<FormulaType> FormulaTypes { get; set; }
        public DbSet<MonthlySalaryComponent> MonthlySalaryComponents { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<Payslip> Payslips { get; set; }
        public DbSet<ReferenceTable> ReferenceTables { get; set; }
        public DbSet<ReferenceTableDetail> ReferenceTableDetails { get; set; }
        public DbSet<ReferenceTableType> ReferenceTableTypes { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<PositionDetail> PositionDetails { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<SalaryComponent> SalaryComponents { get; set; }
        public DbSet<PayslipTemplate> PayslipTemplates { get; set; }
        public DbSet<PayrollComponent> PayrollComponents { get; set; }

        public object GetContext => this;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=SCPSDB;Trusted_Connection=False;User Id=sa;Password=123456");
                //optionsBuilder.UseSqlServer("Server=localhost;Database=spcsdb;User Id=sa;Password=1234");
                //optionsBuilder.UseSqlServer("Server=tcp:spcsdb.database.windows.net,1433;Initial Catalog=spcsdb;Persist Security Info=False;User ID=hangcindy;Password=Zaq@1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ConstantTypeConfig())
                .ApplyConfiguration(new DocumentConfig())
                .ApplyConfiguration(new EmployeeConfig())
                .ApplyConfiguration(new FieldConfig())
                .ApplyConfiguration(new FieldTypeConfig())
                .ApplyConfiguration(new FormulaConfig())
                .ApplyConfiguration(new FormulaDetailConfig())
                .ApplyConfiguration(new FormulaTypeConfig())
                .ApplyConfiguration(new MonthlySalaryComponentConfig())
                .ApplyConfiguration(new PayrollConfig())
                .ApplyConfiguration(new PayslipConfig())
                .ApplyConfiguration(new ReferenceTableConfig())
                .ApplyConfiguration(new ReferenceTableDetailConfig())
                .ApplyConfiguration(new ReferenceTableTypeConfig())
                .ApplyConfiguration(new RoleConfig())
                .ApplyConfiguration(new SalaryComponentConfig())
                .ApplyConfiguration(new AccountConfig())
                .ApplyConfiguration(new PositionConfig())
                .ApplyConfiguration(new PositionDetailConfig())
                .ApplyConfiguration(new DepartmentConfig())
                .ApplyConfiguration(new PayrollComponentConfig())
                .ApplyConfiguration(new PayslipTemplateConfig());
        }
    }
}
