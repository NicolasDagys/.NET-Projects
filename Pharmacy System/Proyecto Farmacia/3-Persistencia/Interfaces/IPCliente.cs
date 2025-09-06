using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1_EC;

namespace _3_Persistencia.Interfaces
{
    public interface IPCliente
    {
        void Alta(Cliente unCliente, Empleado empLog);
        void Modificar(Cliente unCliente, Empleado empLog);
        Cliente Buscar(string pCiCli, Empleado empLog);
        List<Cliente> Listar(Empleado empLog);
    }
}
