using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1_EC;

namespace _3_Persistencia.Interfaces
{
    public interface IPCategoria
    {
        void Alta(Categoria unaCategoria, Empleado empLog );
        void Baja(Categoria unaCategoria, Empleado empLog);
        void Modificar(Categoria unaCategoria, Empleado empLog);
        Categoria BuscarActiva (string unCodigo, Empleado empLog);
        List<Categoria> ListarActivas(Empleado empLog);
    }
}
