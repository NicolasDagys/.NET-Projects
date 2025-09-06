using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1_EC;

namespace _3_Persistencia.Interfaces
{
    public interface IPEstado
    {
        Estado Buscar(int pnumEst, Empleado empLog);
        List <Estado> ListarEstados(Empleado empLog);
    }
}
