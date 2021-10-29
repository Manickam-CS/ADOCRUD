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
    public class DepartmentsRepository : IDepartmentsRepository
    {     
        private readonly IOptions<EmployeeDBConfiguration> _configs;
        string _connectionString;   

        public DepartmentsRepository(IOptions<EmployeeDBConfiguration> Configs)
        {
            _configs = Configs;    
            _connectionString = _configs.Value.DbConnection;
        }

        public IList<Department> GetDepartmentsByQuery()
        {
            List<Department> departments = new List<Department>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            //using (IDbConnection con = _connectionFactory.GetConnection)
            {
                string sqlQuery = "SELECT * FROM Departments";
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();               
                while (rdr.Read())
                {
                    Department department = new Department();
                    department.DeptId = Convert.ToInt32(rdr["DeptId"]);
                    department.Name = rdr["Name"].ToString();
                    departments.Add(department);
                }
                con.Close();
            }
            return departments;
        }

        public Department GetDepartmentsById(int deptId)
        {
            Department department = new Department();
            using (SqlConnection con = new SqlConnection(_connectionString))         
            {
                string sqlQuery = @"Select * From Departments WHERE deptId = @deptId";
                SqlCommand cmd = new SqlCommand(sqlQuery, con);              
                cmd.Parameters.AddWithValue("@deptId", deptId);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    department.DeptId = Convert.ToInt32(rdr["DeptId"]);
                    department.Name = rdr["Name"].ToString();
                }
                con.Close();
            }
            return department;
        }

        public bool AddDepartment(Department department)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("usp_AddDepartment", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", department.Name);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool UpdateDepartment(Department department)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("usp_UpdateDepartment", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DeptId", department.DeptId);
                    cmd.Parameters.AddWithValue("@Name", department.Name);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteDepartment(int deptId)
        {
            try
            {
                int rowsAffected = 0;
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    string sqlQuery = "@Delete From Departments WHERE deptId = @deptId";
                    SqlCommand cmd = new SqlCommand(sqlQuery, con);
                    cmd.Parameters.AddWithValue("@deptId", deptId);
                    con.Open();
                    rowsAffected = cmd.ExecuteNonQuery();
                    con.Close();
                }

                if (rowsAffected > 0)
                    return true;
                else
                    return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
