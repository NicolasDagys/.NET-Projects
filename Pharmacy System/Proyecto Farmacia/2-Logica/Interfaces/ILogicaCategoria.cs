using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1_EC;
using _3_Persistencia;

namespace _2_Logica.Interfaces
{
    public interface ILogicaCategoria
    {
        void Alta(Categoria unaC, Empleado empLog);

        void Baja(Categoria unaC, Empleado empLog);

        void Modificar(Categoria unaC, Empleado empLog);

        Categoria BuscarActiva(string CodCat, Empleado empLog);

        List<Categoria> ListarActivas(Empleado empLog);
    }
}
