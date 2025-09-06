using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1_EC;
using _3_Persistencia;

namespace _2_Logica.Interfaces
{
    public interface ILogicaCliente
    {
        void Alta(Cliente unC, Empleado empLog);

        void Modificar(Cliente unC, Empleado empLog);

        Cliente Buscar(string CiCli, Empleado empLog);

        List<Cliente> Listar(Empleado empLog);
        
    }
}
