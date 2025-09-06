using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;
using Entidades_Compartidas;

namespace Persistencia
{
    public class PersistenciaAeropuerto
    {
        public static Aeropuerto Buscar(string pCodigoA)
        {
            //string oNombreComp, oDireccion;
            //int oTelefono;

            //inicio la coneccion
            SqlConnection oConexion = new SqlConnection(CONEXION.STR);
            SqlCommand oComando = new SqlCommand("BuscarAeropuerto", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            Aeropuerto A = null;
            SqlDataReader oReader;

            //Parametro para buscar
            oComando.Parameters.AddWithValue("@codigoA", pCodigoA);

            try
            {
                oConexion.Open();
                oReader = oComando.ExecuteReader();

                if (oReader.HasRows)
                {
                    oReader.Read();

                    A = new Aeropuerto(oReader["CodigoA"].ToString(),
                        oReader["NombreA"].ToString(),
                        oReader["Direccion"].ToString(),
                        Convert.ToInt64(oReader["ImpuestoOri"]), 
                    Convert.ToInt64(oReader["ImpuestoDes"]),   
                    PercistenciaCiudad.Buscar(oReader["CodigoC"].ToString()));

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
            return A;
        }
        public static void Agregar(Aeropuerto pAeropuerto)
        { 
         //inicio la coneccion
            SqlConnection oConexion = new SqlConnection(CONEXION.STR);
            SqlCommand oComando = new SqlCommand("AgregarAeropuerto", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            //agrego imput parametros del SP
            oComando.Parameters.AddWithValue("@codigoA", pAeropuerto.CodigoA);
            oComando.Parameters.AddWithValue("@codigoC", pAeropuerto.unaC.CodigoC);
            oComando.Parameters.AddWithValue("@nombreA", pAeropuerto.Nombre);
            oComando.Parameters.AddWithValue("@direccion", pAeropuerto.Direccion);
            oComando.Parameters.AddWithValue("@impuestoOri", pAeropuerto.ImpuestoOri);
            oComando.Parameters.AddWithValue("@impuestoDes", pAeropuerto.ImpuestoDes);

            //parametro de retorno para errores
            SqlParameter oRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
            oRetorno.Direction = ParameterDirection.ReturnValue;
            oComando.Parameters.Add(oRetorno);

            try
            {
                oConexion.Open();
                oComando.ExecuteNonQuery();

                int oAfectados = (int)oComando.Parameters["@Retorno"].Value;//casteo tipo de dato (SQL a C#)
                if (oAfectados == 0)
                    throw new Exception("El Codigo del Aeropuerto ya existe");
                else if (oAfectados == -1)
                    throw new Exception("El Codigo de la Ciudad no existe");
                else if (oAfectados == -2)
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
        public static void Modificar(Aeropuerto pAeropuerto)
        {
            //inicio la coneccion
            SqlConnection oConexion = new SqlConnection(CONEXION.STR);
            SqlCommand oComando = new SqlCommand("MofificarAeropuerto", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            //agrego imput parametros del SP
            oComando.Parameters.AddWithValue("@codigoA", pAeropuerto.CodigoA);
            oComando.Parameters.AddWithValue("@codigoC", pAeropuerto.unaC.CodigoC);
            oComando.Parameters.AddWithValue("@nombreA", pAeropuerto.Nombre);
            oComando.Parameters.AddWithValue("@direccion", pAeropuerto.Direccion);
            oComando.Parameters.AddWithValue("@impuestoOri", pAeropuerto.ImpuestoOri);
            oComando.Parameters.AddWithValue("@impuestoDes", pAeropuerto.ImpuestoDes);

            //parametro de retorno para errores
            SqlParameter oRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
            oRetorno.Direction = ParameterDirection.ReturnValue;
            oComando.Parameters.Add(oRetorno);

            try
            {
                oConexion.Open();
                oComando.ExecuteNonQuery();

                int oAfectados = (int)oComando.Parameters["@Retorno"].Value;

                if (oAfectados == -1)
                    throw new Exception("El Codigo del Aeropuerto itroducido no existe - No se Modifica");
                else if (oAfectados == -1)
                    throw new Exception("El Codigo de la Ciudad no existe - No se Modifica");
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
        public static void Eliminar(Aeropuerto pAeropuerto)
        {
            //inicio la coneccion
            SqlConnection oConexion = new SqlConnection(CONEXION.STR);
            SqlCommand oComando = new SqlCommand("EliminarAeropuerto", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            //parametro de retorno para errores
            SqlParameter oRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
            SqlParameter oCodigoA = new SqlParameter("@codigoA", pAeropuerto.CodigoA);

            oRetorno.Direction = ParameterDirection.ReturnValue;
            oComando.Parameters.Add(oRetorno);
            oComando.Parameters.Add(oCodigoA);

            try
            {
                oConexion.Open();
                oComando.ExecuteNonQuery();

                int oAfectados = (int)oComando.Parameters["@Retorno"].Value;
                if (oAfectados == 0)
                    throw new Exception("No existe el Codigo de Aeropuerto - No se Elimina");
                else if (oAfectados == -1)
                    throw new Exception("El Aeropuerto tiene Vuelos asociados - No se Elimina");

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
        public static List<Aeropuerto> ListarAeropuerto()
        {

            SqlConnection oConexion = new SqlConnection(CONEXION.STR);
            SqlCommand oComando = new SqlCommand("ListarAeropuertos", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            Aeropuerto unA = null;
            List<Aeropuerto> _lista = new List<Aeropuerto>();

             try
            {
                oConexion.Open();

                SqlDataReader oReader = oComando.ExecuteReader();

                if (oReader.HasRows)
                {
                    while (oReader.Read())
                    {

                        unA = new Aeropuerto(oReader["CodigoA"].ToString(),
                        oReader["NombreA"].ToString(),
                        oReader["Direccion"].ToString(),
                        Convert.ToInt64(oReader["ImpuestoOri"]),
                    Convert.ToInt64(oReader["ImpuestoDes"]),
                    PercistenciaCiudad.Buscar(oReader["CodigoC"].ToString()));
                        _lista.Add(unA);
                    }
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

            return _lista;
        
        }
    }
}
