using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;
using Entidades_Compartidas;

namespace Persistencia
{
    public class PersistenciaVuelo
    {
        public static Vuelo Buscar(string pCodigoV)
        {
            SqlConnection oConexion = new SqlConnection(CONEXION.STR);
            SqlCommand oComando = new SqlCommand("BuscarVuelo", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            Vuelo V = null;
            SqlDataReader oReader;

            //Parametro para buscar
            oComando.Parameters.AddWithValue("@codigoV", pCodigoV);

            try
            {
                oConexion.Open();
                oReader = oComando.ExecuteReader();

                if (oReader.HasRows) //aca da false y deberia dar true
                {
                    oReader.Read();

                    V = new Vuelo(oReader["CodigoV"].ToString(),
                        Convert.ToDateTime(oReader["FechaPartida"]),
                        Convert.ToDateTime(oReader["FechaLlegada"]),
                        Convert.ToInt32(oReader["Precio"]),
                        Convert.ToInt32(oReader["CantAsientos"]),
                        PersistenciaAeropuerto.Buscar(oReader["CodigoA1Salida"].ToString()),
                        PersistenciaAeropuerto.Buscar(oReader["CodigoA2Llegada"].ToString()));
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
            return V;
        }
        public static void Agregar(Vuelo pVuelo)
        {
            //inicio la coneccion
            SqlConnection oConexion = new SqlConnection(CONEXION.STR);
            SqlCommand oComando = new SqlCommand("AgregarVuelo", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            //agrego imput parametros del SP
            oComando.Parameters.AddWithValue("@codigoV", pVuelo.CodigoV);
            oComando.Parameters.AddWithValue("@codigoA1", pVuelo.unAPartida.CodigoA);
            oComando.Parameters.AddWithValue("@codigoA2", pVuelo.unALlegada.CodigoA);
            oComando.Parameters.AddWithValue("@fechaPartida", pVuelo.FechaPartida);
            oComando.Parameters.AddWithValue("@fechaLlegada", pVuelo.FechaLlegada);
            oComando.Parameters.AddWithValue("@precio", pVuelo.Precio);
            oComando.Parameters.AddWithValue("@cantAsientos", pVuelo.CantAsientos);

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
                if (oAfectados == 0)
                    throw new Exception("El Codigo de Vuelo ya existe");
                else if (oAfectados == -1)
                    throw new Exception("El Codigo de Aeropuerto de Origen no existe");
                else if (oAfectados == -2)
                    throw new Exception("El Codigo de Aeropuerto de Destino no existe");

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
        public static List<Vuelo> ListaVuelosPartida(Aeropuerto unA)
        {
            SqlConnection oConexion = new SqlConnection(CONEXION.STR);
            SqlCommand oComando = new SqlCommand("ListarPartidas", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;
            oComando.Parameters.AddWithValue("@codigoA1", unA.CodigoA);

            Vuelo unV = null;
            List<Vuelo> _lista = new List<Vuelo>();

            try
            {
                oConexion.Open();

                SqlDataReader _lector = oComando.ExecuteReader();

                if (_lector.HasRows)
                {
                    while (_lector.Read())
                    {

                        unV = new Vuelo(_lector["CodigoV"].ToString(),
                            Convert.ToDateTime(_lector["FechaPartida"]),
                            Convert.ToDateTime(_lector["FechaLlegada"]),
                            Convert.ToInt32(_lector["Precio"]),
                            Convert.ToInt32(_lector["CantAsientos"]),
                            unA,
                            PersistenciaAeropuerto.Buscar(_lector["CodigoA2Llegada"].ToString()));
                        _lista.Add(unV);
                    }
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

            return _lista;
        }
        public static List<Vuelo> ListaVuelosLlegada(Aeropuerto unA)
        {
            SqlConnection oConexion = new SqlConnection(CONEXION.STR);
            SqlCommand oComando = new SqlCommand("ListarArribos", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;
            oComando.Parameters.AddWithValue("@codigoA2", unA.CodigoA);

            Vuelo unV = null;
            List<Vuelo> _lista = new List<Vuelo>();

            try
            {
                oConexion.Open();

                SqlDataReader _lector = oComando.ExecuteReader();

                if (_lector.HasRows)
                {
                    while (_lector.Read())
                    {

                        unV = new Vuelo(_lector["CodigoV"].ToString(),
                            Convert.ToDateTime(_lector["FechaPartida"]),
                            Convert.ToDateTime(_lector["FechaLlegada"]),
                            Convert.ToInt32(_lector["Precio"]),
                            Convert.ToInt32(_lector["CantAsientos"]),
                            PersistenciaAeropuerto.Buscar(_lector["CodigoA2Llegada"].ToString()),
                            unA);
                        _lista.Add(unV);
                    }
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

            return _lista;
        }
    }
}
