using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Reflection.Metadata;

namespace API.Data {
    class DataContextDapper {
        private readonly IConfiguration _config;
        public DataContextDapper(IConfiguration config) {
            _config = config;
        }
        public IEnumerable<T> LoadData<T>(string sqlExp) {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Query<T>(sqlExp);
        }
        public T LoadDataSingle<T>(string sqlExp) {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.QuerySingle<T>(sqlExp);
        }
        public bool ExecuteBool(string sqlExp) {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Execute(sqlExp) > 0;
        }
        public int ExecuteWithRowCount(string sqlExp) {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Execute(sqlExp);
        }
        public bool ExecuteWithParams(string sql , DynamicParameters parameters) {
            SqlConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Execute(sql , parameters) > 0;
        }
        public IEnumerable<T> LoadDataWithParams<T>(string sqlExp , DynamicParameters parameters) {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Query<T>(sqlExp , parameters);
        }
        public T LoadDataSingleWithParams<T>(string sqlExp , DynamicParameters parameters) {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.QuerySingle<T>(sqlExp , parameters);
        }

    }
}