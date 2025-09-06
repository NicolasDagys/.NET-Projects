using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1_EC;
using _3_Persistencia;


namespace _2_Logica.ClasesTrabajo
{
    internal class LogicaArticulo : Interfaces.ILogicaArticulo
    {
        // ===== SINGLETON ===== //
        private static LogicaArticulo _instancia = null;
        private LogicaArticulo() { }
        public static LogicaArticulo GetInstancia()
        {
            if (_instancia == null)
                _instancia = new LogicaArticulo();
            return _instancia;
        }

        // ===== OPERACIONES ===== //
        public void Alta(Articulo unA, Empleado empLog)
        {
            if (unA.FechaVtoArt <= DateTime.Today)
                throw new Exception("La fecha de vencimiento es anterior o igual a la fecha actual - producto vencido");
            else
                FabricaPersistencia.GetPersistenciaArticulo().Alta(unA, empLog);
        }

        public void Baja(Articulo unA, Empleado empLog)
        {
            FabricaPersistencia.GetPersistenciaArticulo().Baja(unA, empLog);
        }

        public void Modificar(Articulo unA, Empleado empLog)
        {
            if (unA.FechaVtoArt <= DateTime.Today)
                throw new Exception("La fecha de vencimiento es anterior o igual a la fecha actual - producto vencido");
            else
                FabricaPersistencia.GetPersistenciaArticulo().Modificar(unA, empLog);
        }

        public Articulo BuscarActivo(string CodArt, Empleado empLog)
        {
            return (FabricaPersistencia.GetPersistenciaArticulo().BuscarActivo(CodArt, empLog));
        }

        public List<Articulo> ListarActivos(Empleado empLog)
        {
            return (FabricaPersistencia.GetPersistenciaArticulo().ListarActivos(empLog));
        }

        public List<Articulo> ListarArtNoVenc(Empleado empLog)
        {
            return (FabricaPersistencia.GetPersistenciaArticulo().ListarArtNoVenc(empLog));
        }
    }
}
