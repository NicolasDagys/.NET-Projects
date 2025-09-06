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
    internal class PersistenciaEmpleado: Interfaces.IPEmpleado
    {
        //singleton
        private static PersistenciaEmpleado  _instancia = null;
        private PersistenciaEmpleado() { }
        public static PersistenciaEmpleado GetInstancia()
        {
            if (_instancia == null)
                _instancia = new  PersistenciaEmpleado();
            return _instancia;
        }

        public Empleado Logueo (string pUsuEmp, string pPassEmp)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn(pUsuEmp, pPassEmp));
            Empleado _unEmpleado = null;

            SqlCommand _comando = new SqlCommand("LogueoEmpleado", _cnn);
            _comando.CommandType = System.Data.CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@UsuEmp", pUsuEmp);
            _comando.Parameters.AddWithValue("@PassEmp", pPassEmp);
            
            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();

                
                if (_lector.HasRows)
                {
                    _lector.Read();
                    _unEmpleado = new Empleado ((string)_lector["NomEmp"], pUsuEmp, pPassEmp);
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
            return _unEmpleado;
        }
    }
}
