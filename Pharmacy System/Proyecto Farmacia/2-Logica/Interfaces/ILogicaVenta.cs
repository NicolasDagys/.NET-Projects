using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1_EC;
using _3_Persistencia;


namespace _2_Logica.Interfaces
{
    public interface ILogicaVenta
    {
        void Alta(Venta unaV, Empleado empLog);

        void CambioEstadoVenta(Venta unaV, Empleado empLog);

        List<Venta> ListarVentasTodas(Empleado empLog);
    }
}
