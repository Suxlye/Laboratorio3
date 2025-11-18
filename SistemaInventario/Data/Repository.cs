using Microsoft.Data.SqlClient;
using RepoDb;
using System.Collections.Generic;
using System.Linq;

namespace SistemaInventario.Data
{
    public class Repository<T> where T : class
    {
        public IEnumerable<T> GetAll()
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                return connection.QueryAll<T>();
            }
        }

        public T GetById(object id)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                return connection.Query<T>(id).FirstOrDefault();
            }
        }

        public int Insert(T entity)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                return (int)connection.Insert(entity);
            }
        }

        public int Update(T entity)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                return connection.Update(entity);
            }
        }

        public int Delete(object id)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                return connection.Delete<T>(id);
            }
        }

        public IEnumerable<T> Query(string whereClause, object param = null)
        {
            using (var connection = DatabaseConnection.GetConnection())
            {
                return connection.ExecuteQuery<T>(
                    $"SELECT * FROM {GetTableName()} WHERE {whereClause}",
                    param);
            }
        }

        private string GetTableName()
        {
            var attribute = typeof(T).GetCustomAttributes(typeof(RepoDb.Attributes.MapAttribute), false)
                .FirstOrDefault() as RepoDb.Attributes.MapAttribute;
            return attribute?.Name ?? typeof(T).Name;
        }
    }
}