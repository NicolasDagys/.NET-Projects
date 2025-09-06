using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1_EC;
using _3_Persistencia;

namespace _2_Logica.Interfaces
{
    public interface ILogicaEstado
    {
        Estado Buscar(int NumEst, Empleado empLog);
        List<Estado> ListarEstados(Empleado empLog);
    }
}
