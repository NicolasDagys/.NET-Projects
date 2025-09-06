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
    internal class PersistenciaEstado: Interfaces.IPEstado
    {
        //singleton
        private static PersistenciaEstado _instancia = null;
        private PersistenciaEstado() { }
        public static PersistenciaEstado GetInstancia()
        {
            if (_instancia == null)
                _instancia = new PersistenciaEstado();
            return _instancia;
        }

        //operaciones
        
        public Estado Buscar (int pNumEst, Empleado empLog)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(empLog.UsuEmp, empLog.PassEmp));
            Estado _unEstado = null;

            SqlCommand _comando = new SqlCommand("BuscarEstado", _cnn);
            _comando.CommandType = System.Data.CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@NumEst", pNumEst);

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    _lector.Read();
                    _unEstado = new Estado (pNumEst, (string)_lector["NomEst"]);
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _cnn.Close();
            }
            return _unEstado;
        }

        public List<Estado> ListarEstados(Empleado empLog)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(empLog.UsuEmp, empLog.PassEmp));
            Estado _unEstado = null;
            List<Estado> _lista = new List<Estado>();

            SqlCommand _comando = new SqlCommand("ListarEstados", _cnn);
            _comando.CommandType = System.Data.CommandType.StoredProcedure;

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    while (_lector.Read())
                    {
                        _unEstado = new Estado((int)_lector["NumEst"], (string)_lector["NomEst"]);
                        _lista.Add(_unEstado);
                    }
                }
                _lector.Close();
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
