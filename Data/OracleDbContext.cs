using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Data;
using System.Xml.Linq;

public class OracleDbContext
{
    private readonly string _connectionString;

    public OracleDbContext(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public DataTable EjecutarConsulta(string consulta)
    {
        using (var connection = new OracleConnection(_connectionString))
        {
            var command = new OracleCommand(consulta, connection);
            var dataTable = new DataTable();

            try
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    dataTable.Load(reader);
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }

            return dataTable;
        }
    }

    public int ExecuteStoredProcedure(string pkgName, string spName,string name,string pass)
    {
        int exito = 0;
        using (var connection = new OracleConnection(_connectionString))
        {
            var command = new OracleCommand(pkgName+spName, connection);
            var dataTable = new DataTable();
            command.CommandType = CommandType.StoredProcedure;
            // Parámetros de entrada
            command.Parameters.Add(new OracleParameter("p_nombre", OracleDbType.Varchar2)).Value = name;
            command.Parameters.Add(new OracleParameter("p_pass", OracleDbType.Varchar2)).Value = pass;
            // Parámetro de salida
            OracleParameter outputNumber = new OracleParameter("p_exito", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(outputNumber);
            try
            {
                connection.Open();
                // Ejecuta el procedimiento almacenado
                command.ExecuteNonQuery();
                if (outputNumber.Value is OracleDecimal oracleDecimal)
                {
                    exito = oracleDecimal.ToInt32();
                }
                //exito = Convert.ToInt32(outputNumber.Value);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }

            return exito;
        }
    }

    public List<string> GetUserRoles(string name)
    {
        using (var connection = new OracleConnection(_connectionString))
        {
            var command = new OracleCommand("USUARIOS_PKG." + "SP_OBTENERROLES", connection);
            var dataTable = new DataTable();
            command.CommandType = CommandType.StoredProcedure;
            // Parámetros de entrada
            command.Parameters.Add(new OracleParameter("p_nombre", OracleDbType.Varchar2)).Value = name;

            List<string> roles = new List<string>();
            try
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        roles.Add(reader["rol"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }

            return roles;
        }
    }
    public List<string> UserRolesCheck(string pkgName, string spName, string name, string pass)
    {
        bool exito = false;
        using (var connection = new OracleConnection(_connectionString))
        {
            var command = new OracleCommand(pkgName + spName, connection);
            var dataTable = new DataTable();
            List<string> roles = new List<string>();
            command.CommandType = CommandType.StoredProcedure;
            // Parámetros de entrada
            command.Parameters.Add(new OracleParameter("p_nombre", OracleDbType.Varchar2)).Value = name;
            command.Parameters.Add(new OracleParameter("p_pass", OracleDbType.Varchar2)).Value = pass;
            OracleParameter outputCursor = new OracleParameter("p_roles", OracleDbType.RefCursor)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(outputCursor);
            OracleParameter outputNumber = new OracleParameter("p_exito", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(outputNumber);

            try
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    // Lee el cursor
                    while (reader.Read())
                    {
                        // Accede a los datos de cada columna por índice o por nombre
                        string roleName = reader.GetString(0); // Ejemplo: si la primera columna es un nombre de rol
                        //Console.WriteLine(roleName);
                        roles.Add(roleName);
                    }
                }
                // Lee el valor del parámetro de salida p_exito
                //exito = (Convert.ToInt32(outputNumber.Value)) == 1 ? true:false;             
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }

            return roles;
        }
    }
}
