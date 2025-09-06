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
    internal class PersistenciaCategoria: Interfaces.IPCategoria
    {
        //singleton
        private static PersistenciaCategoria _instancia = null;
        private PersistenciaCategoria() { }
        public static PersistenciaCategoria GetInstancia()
        {
            if (_instancia == null)
                _instancia = new PersistenciaCategoria ();
            return _instancia;
        }

        //operaciones
        public void Alta(Categoria unaCategoria, Empleado empLog)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(empLog.UsuEmp, empLog.PassEmp));

            SqlCommand _comando = new SqlCommand("AltaCategoria", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@CodCat", unaCategoria.CodCat);
            _comando.Parameters.AddWithValue("@NomCat", unaCategoria.NomCat);

            SqlParameter _retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _retorno.Direction = ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);

            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();

                if ((int)_retorno.Value == -1)
                    throw new Exception("No se puede dar de alta: Existe Categoría");
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


        public void Baja(Categoria unaCategoria, Empleado empLog)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(empLog.UsuEmp, empLog.PassEmp));

            SqlCommand _comando = new SqlCommand("BajaCategoria", _cnn);
            _comando.CommandType = System.Data.CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@CodCat", unaCategoria.CodCat);
            SqlParameter _retorno = new SqlParameter("@Retorno", System.Data.SqlDbType.Int);
            _retorno.Direction = System.Data.ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);

            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();
                if ((int)_retorno.Value == -1)
                    throw new Exception("No existe Categoría");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("Error en Baja de Categoría");
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


        public void Modificar(Categoria unaCategoria, Empleado empLog)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(empLog.UsuEmp, empLog.PassEmp));
            SqlCommand _comando = new SqlCommand("ModificarCategoria", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@CodCat", unaCategoria.CodCat);
            _comando.Parameters.AddWithValue("@NomCat", unaCategoria.NomCat);

            SqlParameter _retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _retorno.Direction = ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);

            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();

                if ((int)_retorno.Value == -1)
                    throw new Exception("No se puede modificar, no existe el categoría");
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


        public Categoria BuscarActiva(string unCodigo, Empleado empLog)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(empLog.UsuEmp, empLog.PassEmp));
            Categoria _unaCategoria = null;

            SqlCommand _comando = new SqlCommand("BuscarCategoriaActivo", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@CodCat", unCodigo);

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    _lector.Read();
                    _unaCategoria = new Categoria(unCodigo, (string)_lector["NomCat"]);
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
            return _unaCategoria;
        }


        internal Categoria BuscarTodas(string unCodigo, Empleado empLog)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(empLog.UsuEmp, empLog.PassEmp));
            Categoria _unaCategoria = null;

            SqlCommand _comando = new SqlCommand("BuscarCategoriaTodos", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@CodCat", unCodigo);

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    _lector.Read();
                    _unaCategoria = new Categoria(unCodigo, (string)_lector["NomCat"]);
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
            return _unaCategoria;
        }


        public List<Categoria> ListarActivas(Empleado empLog)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(empLog.UsuEmp, empLog.PassEmp));
            Categoria _unaCategoria = null;
            List<Categoria> _lista = new List<Categoria>();

            SqlCommand _comando = new SqlCommand("ListarCategoriasActivas", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    while (_lector.Read())
                    {
                        _unaCategoria = new Categoria ((string)_lector["CodCat"], (string)_lector["NomCat"]);
                        _lista.Add(_unaCategoria);
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
