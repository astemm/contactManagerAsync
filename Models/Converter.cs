using System.Diagnostics.Tracing;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Diagnostics;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using System.Threading.Tasks;
using System.Globalization;
using CsvHelper.TypeConversion;

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

        public async Task<IList<Employee>> convertFromCSVAsync(IFormFile file)
        {
            StringBuilder sb = new StringBuilder();
            IList<Employee> employees = new List<Employee>();
            using (var memoryStream = new MemoryStream())
            {
            await file.CopyToAsync(memoryStream);
            memoryStream.Position=0;
            using (var reader = new StreamReader(memoryStream))
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
            }
            return employees;
        }

        public async Task<IList<Employee>> convertWithLibAsync(IFormFile file)
        {
            IList<Employee> employees = new List<Employee>();
            using (var memoryStream = new MemoryStream())
            {
            await file.CopyToAsync(memoryStream);
            memoryStream.Position=0;
            using (var reader = new StreamReader(memoryStream))
            {
                /*
                var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                    HasHeaderRecord = false,
                    TrimOptions = TrimOptions.Trim,
                    }; */

                var csv = new CsvReader(reader,CultureInfo.InvariantCulture);
                csv.Configuration.RegisterClassMap<EmployeeMap>();
                csv.Configuration.HasHeaderRecord = false;
                csv.Configuration.TrimOptions = TrimOptions.Trim;  
                employees = csv.GetRecords<Employee>().ToList();
            }
            }
            return employees;
        }
    }
}