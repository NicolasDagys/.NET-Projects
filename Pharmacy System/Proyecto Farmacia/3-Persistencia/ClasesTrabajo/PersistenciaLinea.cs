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
    internal class PersistenciaLinea
    {
        private static PersistenciaLinea _instancia;
        private PersistenciaLinea() { }

        public static PersistenciaLinea GetInstancia()
        {
            if (_instancia == null)
                _instancia = new PersistenciaLinea();

            return _instancia;
        }

        internal void Alta(int nroVenta, Linea unaLinea, SqlTransaction trn)
        {
            SqlCommand _comando = new SqlCommand("AltaLinea ", trn.Connection);
            _comando.CommandType = CommandType.StoredProcedure;

            _comando.Parameters.AddWithValue("@NumVent", nroVenta);
            _comando.Parameters.AddWithValue("@CodArt", unaLinea.UnArt.CodArt);
            _comando.Parameters.AddWithValue("@Cant", unaLinea.Cant);

            SqlParameter _retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _retorno.Direction = ParameterDirection.ReturnValue;
            _comando.Parameters.Add(_retorno);

            try
            {
                _comando.Transaction = trn;
                _comando.ExecuteNonQuery();
                if ((int)_retorno.Value == -1)
                    throw new Exception("Error - No existe artículo");
                else if ((int)_retorno.Value == -2)
                    throw new Exception("Error - No existe venta");
                else if ((int)_retorno.Value == -3)
                    throw new Exception("Error en Alta de la línea de la Venta");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal List<Linea> ListarLineas(int nroVenta, Empleado empLog)
        {
            SqlConnection _Conexion = new SqlConnection(Conexion.Cnn(empLog.UsuEmp, empLog.PassEmp));
            Linea unaLinea = null;
            Articulo unArticulo = null;
            List<Linea> _Lista = new List<Linea>();

            SqlCommand _comando = new SqlCommand("ListarLineas", _Conexion);
            _comando.CommandType = CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@NumVent", nroVenta);

            try
            {
                _Conexion.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                while (_lector.Read())
                {
                    string _CodArt = (string)_lector["CodArt"];
                    int Cant = (int)_lector["Cant"];
                    unArticulo = PersistenciaArticulo.GetInstancia().BuscarTodos(_CodArt, empLog);
                    unaLinea = new Linea(unArticulo, Cant);
                    _Lista.Add(unaLinea);
                }
                _lector.Close(); 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _Conexion.Close();
            }
            return _Lista;
        }
    }
}
