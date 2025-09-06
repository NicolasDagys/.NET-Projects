using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;
using Entidades_Compartidas;

namespace Persistencia
{
    public class PercistenciaCiudad
    {
        public static Ciudad Buscar(string pCodigoC)
        {
            SqlConnection oConexion = new SqlConnection(CONEXION.STR);
            SqlCommand oComando = new SqlCommand("BuscarCiudad", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            Ciudad C = null;
            SqlDataReader oReader;

            //Parametro para buscar
            oComando.Parameters.AddWithValue("@codigoC", pCodigoC);

            try
            {
                oConexion.Open();
                oReader = oComando.ExecuteReader();

                if (oReader.HasRows)
                {
                    oReader.Read();

                    C = new Ciudad(oReader["CodigoC"].ToString(),
                        oReader["NombreC"].ToString(),
                        oReader["Pais"].ToString());
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
        public static void Agregar(Ciudad pCiudad)
        {
            //inicio la coneccion
            SqlConnection oConexion = new SqlConnection(CONEXION.STR);
            SqlCommand oComando = new SqlCommand("AgregarCiudad", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            //agrego imput parametros del SP
            oComando.Parameters.AddWithValue("@codigoC", pCiudad.CodigoC);
            oComando.Parameters.AddWithValue("@nombreC", pCiudad.Nombre);
            oComando.Parameters.AddWithValue("@pais", pCiudad.Pais);

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
                    throw new Exception("El Codigo de la Ciudad ya existe");
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
        public static void Modificar(Ciudad pCiudad)
        {
            //inicio la coneccion
            SqlConnection oConexion = new SqlConnection(CONEXION.STR);
            SqlCommand oComando = new SqlCommand("ModificarCiudad", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            //agrego imput parametros del SP
            oComando.Parameters.AddWithValue("@codigoC", pCiudad.CodigoC);
            oComando.Parameters.AddWithValue("@nombreC", pCiudad.Nombre);
            oComando.Parameters.AddWithValue("@pais", pCiudad.Pais);

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
        public static void Eliminar(Ciudad pCiudad)
        {
            //inicio la coneccion
            SqlConnection oConexion = new SqlConnection(CONEXION.STR);
            SqlCommand oComando = new SqlCommand("EliminarCiudad", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            //parametro de retorno para errores
            SqlParameter oRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
            SqlParameter oCodigoC = new SqlParameter("@codigoC", pCiudad.CodigoC);

            oRetorno.Direction = ParameterDirection.ReturnValue;
            oComando.Parameters.Add(oRetorno);
            oComando.Parameters.Add(oCodigoC);

            try
            {
                oConexion.Open();
                oComando.ExecuteNonQuery();

                int oAfectados = (int)oComando.Parameters["@Retorno"].Value;
                if (oAfectados == -1)
                    throw new Exception("No existe el Codigo de Ciudad introducido - No se Elimina");
                else if (oAfectados == -2)
                    throw new Exception("La Ciudad tiene Aeropuertos asociados - No se Elimina");

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
    }
}
