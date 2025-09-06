using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1_EC;

namespace _3_Persistencia.Interfaces
{
    public interface IPVenta
    {
        void Alta (Venta unaVenta, Empleado empLog);
        void CambioEstadoVenta(Venta unaVenta, Empleado empLog);
        List<Venta> ListarVentasTodas(Empleado empLog);
    }
}
