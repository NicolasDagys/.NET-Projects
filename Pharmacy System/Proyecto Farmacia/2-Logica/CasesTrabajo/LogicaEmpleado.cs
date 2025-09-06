using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1_EC;
using _3_Persistencia;

namespace _2_Logica.ClasesTrabajo
{
    internal class LogicaEmpleado : Interfaces.ILogicaEmpleado
    {
        // ===== SINGLETON ===== //
        private static LogicaEmpleado _instancia = null;
        private LogicaEmpleado() { }
        public static LogicaEmpleado GetInstancia()
        {
            if (_instancia == null)
                _instancia = new LogicaEmpleado();
            return _instancia;
        }

        // ===== OPERACIONES ===== //

        public Empleado Logueo(string pUsuEmp, string pPassEmp)
        {
            return (FabricaPersistencia.GetPersistenciaEmpleado().Logueo(pUsuEmp, pPassEmp));
        }
    }
}
