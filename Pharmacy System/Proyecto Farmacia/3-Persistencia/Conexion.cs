using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3_Persistencia
{
    internal class Conexion
    {
        internal static string Cnn(string pUsuEmp = "", string pPassEmp = "")
        {
            if (pUsuEmp == "" || pPassEmp == "")
                return "Data Source=NICOD\\SQLEXPRESS; Initial Catalog = Farmacia; Integrated Security = true";
            else
                return "Data Source=NICOD\\SQLEXPRESS; Initial Catalog = Farmacia; User="
                        + pUsuEmp + "; Password='" + pPassEmp + "'";

            /*  internal static string Cnn (_1_EC.Empleado pEmp = null)
            {
                if (pEmp == null)
                    return "Data Source=ASUS-CAMILO\\SQLEXPRESS; Initial Catalog = Farmacia; Integrated Security = true";
                else
                    return "Data Source=ASUS-CAMILO\\SQLEXPRESS; Initial Catalog = Farmacia; User="
                            + pEmp.UsuEmp + "; Password='" + pEmp.PassEmp + "'";
            } */
        }
    }
}
