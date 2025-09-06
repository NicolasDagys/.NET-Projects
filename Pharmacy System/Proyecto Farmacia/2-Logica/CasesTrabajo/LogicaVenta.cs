using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1_EC;
using _3_Persistencia;

namespace _2_Logica.ClasesTrabajo
{
    internal class LogicaVenta : Interfaces.ILogicaVenta
    {
        // ===== SINGLETON ===== //
        private static LogicaVenta _instancia = null;
        private LogicaVenta() { }
        public static LogicaVenta GetInstancia()
        {
            if (_instancia == null)
                _instancia = new LogicaVenta();
            return _instancia;
        }

        // ===== OPERACIONES ===== //

        public void Alta(Venta unaV, Empleado empLog)
        {
            FabricaPersistencia.GetPersistenciaVenta().Alta(unaV, empLog);
        }

        public void CambioEstadoVenta(Venta unaV, Empleado empLog)
        {
            FabricaPersistencia.GetPersistenciaVenta().CambioEstadoVenta(unaV, empLog);
        }
        public List<Venta> ListarVentasTodas(Empleado empLog)
        {
            return (FabricaPersistencia.GetPersistenciaVenta().ListarVentasTodas(empLog));
        }
    }
}
