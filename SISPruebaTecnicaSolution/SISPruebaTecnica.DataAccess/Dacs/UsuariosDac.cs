using SISPruebaTecnica.Entities.Models;
using System.Data.SqlClient;
using System.Data;
using SISPruebaTecnica.DataAccess.Interfaces;

namespace SISPruebaTecnica.DataAccess.Dacs
{
    public class UsuariosDac : IUsuarioDac
    {
        #region CONSTRUCTOR AND DEPENDENCY INJECTION
        private readonly IConnectionDac Connection;
        public UsuariosDac(IConnectionDac Connection)
        {
            this.Error_message = string.Empty;

            this.Connection = Connection;
        }
        #endregion

        #region SQL ERROR MESSAGE
        private void SqlCon_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            string mensaje_error = e.Message;
            if (e.Errors != null)
            {
                if (e.Errors.Count > 0)
                {
                    mensaje_error += string.Join("|", e.Errors);
                }
            }
            this.Error_message = mensaje_error;
        }
        #endregion

        #region PROPERTIES
        public string Error_message { get; set; }
        #endregion

        #region METHOD INSERT USUARIO
        public Task<string> InsertarUsuario(Usuarios usuario)
        {
            string rpta = string.Empty;

            SqlConnection SqlCon = new();
            SqlCon.InfoMessage += new SqlInfoMessageEventHandler(SqlCon_InfoMessage);
            SqlCon.FireInfoMessageEventOnUserErrors = true;

            try
            {
                SqlCon.ConnectionString = this.Connection.Cn();
                SqlCon.Open();

                SqlCommand SqlCmd = new()
                {
                    Connection = SqlCon,
                    CommandText = "sp_Usuarios_i",
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter Id_usuario = new()
                {
                    ParameterName = "@Id_usuario",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };
                SqlCmd.Parameters.Add(Id_usuario);

                SqlParameter Nombres = new()
                {
                    ParameterName = "@Nombres",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = usuario.Nombres.Trim(),
                };
                SqlCmd.Parameters.Add(Nombres);

                SqlParameter Apellidos = new()
                {
                    ParameterName = "@Apellidos",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = usuario.Apellidos.Trim(),
                };
                SqlCmd.Parameters.Add(Apellidos);

                SqlParameter Fecha_nacimiento = new()
                {
                    ParameterName = "@Fecha_nacimiento",
                    SqlDbType = SqlDbType.Date,
                    Value = usuario.Fecha_nacimiento,
                };
                SqlCmd.Parameters.Add(Fecha_nacimiento);

                SqlParameter Foto_usuario = new()
                {
                    ParameterName = "@Foto_usuario",
                    SqlDbType = SqlDbType.VarChar,
                    Value = usuario.Foto_usuario?.Trim(),
                };
                SqlCmd.Parameters.Add(Foto_usuario);

                SqlParameter Estado_civil = new()
                {
                    ParameterName = "@Estado_civil",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = usuario.Estado_civil?.Trim(),
                };
                SqlCmd.Parameters.Add(Estado_civil);

                SqlParameter Hermanos = new()
                {
                    ParameterName = "@Hermanos",
                    SqlDbType = SqlDbType.Bit,
                    Value = usuario.Hermanos,
                };
                SqlCmd.Parameters.Add(Hermanos);

                rpta = SqlCmd.ExecuteNonQuery() >= 1 ? "OK" : "NO";

                if (rpta != "OK")
                {
                    if (this.Error_message != null)
                    {
                        rpta = this.Error_message;
                    }
                }
                else
                {
                    usuario.Id_usuario = Convert.ToInt32(SqlCmd.Parameters["@Id_usuario"].Value);
                }
            }
            catch (SqlException ex)
            {
                rpta = ex.Message;
            }
            catch (Exception ex)
            {
                rpta = ex.Message;
            }
            finally
            {
                if (SqlCon.State == ConnectionState.Open)
                    SqlCon.Close();
            }
            return Task.FromResult(rpta);
        }
        #endregion

        #region SEARCH USUARIOS
        public string SearchUsuarios(string type_search, string value_search,
            out DataTable dt)
        {
            //Inicializamos la respuesta que vamos a devolver
            dt = new();
            string rpta = "OK";
            SqlConnection SqlCon = new();
            try
            {
                //Asignamos un evento SqlInfoMessage para obtener errores con severidad < 10 desde SQL
                SqlCon.InfoMessage += new SqlInfoMessageEventHandler(SqlCon_InfoMessage);
                SqlCon.FireInfoMessageEventOnUserErrors = true;
                //Asignamos la cadena de conexión desde un método estático que lee el archivo de configuracion
                SqlCon.ConnectionString = Connection.Cn();
                //Abrimos la conexión.
                SqlCon.Open();
                //Creamos un comando para ejecutar un procedimiento almacenado
                SqlCommand SqlCmd = new()
                {
                    Connection = SqlCon,
                    CommandText = "sp_Usuarios_g",
                    CommandType = CommandType.StoredProcedure
                };
                //Creamos cada parámetro y lo agregamos a la lista de parámetros del comando
                //El primer comando es el id del usuario que es parámetro de salida
                SqlParameter Type_search = new()
                {
                    ParameterName = "@Tipo_busqueda",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = type_search
                };
                SqlCmd.Parameters.Add(Type_search);
                //Los parámetros varchar se les asigna una propiedad extra y es el Size
                SqlParameter Value_search = new()
                {
                    ParameterName = "@Texto_busqueda",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = value_search,
                };
                SqlCmd.Parameters.Add(Value_search);

                //Ejecutamos nuestro comando cuando agreguemos todos los parámetros requeridos
                SqlDataAdapter SqlData = new(SqlCmd);
                SqlData.Fill(dt);

                //Comprobamos la variable de respuesta Mensaje_error que guarda el mensaje específico
                //De cualquier error generado en SQL procedimiento almacenado
                if (dt == null)
                {
                    if (!string.IsNullOrEmpty(this.Error_message))
                        rpta = this.Error_message;
                }
                else
                {
                    if (dt.Rows.Count < 1)
                        dt = null;
                }
            }
            catch (Exception ex)
            {
                rpta = ex.Message;
                dt = null;
            }
            finally
            {
                if (SqlCon.State == ConnectionState.Open)
                    SqlCon.Close();
            }
            return rpta;
        }
        #endregion
    }
}
