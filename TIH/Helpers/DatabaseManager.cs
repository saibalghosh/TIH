using System.Data;
using System.Data.SqlClient;
using System.Configuration;

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
            if(_connection.State == ConnectionState.Closed || _connection.State == ConnectionState.Broken)
            {
                _connection.Open();
            }
            return _connection;
        }

        public DataTable ExecuteSelectQuery(string sqlQuery, SqlParameter[] sqlParameters)
        {
            DataTable dtQueryResults = new DataTable();
            dtQueryResults = null;
            DataSet dsQueryResults = new DataSet();

            try
            {
                using (_command = new SqlCommand())
                {
                    _command.Connection = getOpenDbConnection();
                    _command.CommandText = sqlQuery;
                    _command.Parameters.AddRange(sqlParameters);
                    _command.ExecuteNonQuery();
                    _adapter.SelectCommand = _command;
                    _adapter.Fill(dsQueryResults);
                    dtQueryResults = dsQueryResults.Tables[0];
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (_command.Connection != null)
                {
                    _command.Connection.Close();
                }
            }

            return dtQueryResults;
        }
    }
}