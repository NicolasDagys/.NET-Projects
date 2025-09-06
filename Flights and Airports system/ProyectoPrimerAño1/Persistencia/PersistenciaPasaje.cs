using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;
using Entidades_Compartidas;

namespace Persistencia
{
    public class PersistenciaPasaje
    {
        public static void Agregar(Pasaje pPasaje)
        {
            //inicio la coneccion
            SqlConnection oConexion = new SqlConnection(CONEXION.STR);
            SqlCommand oComando = new SqlCommand("AgregarPasaje", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            //agrego imput parametros del SP
            oComando.Parameters.AddWithValue("@codigoV", pPasaje.unV.CodigoV);
            oComando.Parameters.AddWithValue("@pasaportre", pPasaje.unC.Pasaporte);
            oComando.Parameters.AddWithValue("@fechaVenta", pPasaje.FechaVenta);
            oComando.Parameters.AddWithValue("@precioTot", pPasaje.PrecioTot);
            

            //parametro de retorno para errores
            SqlParameter oRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
            oRetorno.Direction = ParameterDirection.ReturnValue;
            oComando.Parameters.Add(oRetorno);

            int oAfectados = 0;
            try
            {
                oConexion.Open();
                oComando.ExecuteNonQuery();

                oAfectados = (int)oComando.Parameters["@Retorno"].Value;
                if (oAfectados == -1)
                    throw new Exception("El Vodigo de Vuelo no existe");
                else if (oAfectados == -2)
                    throw new Exception("El Pasaporte del Cliente no existe");

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
        public static List<Pasaje> ListadoPasajeXCliente(Cliente pUnC)
        {

            SqlConnection oConexion = new SqlConnection(CONEXION.STR);
            SqlCommand oComando = new SqlCommand("PasajeXCliente", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;
            oComando.Parameters.AddWithValue("@pasaporte", pUnC.Pasaporte);

            List<Pasaje> oListaPasajeXCli = new List<Pasaje>();
            //Pasaje p = null;
            SqlDataReader oReader;

            try
            {
                oConexion.Open();
                oReader = oComando.ExecuteReader();

                if (oReader.HasRows)
                {
                    while (oReader.Read())
                    {
                        Pasaje P = new Pasaje(Convert.ToInt32(oReader["NumV"]),
                            Convert.ToDateTime(oReader["FechaVenta"]),
                            Convert.ToInt32(oReader["PrecioTot"]),
                            pUnC,
                            PersistenciaVuelo.Buscar(oReader["CodigoV"].ToString()));
                        oListaPasajeXCli.Add(P);
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
            return oListaPasajeXCli;

        }
    }
}
