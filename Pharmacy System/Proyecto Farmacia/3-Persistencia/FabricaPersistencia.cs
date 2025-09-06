using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3_Persistencia
{
    public class FabricaPersistencia
    {
        public static Interfaces.IPArticulo GetPersistenciaArticulo()
        {
            return (ClasesTrabajo.PersistenciaArticulo.GetInstancia());
        }

        public static Interfaces.IPCategoria GetPersistenciaCategoria()
        {
            return (ClasesTrabajo.PersistenciaCategoria.GetInstancia());
        }

        public static Interfaces.IPCliente GetPersistenciaCliente()
        {
            return (ClasesTrabajo.PersistenciaCliente.GetInstancia());
        }

        public static Interfaces.IPEmpleado GetPersistenciaEmpleado()
        {
            return (ClasesTrabajo.PersistenciaEmpleado.GetInstancia());
        }

        public static Interfaces.IPEstado GetPersistenciaEstado()
        {
            return (ClasesTrabajo.PersistenciaEstado.GetInstancia());
        }

        public static Interfaces.IPVenta GetPersistenciaVenta()
        {
            return (ClasesTrabajo.PersistenciaVenta.GetInstancia());
        }
    }
}
