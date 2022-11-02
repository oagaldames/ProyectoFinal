using ProyectoFinal.Models;
using System.Data.SqlClient;

namespace ProyectoFinal.Repository
{
    public class DataSql
    {
        public SqlConnection Connection { get; set; }

        public DataSql()
        {

        }

        public DataSql(SqlConnection connection)
        {
            Connection = connection;
        }

        public bool ConectarSQL()
        {
            try
            {
                SqlConnectionStringBuilder connectionBuilder = new();
                connectionBuilder.DataSource = Parametros.Servidor;
                connectionBuilder.InitialCatalog = Parametros.BaseDeDatos;
                connectionBuilder.IntegratedSecurity = true;
                var cs = connectionBuilder.ConnectionString;
                Connection = new SqlConnection(cs);
                Connection.Open();

                if (Connection.State == System.Data.ConnectionState.Open)
                {
                    return true;
                }
                else
                {
                    return false;
                }              
            }
            catch (Exception err)
            {
                string error = err.Message;
                Console.WriteLine("\nERROR ConectarSQL  " + error);
                return false;
            }

        }

        public void DesconectarSQL()
        {

            if (Connection != null)
            {
                if (Connection.State == System.Data.ConnectionState.Open)
                {
                    Connection.Close();
                }
            }
        }

    }
}
