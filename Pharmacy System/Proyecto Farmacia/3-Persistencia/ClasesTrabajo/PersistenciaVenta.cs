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
    internal class PersistenciaVenta: Interfaces.IPVenta
    {
        //singleton
        private static PersistenciaVenta _instancia = null;
        private PersistenciaVenta() { }
        public static PersistenciaVenta GetInstancia()
        {
            if (_instancia == null)
                _instancia = new PersistenciaVenta();
            return _instancia;
        }

        //operaciones

        public void Alta(Venta unaVenta, Empleado empLog)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(empLog.UsuEmp, empLog.PassEmp));
            SqlCommand _comando = new SqlCommand("AltaVenta", _cnn);
            _comando.CommandType = System.Data.CommandType.StoredProcedure;

            _comando.Parameters.AddWithValue("@DirEnvioVent",unaVenta.DirEnvIoVent);
            _comando.Parameters.AddWithValue("@TotalVent", unaVenta.TotalVent);
            _comando.Parameters.AddWithValue("@CiCli", unaVenta.UnCliente.CiCli);

            SqlParameter _retorno = new SqlParameter("@Retorno", System.Data.SqlDbType.Int);
            _retorno.Direction = System.Data.ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);

            SqlTransaction _transaccion = null;

            try
            {
                _cnn.Open();
                _transaccion = _cnn.BeginTransaction();
                _comando.Transaction = _transaccion;

                _comando.ExecuteNonQuery();
                int _CodVent = Convert.ToInt32(_retorno.Value);

                //manejo excepciones
                if (_CodVent == -1)
                    throw new Exception("No se puede dar de alta - No existe cliente");
                else if (_CodVent == -2)
                    throw new Exception("Error en Alta");

                // ingreso las líneas
                foreach (Linea l in unaVenta.ListaLineas)
                    PersistenciaLinea.GetInstancia().Alta(_CodVent, l, _transaccion);   

                //Asingo el valor 1 como primer Estado
              //PersistenciaAsignacion.GetInstancia().AsignoEstado(_CodVent, _transaccion);
                
                _transaccion.Commit(); 
            }
           catch (Exception ex)
           {
                _transaccion.Rollback();
                throw ex;
           }
           finally
           {
               _cnn.Close();
           }
        }

        public void CambioEstadoVenta(Venta unaVenta, Empleado empLog)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(empLog.UsuEmp, empLog.PassEmp));
            SqlCommand _comando = new SqlCommand("AsignoEstadoVenta", _cnn);

            _comando.CommandType = CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@NumVent", unaVenta.NumVent);

            SqlParameter _retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _retorno.Direction = ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);

            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();

                if ((int)_retorno.Value == -1)
                    throw new Exception("No se puede modificar, la venta no existe ");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("Estado último (devuelto)");
                else if ((int)_retorno.Value == -3)
                    throw new Exception("Error en cambio de estado");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _cnn.Close();
            }
        }
        
        public List<Venta> ListarVentasTodas(Empleado empLog)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(empLog.UsuEmp, empLog.PassEmp));
            Venta unaVenta = null;
            Cliente unCliente = null;
            List<Venta> _lista = new List<Venta>();

            SqlCommand _comando = new SqlCommand("ListarVentas", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    while (_lector.Read())
                    {
                        int _NumVent = (int)_lector["NumVent"];
                        DateTime _FechaVent = (DateTime)_lector["FechaVent"];
                        string _DirEnvioVent = (string)_lector["DirEnvioVent"];
                        int _PTotalVent = (int)_lector["TotalVent"];
                        string _CiCli = (string)_lector["CiCli"];

                        unCliente = PersistenciaCliente.GetInstancia().Buscar(_CiCli, empLog);

                        unaVenta = new Venta(_NumVent, _FechaVent, _DirEnvioVent, _PTotalVent, unCliente, 
                                   PersistenciaAsignacion.GetInstancia().ListadoEstadosVenta(_NumVent, empLog), 
                                   PersistenciaLinea.GetInstancia().ListarLineas(_NumVent, empLog));
                        _lista.Add(unaVenta);
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
