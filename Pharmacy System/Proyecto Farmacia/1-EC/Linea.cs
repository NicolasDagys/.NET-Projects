using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace _1_EC
{
    public class Linea
    {
        //atributos
        private Articulo unArt;
        private int cant;

        //propiedades
        public Articulo UnArt
        {
            get { return unArt; }
            set { unArt = value; }               
        }

        public int Cant
        {
            get { return cant; }
            set { cant = value; }
        }

        //constructor completo
        public Linea(Articulo pUnArt, int pCant)
        {
            UnArt = pUnArt;
            Cant = pCant;
        }

        //constructor por defecto 
        public Linea() { }


        //validaciones
        public void Validar()
        {
            if (this.unArt == null)
                throw new Exception("Error  en artículo");
            if (this.Cant <= 0)
                throw new Exception("Cantidad debe ser mayor a 0");
        }
    }
}
