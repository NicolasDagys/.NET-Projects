using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Entidades_Compartidas;
using Persistencia;

namespace Logica
{
    public class LogicaAeropuerto
    {
        public static Aeropuerto BuscarAeropuerto(string pCodigoA)
        {
            return PersistenciaAeropuerto.Buscar(pCodigoA);
        }
        public static void AltaAeropuerto(Aeropuerto unA)
        {
            PersistenciaAeropuerto.Agregar(unA);
        }
        public static void ModificarAeropuerto(Aeropuerto unA)
        {
            PersistenciaAeropuerto.Modificar(unA);
        }
        public static void EliminarAeropuerto(Aeropuerto unA)
        {
            PersistenciaAeropuerto.Eliminar(unA);
        }
        public static List<Aeropuerto> ListarAeropuertos()
        {
           return PersistenciaAeropuerto.ListarAeropuerto();
        }
    }
}
