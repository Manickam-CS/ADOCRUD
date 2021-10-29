using ADOLib.Interface;
using ADOLib.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ADOLib
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private readonly IOptions<EmployeeDBConfiguration> _configs;
        string _connectionString;

        public EmployeesRepository(IOptions<EmployeeDBConfiguration> Configs)
        {
            _configs = Configs;
            _connectionString = _configs.Value.DbConnection;
        }

        public IList<Employee> GetEmployeesByQuery()
        {
            var SqlQuery = "SELECT EMP.[EmpId], EMP.[Name], EMP.[Address], EMP.[ImagePath], EMP.[DeptId], DEP.[Name] AS DepartmentName FROM [dbo].[Employees] EMP INNER JOIN [dbo].[Departments] DEP ON EMP.DeptId = DEP.DeptId";
            List<Employee> employees = new List<Employee>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(SqlQuery, con);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Employee employee = new Employee();
                    employee.EmpId = Convert.ToInt32(rdr["EmpId"]);
                    employee.Name = rdr["Name"].ToString();
                    employee.Address = rdr["Address"].ToString();
                    employee.ImagePath = rdr["ImagePath"].ToString();
                    employee.DeptId = Convert.ToInt32(rdr["DeptId"]);
                    employee.DepartmentName = rdr["DepartmentName"].ToString();
                    employees.Add(employee);
                }
                con.Close();
            }
            return employees;
        }

        public Employee GetEmployeesById(int empId)
        {
            var SqlQuery = "SELECT EMP.[EmpId], EMP.[Name], EMP.[Address], EMP.[ImagePath], EMP.[DeptId], DEP.[Name] AS DepartmentName  FROM [dbo].[Employees] EMP INNER JOIN [dbo].[Departments] DEP ON EMP.DeptId = DEP.DeptId WHERE  EMP.[EmpId] =  @empId";
            Employee employee = null;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(SqlQuery, con);
                cmd.Parameters.AddWithValue("@empId", empId);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    employee = new Employee();
                    employee.EmpId = Convert.ToInt32(rdr["EmpId"]);
                    employee.Name = rdr["Name"].ToString();
                    employee.Address = rdr["Address"].ToString();
                    employee.ImagePath = rdr["ImagePath"].ToString();
                    employee.DeptId = Convert.ToInt32(rdr["DeptId"]);
                    employee.DepartmentName = rdr["DepartmentName"].ToString();
                }
                con.Close();
            }
            return employee;


        }

        public bool AddEmployee(Employee employee)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("usp_AddEmployee", con);
                    SqlParameter outputIdParam = new SqlParameter("@EmpId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(outputIdParam);
                    cmd.Parameters.AddWithValue("@Name", employee.Name);
                    cmd.Parameters.AddWithValue("@Address", employee.Address);
                    cmd.Parameters.AddWithValue("@ImagePath", employee.ImagePath);
                    cmd.Parameters.AddWithValue("@DeptId", employee.DeptId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    int id = Convert.ToInt32(outputIdParam.Value);
                    con.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateEmployee(Employee employee)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("usp_UpdateEmployee", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("EmpId", employee.EmpId);
                    cmd.Parameters.AddWithValue("@Name", employee.Name);
                    cmd.Parameters.AddWithValue("@Address", employee.Address);
                    cmd.Parameters.AddWithValue("@ImagePath", employee.ImagePath);
                    cmd.Parameters.AddWithValue("@DeptId", employee.DeptId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteEmployee(int empId)
        {
            try
            {
                int rowsAffected = 0;
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    string sqlQuery = @"Delete From [dbo].[Employees] WHERE EmpId = @empId";
                    SqlCommand cmd = new SqlCommand(sqlQuery, con);
                    cmd.Parameters.AddWithValue("@empId", empId);
                    con.Open();
                    rowsAffected = cmd.ExecuteNonQuery();
                    con.Close();
                }
                if (rowsAffected > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}