using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Entities
{
    public class Employee : IEquatable<Employee>
    {
        public string Code { get; set; }
        public string Fullname { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public bool? IsForeigner { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public bool? IsWorking { get; set; }
        public Account Account { get; set; }
        public ICollection<Payslip> Payslips { get; set; }
        public ICollection<SalaryComponent> SalaryComponents { get; set; }
        public ICollection<PositionDetail> PositionDetails { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Employee);
        }

        public bool Equals(Employee other)
        {
            return other != null &&
                   Code == other.Code &&
                   Fullname == other.Fullname &&
                   EqualityComparer<DateTimeOffset?>.Default.Equals(DateOfBirth, other.DateOfBirth) &&
                   Gender == other.Gender &&
                   Phone == other.Phone &&
                   Email == other.Email &&
                   Address == other.Address &&
                   EqualityComparer<bool?>.Default.Equals(IsForeigner, other.IsForeigner) &&
                   DepartmentId == other.DepartmentId &&
                   EqualityComparer<Department>.Default.Equals(Department, other.Department) &&
                   EqualityComparer<DateTimeOffset?>.Default.Equals(StartDate, other.StartDate) &&
                   EqualityComparer<bool?>.Default.Equals(IsWorking, other.IsWorking) &&
                   EqualityComparer<Account>.Default.Equals(Account, other.Account) &&
                   EqualityComparer<ICollection<Payslip>>.Default.Equals(Payslips, other.Payslips) &&
                   EqualityComparer<ICollection<SalaryComponent>>.Default.Equals(SalaryComponents, other.SalaryComponents) &&
                   EqualityComparer<ICollection<PositionDetail>>.Default.Equals(PositionDetails, other.PositionDetails);
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Code);
            hash.Add(Fullname);
            hash.Add(DateOfBirth);
            hash.Add(Gender);
            hash.Add(Phone);
            hash.Add(Email);
            hash.Add(Address);
            hash.Add(IsForeigner);
            hash.Add(DepartmentId);
            hash.Add(Department);
            hash.Add(StartDate);
            hash.Add(IsWorking);
            hash.Add(Account);
            hash.Add(Payslips);
            hash.Add(SalaryComponents);
            hash.Add(PositionDetails);
            return hash.ToHashCode();
        }
    }
}
