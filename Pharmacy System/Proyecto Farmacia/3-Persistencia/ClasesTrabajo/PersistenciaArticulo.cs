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
    internal class PersistenciaArticulo : Interfaces.IPArticulo
    {
        //singleton
        private static PersistenciaArticulo _instancia = null;
        private PersistenciaArticulo() { }
        public static PersistenciaArticulo GetInstancia()
        {
            if (_instancia == null)
                _instancia = new PersistenciaArticulo();
            return _instancia;
        }

        //operaciones
        public void Alta (Articulo unArticulo, Empleado empLog)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(empLog.UsuEmp, empLog.PassEmp));
            SqlCommand _comando = new SqlCommand("AltaArticulo", _cnn);

            _comando.CommandType = CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@CodArt", unArticulo.CodArt);
            _comando.Parameters.AddWithValue("@NomArt", unArticulo.NomArt);
            _comando.Parameters.AddWithValue("@PrecioArt", unArticulo.PrecioArt);
            _comando.Parameters.AddWithValue("@FechaVtoArt", unArticulo.FechaVtoArt);
            _comando.Parameters.AddWithValue("@TipoPArt", unArticulo.TipoPArt);
            _comando.Parameters.AddWithValue("@TamArt", unArticulo.TamArt);
            _comando.Parameters.AddWithValue("@CodCat", unArticulo.UnaCat.CodCat);

            SqlParameter _retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _retorno.Direction = ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);

            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();

                if ((int)_retorno.Value == -1)
                    throw new Exception("No se puede dar de alta, no existe Categoría");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("No se pude dar de alta, ya existe este artículo");
                else if ((int)_retorno.Value == -3)
                    throw new Exception("Error en alta de artículo");
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

        public void Baja(Articulo unArticulo, Empleado empLog)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(empLog.UsuEmp, empLog.PassEmp));
            SqlCommand _comando = new SqlCommand("BajaArticulo", _cnn);
            _comando.CommandType = System.Data.CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@CodArt", unArticulo.CodArt);
            SqlParameter _retorno = new SqlParameter("@Retorno", System.Data.SqlDbType.Int);
            _retorno.Direction = System.Data.ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);

            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();
                if ((int)_retorno.Value == -1)
                    throw new Exception("No existe Artículo");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("Error en Baja de Artículo");
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

        public void Modificar(Articulo unArticulo, Empleado empLog)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(empLog.UsuEmp, empLog.PassEmp));
            SqlCommand _comando = new SqlCommand("ModificarArticulo", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@CodArt", unArticulo.CodArt);
            _comando.Parameters.AddWithValue("@NomArt", unArticulo.NomArt);
            _comando.Parameters.AddWithValue("@PrecioArt", unArticulo.PrecioArt);
            _comando.Parameters.AddWithValue("@FechaVtoArt", unArticulo.FechaVtoArt);
            _comando.Parameters.AddWithValue("@TipoPArt", unArticulo.TipoPArt);
            _comando.Parameters.AddWithValue("@TamArt", unArticulo.TamArt);
            _comando.Parameters.AddWithValue("@CodCat", unArticulo.UnaCat.CodCat);

            SqlParameter _retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _retorno.Direction = ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);

            try
            {
                _cnn.Open();
                _comando.ExecuteNonQuery();

                if ((int)_retorno.Value == -1)
                    throw new Exception("No existe categoría");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("No existe artículo");
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

        public Articulo BuscarActivo(string unCodigo, Empleado empLog)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(empLog.UsuEmp, empLog.PassEmp));
            Articulo unArticulo = null;

            SqlCommand _comando = new SqlCommand("ArticuloBuscarActivo", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("CodArt", unCodigo);

            Categoria _unaCat = null;

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    _lector.Read();

                    string _NomArt = (string)_lector["NomArt"];
                    int _PrecioArt = (int)_lector["PrecioArt"];
                    DateTime _FechaVtoArt = (DateTime)_lector["FechaVtoArt"];
                    string _TipoPArt = (string)_lector["TipoPArt"];
                    int _TamArt = (int)_lector["TamArt"];
                    string _CodCat = (string)_lector["CodCat"];

                    _unaCat = PersistenciaCategoria.GetInstancia().BuscarTodas(_CodCat, empLog);
                    unArticulo = new Articulo(unCodigo, _NomArt, _PrecioArt, _FechaVtoArt,
                                                _TipoPArt, _TamArt, _unaCat);
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
            return unArticulo;
        }

        internal Articulo BuscarTodos(string unCodigo, Empleado empLog)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(empLog.UsuEmp, empLog.PassEmp));
            Articulo unArticulo = null;

            SqlCommand _comando = new SqlCommand("ArticuloBuscarTodos", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("CodArt", unCodigo);

            Categoria _unaCat = null;

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    _lector.Read();

                    string _NomArt = (string)_lector["NomArt"];
                    int _PrecioArt = (int)_lector["PrecioArt"];
                    DateTime _FechaVtoArt = (DateTime)_lector["FechaVtoArt"];
                    string _TipoPArt = (string)_lector["TipoPArt"];
                    int _TamArt = (int)_lector["TamArt"];
                    string _CodCat = (string)_lector["CodCat"];

                    _unaCat = PersistenciaCategoria.GetInstancia().BuscarTodas(_CodCat, empLog);
                    unArticulo = new Articulo(unCodigo, _NomArt, _PrecioArt, _FechaVtoArt,
                                                _TipoPArt, _TamArt, _unaCat);
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
            return unArticulo;
        }

        public List<Articulo> ListarActivos(Empleado empLog)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(empLog.UsuEmp, empLog.PassEmp));
            Articulo unArticulo = null;
            Categoria unaCat = null;
            List<Articulo> _lista = new List<Articulo>();

            SqlCommand _comando = new SqlCommand("ListarTodosArticulos", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    while (_lector.Read())
                    {
                        string _CodArt = (string)_lector["CodArt"];
                        string _NomArt = (string)_lector["NomArt"];
                        int _PrecioArt = (int)_lector["PrecioArt"];
                        DateTime _FechaVtoArt = (DateTime)_lector["FechaVtoArt"];
                        string _TipoPArt = (string)_lector["TipoPArt"];
                        int _TamArt = (int)_lector["TamArt"];
                        string _CodCat = (string)_lector["CodCat"];

                        unaCat = PersistenciaCategoria.GetInstancia().BuscarTodas(_CodCat, empLog);
                        unArticulo = new Articulo(_CodArt, _NomArt, _PrecioArt, _FechaVtoArt,
                                                    _TipoPArt, _TamArt, unaCat);
                        _lista.Add(unArticulo);
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

        public List<Articulo> ListarArtNoVenc(Empleado empLog)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(empLog.UsuEmp, empLog.PassEmp));
            Articulo unArticulo = null;
            Categoria unaCat = null;
            List<Articulo> _lista = new List<Articulo>();

            SqlCommand _comando = new SqlCommand("ListarArticulosNoVencidos", _cnn);
            _comando.CommandType = CommandType.StoredProcedure;

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (_lector.HasRows)
                {
                    while (_lector.Read())
                    {
                        string _CodArt = (string)_lector["CodArt"];
                        string _NomArt = (string)_lector["NomArt"];
                        int _PrecioArt = (int)_lector["PrecioArt"];
                        DateTime _FechaVtoArt = (DateTime)_lector["FechaVtoArt"];
                        string _TipoPArt = (string)_lector["TipoPArt"];
                        int _TamArt = (int)_lector["TamArt"];
                        string _CodCat = (string)_lector["CodCat"];

                        unaCat = PersistenciaCategoria.GetInstancia().BuscarTodas(_CodCat, empLog);
                        unArticulo = new Articulo(_CodArt, _NomArt, _PrecioArt, _FechaVtoArt,
                                                    _TipoPArt, _TamArt, unaCat);
                        _lista.Add(unArticulo);
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
