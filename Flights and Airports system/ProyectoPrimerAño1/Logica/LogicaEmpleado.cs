using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Entidades_Compartidas;
using Persistencia;

namespace Logica
{
    public class LogicaEmpleado
    {
        public static Empleado Logueo(string pUsuario, string pContrasenia)
        {
            return PersistenciaEmpleado.Logueo(pUsuario, pContrasenia);
        }
    }
}
