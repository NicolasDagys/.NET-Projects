using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Entidades_Compartidas;
using Persistencia;

namespace Logica
{
    public class LogicaCliente
    {
        public static Cliente BuscarCliente(Int64 pPasaporte)
        {
            return PersistenciaCliente.Buscar(pPasaporte);
        }
        public static void AltaCliente(Cliente unC)
        {
            PersistenciaCliente.Agregar(unC);
        }
        public static void ModificarCliente(Cliente unC)
        {
            PersistenciaCliente.Modificar(unC);
        }
        public static void EliminarCliente(Cliente unC)
        {
            PersistenciaCliente.Eliminar(unC);
        }
        public static Cliente Logueo(Int64 pPasaporte, string pPass)
        {
            return PersistenciaCliente.Logueo(pPasaporte, pPass);
        }

    }
}

