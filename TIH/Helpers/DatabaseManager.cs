using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System;

namespace TIH.Helpers
{
    public class DatabaseManager
    {
        private SqlDataAdapter _adapter;
        private SqlConnection _connection;
        private SqlCommand _command;

        public DatabaseManager()
        {
            _adapter = new SqlDataAdapter();
            _connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TIHAzureDbConnection"].ConnectionString);
        }

        private SqlConnection getOpenDbConnection()
        {
            try
            {
                if (_connection.State == ConnectionState.Closed || _connection.State == ConnectionState.Broken)
                {
                    _connection.Open();
                }
                return _connection;
            }
            catch
            {
                throw new Exception("Could not open a connection to the database.");
            }
        }

        public DataTable ExecuteSelectQueryByProcedureName(string procedureName, SqlParameter[] sqlParameters)
        {
            DataTable dtQueryResults;

            try
            {
                using (_command = new SqlCommand())
                {
                    
                    _command.CommandText = procedureName;
                    _command.CommandType = CommandType.StoredProcedure;
                    foreach (SqlParameter parameter in sqlParameters)
                    {
                        _command.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                    }
                    using (_command.Connection = getOpenDbConnection())
                    using (SqlDataReader reader = _command.ExecuteReader())
                    {
                        dtQueryResults = new DataTable();
                        dtQueryResults.Load(reader);
                    }
                }
            }

            catch
            {
                return null;
            }

            finally
            {
                if (_command.Connection != null && _command.Connection.State != ConnectionState.Closed)
                {
                    _command.Connection.Close();
                }
            }

            return dtQueryResults;
        }
    }
}