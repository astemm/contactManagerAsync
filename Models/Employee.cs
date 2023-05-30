using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactManager.Models
{
    public class Employee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }  
        public Boolean Married { get; set; }
        public string Phone { get; set; }
        public Decimal Salary { get; set; }

        public Employee(string Name, DateTime DateOfBirth, Boolean Married,
            string Phone, Decimal Salary)
        {
            this.Name = Name; 
            this.DateOfBirth = DateOfBirth; 
            this.Married = Married;
            this.Phone = Phone; 
            this.Salary = Salary;
        }

        public Employee(int EmployeeId, string Name, DateTime DateOfBirth, Boolean Married,
            string Phone, Decimal Salary)
        {
            this.EmployeeId = EmployeeId;
            this.Name = Name; 
            this.DateOfBirth = DateOfBirth; 
            this.Married = Married;
            this.Phone = Phone; 
            this.Salary = Salary;
        }

        public Employee() { }
    }
}