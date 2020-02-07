using System;
using System.Data.SqlClient;

namespace AppNiZiAPI.Models.Repositories
{
    public abstract class Repository
    {
        public SqlConnection conn = new SqlConnection(Environment.GetEnvironmentVariable("sqldb_connection"));

    }
}
