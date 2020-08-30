using Bfar.Extensions.Core;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Bfar.Repository.Core
{
    /// <summary>
    /// Obsolete
    /// </summary>
    public class SimpleMSSQL : ISimpleRepository
    {
        private const string providerString = "System.Data.SqlClient";
        private string _connectionString;
        public static SimpleMSSQL Factory(string server, string databaseName, string userName, string password) { return new SimpleMSSQL(server,databaseName,userName,password);  }
        public static SimpleMSSQL Connect(string connectionString)
        {
            return new SimpleMSSQL(connectionString);
        }
        public SimpleMSSQL BuildRepository(string server, string databaseName, string userName, string password) 
        {
            MinTimeOut = 120;
            _connectionString = $"Server = {server}; Database = {databaseName}; User Id = {userName}; Password = {password};";
            return this;
        }
        public string AdHocCommand { get; set; }
        public int MinTimeOut { get; set; }

        public SqlConnection Connection
        {
            get
            {
                var connection = new SqlConnection();
                connection.ConnectionString = _connectionString;
                return connection;
            }
        }

        public SimpleMSSQL(string server,string databaseName,string userName,string password)
        {
            MinTimeOut = 120;
            _connectionString = $"Server = {server}; Database = {databaseName}; User Id = {userName}; Password = {password};";
        }
        public SimpleMSSQL(string ConnectionString)
        {
            MinTimeOut = 120;
            _connectionString = ConnectionString;
        }
        public long Add<K>(K model) where K : class
        {
            long result = 0;
            using (SqlConnection Con = Connection)
            {
                Con.Open();
                result = Con.Insert(model);
            }
            return result;
        }

        public ISimpleRepository Build(string Command)
        {
            AdHocCommand = Command;
            return this;
        }

        public void Execute(object Parameters)
        {
            using (SqlConnection Con = Connection)
            {
                Con.Open();
                var t = Con.Query(AdHocCommand, Parameters, null, false, MinTimeOut, CommandType.StoredProcedure).SingleOrDefault();
                Con.Close();
            }
        }

        public List<K> Execute<K>(object Parameters) where K : class
        {
            using (SqlConnection Con = Connection)
            {
                Con.Open();
                var t = Con.Query<K>(AdHocCommand, Parameters, null, false, MinTimeOut, CommandType.StoredProcedure);
                Con.Close();
                return t?.ToList();
            }
        }

        public List<K> ExecuteAdHoc<K>(object Parameters = null) where K : class
        {
            using (SqlConnection Con = Connection)
            {
                Con.Open();
                var t = Con.Query<K>(AdHocCommand, Parameters, null, false, MinTimeOut, CommandType.Text);
                return t?.ToList();
            }
        }

        public IEnumerable<K> GetAll<K>() where K : class
        {
            using (SqlConnection Con = Connection)
            {
                return Con.GetAll<K>();
            }
        }

        public List<K> ExecuteJson<T, K>(T obj)
        {
            throw new NotImplementedException();
        }

        public ISimpleRepository SetTimeOut(int seconds)
        {
            MinTimeOut = seconds * 1000;
            return this;
        }
    }
}
