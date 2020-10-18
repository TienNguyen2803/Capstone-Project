using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DepName = table.Column<string>(maxLength: 255, nullable: true),
                    DepOffice = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fields",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    LongName = table.Column<string>(maxLength: 255, nullable: true),
                    DataType = table.Column<string>(maxLength: 20, nullable: true),
                    Status = table.Column<bool>(nullable: false, defaultValue: true),
                    CellMapping = table.Column<string>(maxLength: 10, nullable: true),
                    IsMonthlyComponent = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    SampleValue = table.Column<string>(nullable: true, defaultValue: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fields", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Formulas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    CreateDate = table.Column<DateTimeOffset>(nullable: true),
                    IsSalaryFormula = table.Column<bool>(nullable: false, defaultValue: false),
                    Status = table.Column<bool>(nullable: false, defaultValue: true),
                    Type = table.Column<int>(nullable: false, defaultValue: 1),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formulas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReferenceTables",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    SourceType = table.Column<int>(nullable: false),
                    SourceValue = table.Column<int>(nullable: false),
                    CompareType = table.Column<int>(nullable: false),
                    ReturnType = table.Column<string>(maxLength: 20, nullable: true),
                    Status = table.Column<bool>(nullable: false, defaultValue: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferenceTables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Code = table.Column<string>(maxLength: 20, nullable: false),
                    Fullname = table.Column<string>(maxLength: 255, nullable: true),
                    DateOfBirth = table.Column<DateTimeOffset>(nullable: true),
                    Gender = table.Column<string>(maxLength: 50, nullable: true),
                    Phone = table.Column<string>(maxLength: 15, nullable: true),
                    Email = table.Column<string>(maxLength: 320, nullable: true),
                    Address = table.Column<string>(nullable: true),
                    IsForeigner = table.Column<bool>(nullable: true, defaultValue: false),
                    DepartmentId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTimeOffset>(nullable: true),
                    IsWorking = table.Column<bool>(nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    SignDate = table.Column<DateTimeOffset>(nullable: true),
                    ApplyDate = table.Column<DateTimeOffset>(nullable: true),
                    EndDate = table.Column<DateTimeOffset>(nullable: true),
                    CloseDay = table.Column<int>(nullable: false),
                    Deadline = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    FormulaId = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false, defaultValue: 1),
                    DocumentUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Formulas_FormulaId",
                        column: x => x.FormulaId,
                        principalTable: "Formulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FormulaDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<int>(nullable: false),
                    FormulaId = table.Column<int>(nullable: false),
                    Ordinal = table.Column<int>(nullable: false),
                    Operator = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormulaDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormulaDetails_Formulas_FormulaId",
                        column: x => x.FormulaId,
                        principalTable: "Formulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReferenceTableDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ReferenceTableId = table.Column<int>(nullable: false),
                    Key = table.Column<string>(maxLength: 50, nullable: true),
                    Value = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferenceTableDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceTableDetails_ReferenceTables_ReferenceTableId",
                        column: x => x.ReferenceTableId,
                        principalTable: "ReferenceTables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Code = table.Column<string>(maxLength: 20, nullable: false),
                    Password = table.Column<string>(maxLength: 20, nullable: true),
                    RoleId = table.Column<int>(nullable: false, defaultValue: 3)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Accounts_Employees_Code",
                        column: x => x.Code,
                        principalTable: "Employees",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Accounts_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PositionDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmpCode = table.Column<string>(nullable: true),
                    PositionId = table.Column<int>(nullable: false),
                    ApplyDate = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PositionDetails_Employees_EmpCode",
                        column: x => x.EmpCode,
                        principalTable: "Employees",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PositionDetails_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalaryComponents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Value = table.Column<string>(maxLength: 255, nullable: true),
                    EmpId = table.Column<string>(nullable: false),
                    FieldId = table.Column<int>(nullable: false),
                    ApplyDate = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryComponents_Employees_EmpId",
                        column: x => x.EmpId,
                        principalTable: "Employees",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalaryComponents_Fields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Fields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payrolls",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DocId = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    StandardWorkDay = table.Column<int>(nullable: false),
                    Revenue = table.Column<decimal>(nullable: false),
                    FromDate = table.Column<DateTimeOffset>(nullable: true),
                    ToDate = table.Column<DateTimeOffset>(nullable: true),
                    PayDate = table.Column<DateTimeOffset>(nullable: true),
                    Status = table.Column<int>(nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payrolls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payrolls_Documents_DocId",
                        column: x => x.DocId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PayslipTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DocId = table.Column<int>(nullable: false),
                    TemplateUrl = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayslipTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayslipTemplates_Documents_DocId",
                        column: x => x.DocId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConstantTypes",
                columns: table => new
                {
                    FormulaDetailId = table.Column<int>(nullable: false),
                    Value = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConstantTypes", x => x.FormulaDetailId);
                    table.ForeignKey(
                        name: "FK_ConstantTypes_FormulaDetails_FormulaDetailId",
                        column: x => x.FormulaDetailId,
                        principalTable: "FormulaDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FieldTypes",
                columns: table => new
                {
                    FormulaDetailId = table.Column<int>(nullable: false),
                    FieldId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldTypes", x => x.FormulaDetailId);
                    table.ForeignKey(
                        name: "FK_FieldTypes_Fields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Fields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FieldTypes_FormulaDetails_FormulaDetailId",
                        column: x => x.FormulaDetailId,
                        principalTable: "FormulaDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormulaTypes",
                columns: table => new
                {
                    FormulaDetailId = table.Column<int>(nullable: false),
                    FormulaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormulaTypes", x => x.FormulaDetailId);
                    table.ForeignKey(
                        name: "FK_FormulaTypes_FormulaDetails_FormulaDetailId",
                        column: x => x.FormulaDetailId,
                        principalTable: "FormulaDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormulaTypes_Formulas_FormulaId",
                        column: x => x.FormulaId,
                        principalTable: "Formulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReferenceTableTypes",
                columns: table => new
                {
                    FormulaDetailId = table.Column<int>(nullable: false),
                    RefenceTableTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferenceTableTypes", x => x.FormulaDetailId);
                    table.ForeignKey(
                        name: "FK_ReferenceTableTypes_FormulaDetails_FormulaDetailId",
                        column: x => x.FormulaDetailId,
                        principalTable: "FormulaDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReferenceTableTypes_ReferenceTables_RefenceTableTypeId",
                        column: x => x.RefenceTableTypeId,
                        principalTable: "ReferenceTables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PayrollComponents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PayrollId = table.Column<int>(nullable: false),
                    Value = table.Column<string>(maxLength: 255, nullable: true),
                    FieldId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollComponents_Fields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Fields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayrollComponents_Payrolls_PayrollId",
                        column: x => x.PayrollId,
                        principalTable: "Payrolls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payslips",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmpId = table.Column<string>(nullable: true),
                    PayrollId = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Status = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payslips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payslips_Employees_EmpId",
                        column: x => x.EmpId,
                        principalTable: "Employees",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payslips_Payrolls_PayrollId",
                        column: x => x.PayrollId,
                        principalTable: "Payrolls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonthlySalaryComponents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PayslipId = table.Column<int>(nullable: false),
                    Value = table.Column<string>(maxLength: 255, nullable: true),
                    FieldId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlySalaryComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthlySalaryComponents_Fields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Fields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlySalaryComponents_Payslips_PayslipId",
                        column: x => x.PayslipId,
                        principalTable: "Payslips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_RoleId",
                table: "Accounts",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_FormulaId",
                table: "Documents",
                column: "FormulaId",
                unique: true,
                filter: "[FormulaId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Fields_Name",
                table: "Fields",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FieldTypes_FieldId",
                table: "FieldTypes",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_FormulaDetails_FormulaId",
                table: "FormulaDetails",
                column: "FormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_Formulas_Name",
                table: "Formulas",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FormulaTypes_FormulaId",
                table: "FormulaTypes",
                column: "FormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlySalaryComponents_FieldId",
                table: "MonthlySalaryComponents",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlySalaryComponents_PayslipId",
                table: "MonthlySalaryComponents",
                column: "PayslipId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollComponents_FieldId",
                table: "PayrollComponents",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollComponents_PayrollId",
                table: "PayrollComponents",
                column: "PayrollId");

            migrationBuilder.CreateIndex(
                name: "IX_Payrolls_DocId",
                table: "Payrolls",
                column: "DocId");

            migrationBuilder.CreateIndex(
                name: "IX_Payslips_EmpId",
                table: "Payslips",
                column: "EmpId");

            migrationBuilder.CreateIndex(
                name: "IX_Payslips_PayrollId",
                table: "Payslips",
                column: "PayrollId");

            migrationBuilder.CreateIndex(
                name: "IX_PayslipTemplates_DocId",
                table: "PayslipTemplates",
                column: "DocId");

            migrationBuilder.CreateIndex(
                name: "IX_PositionDetails_EmpCode",
                table: "PositionDetails",
                column: "EmpCode");

            migrationBuilder.CreateIndex(
                name: "IX_PositionDetails_PositionId",
                table: "PositionDetails",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferenceTableDetails_ReferenceTableId",
                table: "ReferenceTableDetails",
                column: "ReferenceTableId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferenceTables_Name",
                table: "ReferenceTables",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ReferenceTableTypes_RefenceTableTypeId",
                table: "ReferenceTableTypes",
                column: "RefenceTableTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryComponents_EmpId",
                table: "SalaryComponents",
                column: "EmpId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryComponents_FieldId",
                table: "SalaryComponents",
                column: "FieldId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "ConstantTypes");

            migrationBuilder.DropTable(
                name: "FieldTypes");

            migrationBuilder.DropTable(
                name: "FormulaTypes");

            migrationBuilder.DropTable(
                name: "MonthlySalaryComponents");

            migrationBuilder.DropTable(
                name: "PayrollComponents");

            migrationBuilder.DropTable(
                name: "PayslipTemplates");

            migrationBuilder.DropTable(
                name: "PositionDetails");

            migrationBuilder.DropTable(
                name: "ReferenceTableDetails");

            migrationBuilder.DropTable(
                name: "ReferenceTableTypes");

            migrationBuilder.DropTable(
                name: "SalaryComponents");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Payslips");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "FormulaDetails");

            migrationBuilder.DropTable(
                name: "ReferenceTables");

            migrationBuilder.DropTable(
                name: "Fields");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Payrolls");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Formulas");
        }
    }
}
