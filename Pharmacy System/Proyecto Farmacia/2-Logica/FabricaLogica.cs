using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1_EC;
using _3_Persistencia; 

namespace _2_Logica
{
    public class FabricaLogica
    {
        public static Interfaces.ILogicaArticulo GetLogicaArticulo()
        {
            return (ClasesTrabajo.LogicaArticulo.GetInstancia());
        }

        public static Interfaces.ILogicaCategoria GetLogicaCategoria()
        {
            return (ClasesTrabajo.LogicaCategoria.GetInstancia());
        }

        public static Interfaces.ILogicaCliente GetLogicaCliente()
        {
            return (ClasesTrabajo.LogicaCliente.GetInstancia());
        }

        public static Interfaces.ILogicaEmpleado GetLogicaEmpleado()
        {
            return (ClasesTrabajo.LogicaEmpleado.GetInstancia());
        }

        public static Interfaces.ILogicaEstado GetLogicaEstado()
        {
            return (ClasesTrabajo.LogicaEstado.GetInstancia());
        }

        public static Interfaces.ILogicaVenta GetLogicaVenta()
        {
            return (ClasesTrabajo.LogicaVenta.GetInstancia());
        }
    }
}
