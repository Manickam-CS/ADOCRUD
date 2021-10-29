using ADOLib.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ADOLib.Interface
{
    public interface IEmployeesRepository
    {
        IList<Employee> GetEmployeesByQuery();
        Employee GetEmployeesById(int empId);
        bool AddEmployee(Employee employee);
        bool UpdateEmployee(Employee employee);
        bool DeleteEmployee(int empId);
    }
}
