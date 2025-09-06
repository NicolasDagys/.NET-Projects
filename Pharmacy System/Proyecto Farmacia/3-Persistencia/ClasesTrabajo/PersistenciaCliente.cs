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
    internal class PersistenciaCliente : Interfaces.IPCliente
    {
        //singleton
        private static PersistenciaCliente _instancia = null;

        private PersistenciaCliente() { }

        public static PersistenciaCliente GetInstancia()
        {
            if (_instancia == null)
                _instancia = new PersistenciaCliente();
            return _instancia;
        }

        //operaciones
        public void Alta(Cliente unCliente, Empleado empLog)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(empLog.UsuEmp, empLog.PassEmp));

            SqlCommand _comando = new SqlCommand("AltaCliente", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;

            // Parámetros
            _comando.Parameters.AddWithValue("@CiCli", unCliente.CiCli);
            _comando.Parameters.AddWithValue("@NomCli", unCliente.NomCli);
            _comando.Parameters.AddWithValue("@NumTarjCli", unCliente.NumTarjCli);
            _comando.Parameters.AddWithValue("@TelCli", unCliente.TelCli);

            // Parámetro de retorno
            SqlParameter _retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _retorno.Direction = ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);

            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();

                if ((int)_retorno.Value == -1)
                    throw new Exception("No se puede dar de alta, ya existe el cliente");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("Error al dar de alta");
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
        public void Modificar(Cliente unCliente, Empleado empLog)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(empLog.UsuEmp, empLog.PassEmp));

            SqlCommand _comando = new SqlCommand("ModificarCliente", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@CiCli", unCliente.CiCli);
            _comando.Parameters.AddWithValue("@NomCli", unCliente.NomCli);
            _comando.Parameters.AddWithValue("@NumTarjCli", unCliente.NumTarjCli);
            _comando.Parameters.AddWithValue("@TelCli", unCliente.TelCli);

            SqlParameter _retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _retorno.Direction = ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);

            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();

                if ((int)_retorno.Value == -1)
                    throw new Exception("No se puede modificar, no existe el cliente ");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("Error en modificación");
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

        public Cliente Buscar (string pCiCli, Empleado empLog)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(empLog.UsuEmp, empLog.PassEmp));
            Cliente _unCliente = null;

            SqlCommand _comando = new SqlCommand("BuscarCliente", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@CiCli", pCiCli);

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    _lector.Read();
                    _unCliente = new Cliente(pCiCli, (string)_lector["NomCli"], (string)_lector["NumTarjCli"], (string)_lector["TelCli"]);
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
            return _unCliente;
        }

        public List <Cliente> Listar(Empleado empLog)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(empLog.UsuEmp, empLog.PassEmp));
            Cliente _unCliente = null;
            List<Cliente> _lista = new List<Cliente>();

            SqlCommand _comando = new SqlCommand("ListarClientes", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    while (_lector.Read())
                    {
                        _unCliente = new Cliente((string) _lector ["CiCli"], (string)_lector["NomCli"], (string)_lector["NumTarjCli"], (string)_lector["TelCli"]);
                        _lista.Add(_unCliente);
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
