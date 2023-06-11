using System;
using ContactManager.Models;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ContactManager.Services
{
    public interface IEmployeeService
    {
         Task<List<Employee>> GetEmployees();
         Task<List<Employee>> GetSortedEmployees(string column);
         Task Upload(IFormFile uploadedFile);
         Task AddEmployee(Employee emp);
         Task EditEmployee(Employee newEmployee);
         Task DeleteEmployee(int id);
    }
}