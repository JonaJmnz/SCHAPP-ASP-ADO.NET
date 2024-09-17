using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using SCHAPP.Models;
using System;
using System.Data;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class OracleDbContext
{
    private readonly string _connectionString;

    public OracleDbContext(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }
    public bool ExistUserName(string pkgName, string spName, string UserName)
    {
        bool exist = true;
        using (var connection = new OracleConnection(_connectionString))
        {
            var command = new OracleCommand(pkgName + spName, connection);
            command.CommandType = CommandType.StoredProcedure;
            // Parámetros de entrada
            command.Parameters.Add(new OracleParameter("p_email", OracleDbType.Varchar2)).Value = UserName;
            OracleParameter outExiste = new OracleParameter("p_existe", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(outExiste);

            try
            {
                connection.Open();
                command.ExecuteReader();
                // Lee el valor del parámetro de salida p_exito
                OracleDecimal result = (OracleDecimal)outExiste.Value;
                exist = result == 1;
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
            return exist;
        }
    }
    public bool ExistEmail(string pkgName, string spName, string email)
    {
        bool exist = true;
        using (var connection = new OracleConnection(_connectionString))
        {
            var command = new OracleCommand(pkgName + spName, connection);
            command.CommandType = CommandType.StoredProcedure;
            // Parámetros de entrada
            command.Parameters.Add(new OracleParameter("p_email", OracleDbType.Varchar2)).Value = email;
            OracleParameter outExiste = new OracleParameter("p_existe", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(outExiste);

            try
            {
                connection.Open();
                command.ExecuteReader();
                // Lee el valor del parámetro de salida p_exito
                OracleDecimal result = (OracleDecimal)outExiste.Value;
                exist = result == 1;
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
            return exist;
        }
    }
    public List<string> GetRoles(string pkgName, string spName)
    {
        using (var connection = new OracleConnection(_connectionString))
        {
            var command = new OracleCommand(pkgName + spName, connection);
            command.CommandType = CommandType.StoredProcedure;
            OracleParameter outExiste = new OracleParameter("p_roles", OracleDbType.RefCursor)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(outExiste);

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
    public bool InsertUser(string pkgName, string spName, RegistrarseViewModel model)
    {
        bool exito = false;
        using (var connection = new OracleConnection(_connectionString))
        {
            var command = new OracleCommand(pkgName + spName, connection);
            var dataTable = new DataTable();
            List<string> roles = new List<string>();
            command.CommandType = CommandType.StoredProcedure; 
            // Parámetros de entrada
            command.Parameters.Add(new OracleParameter("P_NOMBRE", OracleDbType.Varchar2)).Value = model.inpNombre;
            command.Parameters.Add(new OracleParameter("P_APELLIDO_PATERNO", OracleDbType.Varchar2)).Value = model.inpApPaterno;
            command.Parameters.Add(new OracleParameter("P_APELLIDO_MATERNO", OracleDbType.Varchar2)).Value = model.inpApMaterno;
            command.Parameters.Add(new OracleParameter("P_NOMBRE_USUARIO", OracleDbType.Varchar2)).Value = model.inpNombreUsu;
            command.Parameters.Add(new OracleParameter("P_ID_SEXO", OracleDbType.Int32)).Value = model.Sexo;
            command.Parameters.Add(new OracleParameter("P_TELEFONO", OracleDbType.Varchar2)).Value = model.inpTelefono;
            command.Parameters.Add(new OracleParameter("P_EMAIL", OracleDbType.Varchar2)).Value = model.inpEmail;
            command.Parameters.Add(new OracleParameter("P_PASS", OracleDbType.Varchar2)).Value = model.inpContraseña;
            command.Parameters.Add(new OracleParameter("P_DOMICILIO", OracleDbType.Varchar2)).Value = model.inpDomicilio;
            command.Parameters.Add(new OracleParameter("P_ROL", OracleDbType.Int32)).Value = model.slctRol;
            command.Parameters.Add(new OracleParameter("P_FECHA_NACIMIENTO", OracleDbType.Date)).Value = model.inpFdeNacimiento;
            OracleParameter outputExito = new OracleParameter("p_exito", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(outputExito);

            try
            {
                connection.Open();
                command.ExecuteReader();
                // Lee el valor del parámetro de salida p_exito// Convertir OracleBoolean a bool
                OracleDecimal result = (OracleDecimal)outputExito.Value;
                exito = result == 1;             
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
        return exito;
    }
    public bool InsertRol(string pkgName, string spName, string rol)
    {
        bool exito = false;
        using (var connection = new OracleConnection(_connectionString))
        {
            var command = new OracleCommand(pkgName + spName, connection);
            var dataTable = new DataTable();
            List<string> roles = new List<string>();
            command.CommandType = CommandType.StoredProcedure;
            // Parámetros de entrada
            command.Parameters.Add(new OracleParameter("P_ROL", OracleDbType.Varchar2)).Value = rol;
            OracleParameter outputExito = new OracleParameter("p_exito", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(outputExito);

            try
            {
                connection.Open();
                command.ExecuteReader();
                // Lee el valor del parámetro de salida p_exito// Convertir OracleBoolean a bool
                OracleDecimal result = (OracleDecimal)outputExito.Value;
                exito = result == 1;
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
        return exito;
    }
    public List<string> UserRolesCheck(string pkgName, string spName, string name, string pass)
    {
        //bool exito = false;
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

    /*
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
    }*/
    /*
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
    }*/

    /*public List<string> GetUserRoles(string name)
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
    }*/
}
