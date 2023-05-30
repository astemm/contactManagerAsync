using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Globalization;

namespace ContactManager.Models
{
    public class Converter
    {
         public IList<Employee> convertFromCSV(IFormFile file)
        {
            StringBuilder sb = new StringBuilder();
            IList<Employee> employees = new List<Employee>();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    sb.Append(line);
                    Employee emp=new Employee(values[0],Convert.ToDateTime(values[1]),
                    Convert.ToBoolean(values[2]),values[3],Convert.ToDecimal(values[4],new CultureInfo("en-US")));
                    employees.Add(emp);
                }
            }
            return employees;
        }
    }
}