using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1_EC;
using _3_Persistencia;

namespace _2_Logica.ClasesTrabajo
{
    internal class LogicaCliente: Interfaces.ILogicaCliente
    {
        // ===== SINGLETON ===== //
        private static LogicaCliente _instancia = null;
        private LogicaCliente() { }
        public static LogicaCliente GetInstancia()
        {
            if (_instancia == null)
                _instancia = new LogicaCliente();
            return _instancia;
        }

        // ===== OPERACIONES ===== //

        public void Alta(Cliente unC, Empleado empLog)
        {
            FabricaPersistencia.GetPersistenciaCliente().Alta(unC, empLog);
        }

        public void Modificar(Cliente unC, Empleado empLog)
        {
            FabricaPersistencia.GetPersistenciaCliente().Modificar(unC, empLog);
        }

        public Cliente Buscar(string CiCli, Empleado empLog)
        {
            return (FabricaPersistencia.GetPersistenciaCliente().Buscar(CiCli, empLog));
        }
        public List<Cliente> Listar(Empleado empLog)
        {
            return (FabricaPersistencia.GetPersistenciaCliente().Listar(empLog));
        }
    }
}
