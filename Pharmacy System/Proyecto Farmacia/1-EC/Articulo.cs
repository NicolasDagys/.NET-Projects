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
    public class Articulo
    {
        // Atributos
        private string codArt;
        private string nomArt;
        private int precioArt;
        private DateTime fechaVtoArt;
        private string tipoPArt;
        private int tamArt;
        private Categoria unaCat;

        // Propiedades  

        [DisplayName("Codigo")]
        public string CodArt
        {
            get { return codArt; }
            set { codArt = value; }
        }

        [DisplayName("Nombre")]
        public string NomArt
        {
            get { return nomArt; }
            set { nomArt = value; }
        }

        [DisplayName("Precio")]
        public int PrecioArt
        {
            get { return precioArt; }
            set { precioArt = value; }
        }

        [DisplayName("Vencimiento")]
        [DataType(DataType.Date)]
        public DateTime FechaVtoArt
        {
            get { return fechaVtoArt; }
            set { fechaVtoArt = value; }
        }

        [DisplayName("Presentación")]
        public string TipoPArt
        {
            get { return tipoPArt; }
            set { tipoPArt = value; }
        }

        [DisplayName("Tamaño")]
        public int TamArt
        {
            get { return tamArt; }
            set { tamArt = value; }
        }

        [DisplayName("Categoría")]
        public Categoria UnaCat
        {
            get { return unaCat; }
            set { unaCat = value; }
        }

        // Constructor completo
        public Articulo(string pCodArt, string pNomArt, int pPrecioArt, DateTime pFechaVtoArt, string pTipoPArt, int pTamArt, Categoria pUnaCat)
        {
            CodArt = pCodArt;
            NomArt = pNomArt;
            PrecioArt = pPrecioArt;
            FechaVtoArt = pFechaVtoArt;
            TipoPArt = pTipoPArt;
            TamArt = pTamArt;
            UnaCat = pUnaCat;
        }

        // Constructor por defecto
        public Articulo() { }

        // Validaciones generales de la entidad (sin la validación de la fecha)
        public void Validar()
        {
            if (!Regex.IsMatch(this.CodArt, "[A-Za-z0-9]{10}"))
                throw new Exception("Error, el código debe tener 10 caracteres - letras o números");

            if (this.NomArt.Trim().Length > 30 || this.NomArt.Trim().Length <= 0)
                throw new Exception("Error en Nombre del artículo - debe tener entre 1 y 30 caracteres");

            if (this.PrecioArt < 0)
                throw new Exception("El precio del artículo no puede ser negativo");

            if (!(this.TipoPArt.Trim().ToLower() == "unidad" ||
                  this.TipoPArt.Trim().ToLower() == "blister" ||
                  this.TipoPArt.Trim().ToLower() == "sobre" ||
                  this.TipoPArt.Trim().ToLower() == "frasco"))
                throw new Exception("Error en tipo de presentación de artículos - Posibilidades: 'unidad', 'blister', 'sobre', 'frasco'");

            if (this.TamArt < 0)
                throw new Exception("El tamaño del artículo no puede ser negativo");

            if (this.unaCat == null)
                throw new Exception("Error: Categoría no asignada");
        }
    }
}