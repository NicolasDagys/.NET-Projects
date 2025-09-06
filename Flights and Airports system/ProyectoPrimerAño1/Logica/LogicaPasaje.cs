using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Entidades_Compartidas;
using Persistencia;

namespace Logica
{
    public class LogicaPasaje
    {
        public static void Agregar(Pasaje unP)
        {
            PersistenciaPasaje.Agregar(unP);
        }
        public static List<Pasaje> ListadoPasajeXCliente(Cliente UnC)
        {
            return PersistenciaPasaje.ListadoPasajeXCliente(UnC);
        }
    }
}
