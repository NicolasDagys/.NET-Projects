using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;
using Entidades_Compartidas;

namespace Persistencia
{
    public class PersistenciaEmpleado
    {
        public static Empleado Logueo(string pUsuario, string pContrasenia)
        {
            SqlConnection oConexion = new SqlConnection(CONEXION.STR);
            SqlCommand oComando = new SqlCommand("LogeoEmpleado", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            Empleado E = null;

            //parametros
            oComando.Parameters.AddWithValue("@usuario", pUsuario);
            oComando.Parameters.AddWithValue("@pass", pContrasenia);

            try
            {
                oConexion.Open();

                SqlDataReader _lector = oComando.ExecuteReader();

                if (_lector.HasRows)
                {
                    _lector.Read();
                    string _Usuario = (string)_lector["Usuario"];
                    string _NombreComp = (string)_lector["NombreCompE"];
                    string _Contrasenia = (string)_lector["ContraE"];
                    string _Cargo = (string)_lector["Cargo"];

                    E = new Empleado(_Usuario, _NombreComp, _Contrasenia, _Cargo);
                }

                _lector.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oConexion.Close();
            }

            return E;
        }
    }
}
