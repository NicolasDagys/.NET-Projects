using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1_EC;
using _3_Persistencia;

namespace _2_Logica.Interfaces
{
    public interface ILogicaArticulo
    {
        void Alta(Articulo unA, Empleado empLog);

        void Baja(Articulo unA, Empleado empLog);

        void Modificar(Articulo unA, Empleado empLog);

        Articulo BuscarActivo(string CodArt, Empleado empLog);

        List<Articulo> ListarActivos(Empleado empLog);

        List<Articulo> ListarArtNoVenc(Empleado empLog);
    }
}
