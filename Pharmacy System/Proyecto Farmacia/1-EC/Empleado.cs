using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_EC
{
    public class Empleado
    {
        //atributos
        private string nomEmp; 
        private string usuEmp;     
        private string passEmp;

        //Propiedades
        public string NomEmp
        {
            get { return nomEmp; }
            set { nomEmp = value; }
        }
        public string UsuEmp
        {
            get { return usuEmp; }
            set { usuEmp = value; }
        }

        public string PassEmp
        {
            get { return passEmp; }
            set { passEmp = value; }
        }

        //Constructor completo
        public Empleado (string pNomEmp, string pUsuEmp, string pPassEmp)
        {
            NomEmp = pNomEmp;
            UsuEmp = pUsuEmp;
            PassEmp = pPassEmp;
        }

        // constructor por defecto
        public Empleado() { }

        // validaciones
        public void Validar()
        {
            if (this.NomEmp.Trim().Length > 30 || this.NomEmp.Trim().Length <= 0)
                throw new Exception("Error en Nombre del Empleado -entre 0 y 30 caracteres");
            if (this.UsuEmp.Trim().Length > 15 || this.NomEmp.Trim().Length <= 0)
                throw new Exception("Error en Nombre de Usuario -entre 0 y 15 caracteres");
            if (this.PassEmp.Trim().Length > 10 || this.NomEmp.Trim().Length <= 0)
                throw new Exception("Error en Contraseña -entre 0 y 10 caracteres");
        }
    }
}
