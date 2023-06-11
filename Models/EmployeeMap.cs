using CsvHelper.Configuration;
using System.Globalization;
using System.Diagnostics;
using CsvHelper.TypeConversion;
namespace ContactManager.Models
{
    public class EmployeeMap : ClassMap<Employee>
    {
         public EmployeeMap()
         {
         Map(e => e.Name).Index(0);
         Map(e => e.DateOfBirth).TypeConverterOption.Format("dd/MM/yyyy")
         .Index(1);
         Map(e => e.Married).Index(2);
         Map(e => e.Phone).Index(3);
         Map(e => e.Salary).Index(4);
         }
    }
}