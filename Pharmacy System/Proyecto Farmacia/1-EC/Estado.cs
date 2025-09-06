using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace _1_EC
{
    public class Estado
    {
        //atributos
        private int numEst;
        private string nomEst;

        // propiedades
        public int NumEst
        {
            get { return numEst; }
            set { numEst = value; }
        }

        public string NomEst
        {
            get { return nomEst; }
            set { nomEst = value; }
        }

        //constructor 
        public Estado (int pNumEst, string pNomEst)
        {
            NumEst = pNumEst;
            NomEst = pNomEst;
        }

        //constructor por defecto
        public Estado() { }


        //validaciones
        public void Validar()
        {
            if (this.NumEst < 0)
                throw new Exception("El código de Estado debe ser un número positivo");         
        }
    }
}
