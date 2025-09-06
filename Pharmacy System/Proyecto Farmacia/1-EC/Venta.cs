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
    public class Venta
    {
        // atributos
        private int numVent;
        private DateTime fechaVent;
        private string dirEnvIoVent;
        private int totalVent;

        private Cliente unCliente;
        private List<Asignacion> listaAsignaciones;
        private List<Linea> listaLineas;


        //propiedades

        [DisplayName("Numero")]
        public int NumVent
        {
            get { return numVent; }
            set { numVent = value; }
        }

        [DataType(DataType.Date)]
        public DateTime FechaVent
        {
            get { return fechaVent; }
            set { fechaVent = value; }
        }


        [DisplayName("Dirección Envío")]
        public string DirEnvIoVent
        {
            get { return dirEnvIoVent; }
            set { dirEnvIoVent = value; }
        }


        [DisplayName("Costo Total")]
        public int TotalVent
        {
            get { return totalVent; }
            set { totalVent = value; }
        }


        [DisplayName("Cliente")]
        public Cliente UnCliente
        {
            get { return unCliente; }
            set { unCliente = value; }
        }

        public List<Asignacion> ListaAsignaciones
        {
            get { return listaAsignaciones; }
            set { listaAsignaciones = value; }
        }

        public List<Linea> ListaLineas
        {
            get { return listaLineas; }
            set
            {
                listaLineas = value;
            }
        }


        //constructor completo
        public Venta(int pNumVent, DateTime pFechaVent, string pDirEnvIoVent, int pTotalVent, Cliente pUnCliente, List<Asignacion> pListaAsign, List<Linea> pListaLineas)
        {
            NumVent = pNumVent;
            FechaVent = pFechaVent;
            DirEnvIoVent = pDirEnvIoVent;
            TotalVent = pTotalVent;
            UnCliente = pUnCliente;
            ListaAsignaciones = pListaAsign;
            ListaLineas = pListaLineas;
        }

        //constructor por defecto
        public Venta() { }

        //validaciones
        public void Validar()
        {
            // Validación de cliente
            if (this.UnCliente == null)
                throw new Exception("Error: El cliente es obligatorio");

            // Validación de estado de la venta
            if (this.ListaAsignaciones == null || this.ListaAsignaciones.Count == 0)
                throw new Exception("Error: La venta debe tener al menos un estado");

            // Validación de líneas de la venta
            if (this.ListaLineas == null || this.ListaLineas.Count == 0)
                throw new Exception("Error: La venta debe tener al menos una línea de producto");

            // Validación del monto total de la venta
            if (this.TotalVent <= 0)
                throw new Exception("Error: El monto total de la venta debe ser mayor que 0");
        }
    }
}
