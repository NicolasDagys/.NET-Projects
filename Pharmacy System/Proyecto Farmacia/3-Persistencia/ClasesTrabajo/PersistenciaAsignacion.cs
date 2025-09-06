using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using _1_EC;
using System.Data.SqlClient;
using System.Data;

namespace _3_Persistencia.ClasesTrabajo
{
    internal class PersistenciaAsignacion
    {
        private static PersistenciaAsignacion _instancia;
        private PersistenciaAsignacion() { }

        public static PersistenciaAsignacion GetInstancia()
        {
            if (_instancia == null)
                _instancia = new PersistenciaAsignacion();

            return _instancia;
        }

        internal void AsignoEstado(int nroVent, SqlTransaction trn)
        {
            SqlCommand _comando = new SqlCommand("AsignoEstadoVenta", trn.Connection);
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@NumVent", nroVent);

            SqlParameter _retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _retorno.Direction = ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);
            try
            {
                _comando.Transaction = trn;
                _comando.ExecuteNonQuery();
                if ((int)_retorno.Value == -1)
                    throw new Exception("No se puede modificar una venta que no existe");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("Último estado - Devuelto");
                else if ((int)_retorno.Value == -3)
                    throw new Exception("Error en cambio de estado de a venta");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        

        internal List <Asignacion> ListadoEstadosVenta(int nroVent, Empleado empLog) 
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(empLog.UsuEmp, empLog.PassEmp));
            Asignacion unaAsignacion = null;
            Estado unEstado = null;
            List<Asignacion> _lista = new List<Asignacion>();

            SqlCommand _comando = new SqlCommand("ListarEstadosVenta", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@NumVent", nroVent);

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    while (_lector.Read())
                    {
                        int _NumEst = (int)_lector["NumEst"];
                        DateTime _FyHEst = (DateTime)_lector["FyHEst"];

                        unEstado = PersistenciaEstado.GetInstancia().Buscar(_NumEst, empLog);
                        unaAsignacion = new Asignacion(unEstado, _FyHEst);
                        _lista.Add(unaAsignacion);
                    }
                    _lector.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _cnn.Close();
            }
            return _lista;
        }
    }
}
