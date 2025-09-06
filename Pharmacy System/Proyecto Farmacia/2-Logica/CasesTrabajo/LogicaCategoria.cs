using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1_EC;
using _3_Persistencia;

namespace _2_Logica.ClasesTrabajo
{
    internal class LogicaCategoria : Interfaces.ILogicaCategoria
    {
        // ===== SINGLETON ===== //
        private static LogicaCategoria _instancia = null;
        private LogicaCategoria() { }
        public static LogicaCategoria GetInstancia()
        {
            if (_instancia == null)
                _instancia = new LogicaCategoria();
            return _instancia;
        }

        // ===== OPERACIONES ===== //
        public void Alta(Categoria unaC, Empleado empLog)
        {
            FabricaPersistencia.GetPersistenciaCategoria().Alta(unaC, empLog);
        }

        public void Baja(Categoria unaC, Empleado empLog)
        {
            FabricaPersistencia.GetPersistenciaCategoria().Baja(unaC, empLog);
        }

        public void Modificar(Categoria unaC, Empleado empLog)
        {
            FabricaPersistencia.GetPersistenciaCategoria().Modificar(unaC, empLog);
        }

        public Categoria BuscarActiva(string CodCat, Empleado empLog)
        {
            return (FabricaPersistencia.GetPersistenciaCategoria().BuscarActiva(CodCat, empLog));
        }

        public List<Categoria> ListarActivas(Empleado empLog)
        {
            return (FabricaPersistencia.GetPersistenciaCategoria().ListarActivas(empLog));
        }
    }
}
