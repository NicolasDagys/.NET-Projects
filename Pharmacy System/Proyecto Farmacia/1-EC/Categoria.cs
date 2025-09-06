using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace _1_EC
{
    public class Categoria
    {
        //atributos
        private string codCat;
        private string nomCat;

        //propiedades 
        public string CodCat
        {
            get { return codCat; }
            set { codCat = value;}
        }

        [DisplayName("Categoría")]
        public string NomCat
        {
            get { return nomCat; }
            set { nomCat = value; }
        }

        //constructor completo
        public Categoria (string pCodCat, string pNomCat)
        {
            CodCat = pCodCat;
            NomCat = pNomCat;
        }

        // constructor por defecto
        public Categoria() { }


        //validaciones
        public void Validar()
        {
            if (!Regex.IsMatch(this.CodCat, "[A-Za-z0-9]{6}"))
               throw new Exception("Error, el código debe tener 6 caracteres - letras o números");
            if (this.NomCat.Trim().Length > 30 || this.NomCat.Trim().Length <= 0)
                throw new Exception("Error en Nombre de la cateogría -entre 0 y 30 caracteres");
        }
    }
}
