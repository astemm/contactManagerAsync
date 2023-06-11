using System;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ContactManager.Models;

namespace ContactManager.Services
{
    public class EmployeeService: IDisposable, IEmployeeService
    {
        private EmployeeContext context;
        private Converter converter = new Converter();
        
        private string fileName;

        public  EmployeeService(EmployeeContext context) {
           this.context=context;
        }

        public async Task<List<Employee>> GetEmployees()
        {
            var list = await context.Employees.ToListAsync();
            return list;
        }

        public async Task<List<Employee>> GetSortedEmployees(string column)
        {
            List<Employee> list = null;
            if (column.Equals("Name")) {list = await context.Employees.OrderBy(e => e.Name).ToListAsync<Employee>(); }
            else if (column.Equals("DateOfBirth")) {list = await context.Employees.OrderBy(e => e.DateOfBirth).ToListAsync<Employee>(); }
            else if (column.Equals("Married")) {list = await context.Employees.OrderBy(e => e.Married).ToListAsync<Employee>(); }
            else if (column.Equals("Phone")) {list = await context.Employees.OrderBy(e => e.Phone).ToListAsync<Employee>(); }
            else if (column.Equals("Salary")) {list = await context.Employees.OrderBy(e => e.Salary).ToListAsync<Employee>(); }
            else if (column.Equals("NoSorting")) {list = await context.Employees.ToListAsync<Employee>(); }
            return list;
        }

        public async Task Upload(IFormFile file)
        {
            /* 
            IFormFile file=null;
            try
             {   
                file = Request.Form.Files.FirstOrDefault();
                if (file==null) throw new NullReferenceException();
       
            }
            catch (Exception ex) { throw new Exception("No file chosen"); } */

            if (file != null && file.Length > 0)
            {
                fileName = file.FileName;
            }

            try
            {
                //Task<IList<Employee>> task =Task.FromResult<IList<Employee>>(await converter.convertFromCSVAsync(file));
                //task.Wait();
                //IList<Employee> emps = task.Result;
                IList<Employee> emps = await converter.convertWithLibAsync(file);
                Console.Write("l05 "+emps.Count);
                foreach (Employee e in emps)
                {
                    await AddEmployee(e);
                }
            }
            catch (Exception ex)  { throw new Exception (ex.InnerException.Message+ "Conversion error"); }
        }

        public async Task AddEmployee(Employee emp)
        {
            await context.Employees.AddAsync(emp);
            await context.SaveChangesAsync();
        }


        public async Task EditEmployee(Employee newEmployee)
        {
            Employee e = await context.Employees.FindAsync(newEmployee.EmployeeId);
            e.Name = newEmployee.Name;
            e.DateOfBirth = Convert.ToDateTime(newEmployee.DateOfBirth).Date;
            e.Married = newEmployee.Married;
            e.Phone=newEmployee.Phone;
            e.Salary=Convert.ToDecimal(newEmployee.Salary);
            try
            {
               await context.SaveChangesAsync();
            }
            catch (Exception ex) { 
                throw new Exception("There is no such employee");
            }
        }

        public async Task DeleteEmployee(int id)
        {
            Console.WriteLine("iddd: "+id);
            Employee e = await context.Employees.FindAsync(id);
            try
            {
                context.Employees.Remove(e);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("There is no such employee");
            }
        }

        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {   
                    context.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}