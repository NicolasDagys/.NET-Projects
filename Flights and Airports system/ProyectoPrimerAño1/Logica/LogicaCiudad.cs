using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Entidades_Compartidas;
using Persistencia;

namespace Logica
{
    public class LogicaCiudad
    {
        public static Ciudad BuscarCiudad(string pCodigoC)
        {

            return PercistenciaCiudad.Buscar(pCodigoC);
        }
        public static void AltaCiudad(Ciudad unaC)
        {
            PercistenciaCiudad.Agregar(unaC);
        }

        public static void ModificarCiudad(Ciudad unaC)
        {
            PercistenciaCiudad.Modificar(unaC);
        }

        public static void EliminarCiudad(Ciudad unaC)
        {
            PercistenciaCiudad.Eliminar(unaC);
        }
    }
}
