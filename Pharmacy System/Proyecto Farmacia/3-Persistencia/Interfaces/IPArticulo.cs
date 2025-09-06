using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1_EC;

namespace _3_Persistencia.Interfaces
{
    public interface IPArticulo
    {
        void Alta(Articulo unArticulo, Empleado empLog);
        void Baja(Articulo unArticulo, Empleado empLog);
        void Modificar(Articulo unArticulo, Empleado empLog);
        Articulo BuscarActivo(string unCodigo, Empleado empLog);
        List<Articulo> ListarActivos(Empleado empLog);
        List<Articulo> ListarArtNoVenc(Empleado empLog);
    }
}
