using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ADOLib.Interface
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection { get; }
        void CloseConnection();
    }
}
