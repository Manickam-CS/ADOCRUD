using ADOLib.Interface;
using ADOLib.Models;
using System;
using System.Data;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace ADOLib
{
    public class ConnectionFactory : IConnectionFactory
    {
        private IDbConnection _connection;
        private readonly IOptions<EmployeeDBConfiguration> _configs;

        public ConnectionFactory(IOptions<EmployeeDBConfiguration> Configs)
        {
            _configs = Configs;
        }
        public IDbConnection GetConnection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new SqlConnection(_configs.Value.DbConnection);
                }
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                return _connection;
            }
        }

        public void CloseConnection()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
}
