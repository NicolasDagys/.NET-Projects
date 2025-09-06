using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Entidades_Compartidas;
using Persistencia;

namespace Logica
{
    public class LogicaVuelo
    {
        public static Vuelo Buscar(string pCodigoV)
        {
            return PersistenciaVuelo.Buscar(pCodigoV);
        }
        public static void Agregar(Vuelo pVuelo)
        {
            if (pVuelo.FechaPartida > DateTime.Now)
                PersistenciaVuelo.Agregar(pVuelo);
            else
                throw new Exception("La fecha de partda del Vuelo no puede ser anterior a la fecha actual");
        }
        public static List<Vuelo> ListaVuelosPartida(Aeropuerto unA)
        {
           return PersistenciaVuelo.ListaVuelosPartida(unA);
        }
        public static List<Vuelo> ListaVuelosLlegada(Aeropuerto unA)
        {
            return PersistenciaVuelo.ListaVuelosLlegada(unA);
        }
    }
}
