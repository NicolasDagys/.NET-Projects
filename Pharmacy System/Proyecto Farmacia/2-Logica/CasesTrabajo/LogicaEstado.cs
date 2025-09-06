using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1_EC;
using _3_Persistencia;

namespace _2_Logica.ClasesTrabajo
{
    internal class LogicaEstado : Interfaces.ILogicaEstado
    {
        // ===== SINGLETON ===== //
        private static LogicaEstado _instancia = null;
        private LogicaEstado() { }
        public static LogicaEstado GetInstancia()
        {
            if (_instancia == null)
                _instancia = new LogicaEstado();
            return _instancia;
        }

        // ===== OPERACIONES ===== //
        public Estado Buscar(int NumEst, Empleado empLog)
        {
            return (FabricaPersistencia.GetPersistenciaEstado().Buscar(NumEst, empLog));
        }
        public List<Estado> ListarEstados(Empleado empLog)
        {
            return (FabricaPersistencia.GetPersistenciaEstado().ListarEstados(empLog));
        }
    }
}
