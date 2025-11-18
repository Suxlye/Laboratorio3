using Microsoft.Data.SqlClient;
using RepoDb;
using System;
using System.Configuration;

namespace SistemaInventario.Data
{
    public static class DatabaseConnection
    {
        private static string _connectionString;
        private static bool _initialized = false;

        public static void Initialize()
        {
            if (_initialized)
                return;

            try
            {
                SqlServerBootstrap.Initialize();

                var connString = ConfigurationManager.ConnectionStrings["conexionBD"];

                if (connString == null)
                {
                    throw new InvalidOperationException(
                        "No se encontró la cadena de conexión 'conexionBD' en App.config");
                }

                _connectionString = connString.ConnectionString;
                _initialized = true;

                // Probar la conexión
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Error al inicializar la conexión a la base de datos: {ex.Message}", ex);
            }
        }

        public static SqlConnection GetConnection()
        {
            if (!_initialized)
            {
                throw new InvalidOperationException(
                    "DatabaseConnection no inicializada. Llama a DatabaseConnection.Initialize() primero.");
            }

            return new SqlConnection(_connectionString);
        }

        public static string ConnectionString
        {
            get
            {
                if (!_initialized)
                {
                    throw new InvalidOperationException(
                        "DatabaseConnection no inicializada.");
                }
                return _connectionString;
            }
        }
    }
}
