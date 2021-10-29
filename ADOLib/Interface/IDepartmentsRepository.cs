using ADOLib.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ADOLib.Interface
{
    public interface IDepartmentsRepository
    {
        IList<Department> GetDepartmentsByQuery();
        Department GetDepartmentsById(int deptId);
        bool AddDepartment(Department department);
        bool UpdateDepartment(Department department);
        bool DeleteDepartment(int deptId);
    }
}
