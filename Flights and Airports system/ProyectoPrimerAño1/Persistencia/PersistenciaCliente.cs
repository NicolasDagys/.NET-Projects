using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;
using Entidades_Compartidas;

namespace Persistencia
{
    public class PersistenciaCliente
    {
        public static Cliente Buscar(Int64 pPasaporte)
        {
           
            //inicio la coneccion
            SqlConnection oConexion = new SqlConnection(CONEXION.STR);
            SqlCommand oComando = new SqlCommand("BuscarCliente", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            Cliente C = null;
            SqlDataReader oReader;

            //Parametro para buscar
            oComando.Parameters.AddWithValue("@pasaporte", pPasaporte);

            try
            {
                oConexion.Open();
                oReader = oComando.ExecuteReader();

                if (oReader.HasRows)
                {
                    oReader.Read();

                    C = new Cliente(Convert.ToInt64(oReader["Pasaporte"]),
                        oReader["NombreCompC"].ToString(),
                        oReader["ContraC"].ToString(),
                        Convert.ToInt64(oReader["NumeroTarj"]));

                }
                oReader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oConexion.Close();
            }
            return C;
        }
        public static void Agregar(Cliente pCliente)
        {
            //inicio la coneccion
            SqlConnection oConexion = new SqlConnection(CONEXION.STR);
            SqlCommand oComando = new SqlCommand("AgregarAeropuerto", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            //agrego imput parametros del SP
            oComando.Parameters.AddWithValue("@pasaporte", pCliente.Pasaporte);
            oComando.Parameters.AddWithValue("@nombreCompC", pCliente.Nombre);
            oComando.Parameters.AddWithValue("@contraC", pCliente.Contrasenia);
            oComando.Parameters.AddWithValue("@numeroTarj", pCliente.NumeroTarj);

            //parametro de retorno para errores
            SqlParameter oRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
            oRetorno.Direction = ParameterDirection.ReturnValue;
            oComando.Parameters.Add(oRetorno);

            try
            {
                oConexion.Open();
                oComando.ExecuteNonQuery();

                int oAfectados = (int)oComando.Parameters["@Retorno"].Value;
                if (oAfectados == 0)
                    throw new Exception("No existe usuario Cliente cn ese Pasaporte");
                else if (oAfectados == -1)
                    throw new Exception("Error");

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oConexion.Close();
            }
        }
        public static void Modificar(Cliente pCliente)
        {
            //inicio la coneccion
            SqlConnection oConexion = new SqlConnection(CONEXION.STR);
            SqlCommand oComando = new SqlCommand("ModificarCliente", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            //agrego imput parametros del SP
            oComando.Parameters.AddWithValue("@pasaporte", pCliente.Pasaporte);
            oComando.Parameters.AddWithValue("@nombreCompC", pCliente.Nombre);
            oComando.Parameters.AddWithValue("@contraC", pCliente.Contrasenia);
            oComando.Parameters.AddWithValue("@numeroTarj", pCliente.NumeroTarj);

            //parametro de retorno para errores
            SqlParameter oRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
            oRetorno.Direction = ParameterDirection.ReturnValue;
            oComando.Parameters.Add(oRetorno);

            try
            {
                oConexion.Open();
                oComando.ExecuteNonQuery();

                int oAfectados = (int)oComando.Parameters["@Retorno"].Value;

                if (oAfectados == 0)
                    throw new Exception("No existe usuario Cliente cn ese Pasaporte");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oConexion.Close();
            }
        }
        public static void Eliminar(Cliente pCliente)
        {
            //inicio la coneccion
            SqlConnection oConexion = new SqlConnection(CONEXION.STR);
            SqlCommand oComando = new SqlCommand("EliminarCliente", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            //parametro de retorno para errores
            SqlParameter oRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
            SqlParameter oPasaporte = new SqlParameter("@pasaporte", pCliente.Pasaporte);

            oRetorno.Direction = ParameterDirection.ReturnValue;
            oComando.Parameters.Add(oRetorno);
            oComando.Parameters.Add(oPasaporte);

            try
            {
                oConexion.Open();
                oComando.ExecuteNonQuery();

                int oAfectados = (int)oComando.Parameters["@Retorno"].Value;
                if (oAfectados == 0)
                    throw new Exception("No existe usuario Cliente cn ese Pasaporte");
                else if (oAfectados == -1)
                    throw new Exception("El usuario Cliente tiene Pasajes asociados");

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                oConexion.Close();
            }
        }
        public static Cliente Logueo(Int64 pPasaporte, string pContraseña)
        {
            SqlConnection oConexion = new SqlConnection(CONEXION.STR);
            SqlCommand oComando = new SqlCommand("LogeoCliente", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            Cliente C = null;

            //parametros
            oComando.Parameters.AddWithValue("@pasaporte", pPasaporte);
            oComando.Parameters.AddWithValue("@pass", pContraseña);

            try
            {
                oConexion.Open();

                SqlDataReader _lector = oComando.ExecuteReader();

                if (_lector.HasRows)
                {
                    _lector.Read();
                    Int64 _Pasaporte = (Int64)_lector["Pasaporte"];
                    string _NombreComp=(string)_lector["NombreCompC"];
                    string _Contrasenia = (string)_lector["ContraC"];
                    Int64 _NumeroTarj = (Int64)_lector["NumeroTarj"];

                    C = new Cliente(_Pasaporte, _NombreComp, _Contrasenia, _NumeroTarj);
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

            return C;
        }
    }
}
