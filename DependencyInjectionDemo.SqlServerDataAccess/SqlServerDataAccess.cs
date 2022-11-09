using System.Linq;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using DependencyInjectionDemo.DataAccess;

namespace DependencyInjectionDemo.SqlServerDataAccess
{
    public class SqlServerDataAccess : IDataAccess
    {
        public List<T> LoadData<T>(string sql)
        {
            using (IDbConnection cnn = new SqlConnection(LoadConnectionString()))
            {
                var output = cnn.Query<T>(sql, new DynamicParameters());
                return output.ToList();
            }
        }

        public void SaveData<T>(T model, string sql)
        {
            ExecuteQuery(model, sql);
        }

        public void UpdateData<T>(T model, string sql)
        {
            ExecuteQuery(model, sql);
        }

        public void DeleteData<T>(T model, string sql)
        {
            ExecuteQuery(model, sql);
        }

        private void ExecuteQuery<T>(T model, string sql)
        {
            using (IDbConnection cnn = new SqlConnection(LoadConnectionString()))
            {
                cnn.Execute(sql, model);
            }
        }

        private string LoadConnectionString(string id = "SqlConnection")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

    }

}