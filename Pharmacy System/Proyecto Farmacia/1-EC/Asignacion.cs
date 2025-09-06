using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace _1_EC
{
    public class Asignacion
    {
        //atributos
        Estado unEstado;
        DateTime fyhEst;

        //propiedades
        public Estado UnEstado
        {
            get { return unEstado; }
            set { unEstado = value; }
        }
        public DateTime FyHEst
        {
            get { return fyhEst; }
            set { fyhEst = value; }
        }

        //constructor completo
        public Asignacion(Estado pUnEstado, DateTime pFyHEst)
        {
            UnEstado = pUnEstado;
            FyHEst = pFyHEst;
        }

        //constructor por defecto 
        public Asignacion() { }

        //Validaciones
        public void Validar()
        {
            if (this.UnEstado == null)
                throw new Exception("Error: El estado no puede ser nulo");
        }
    }
}
