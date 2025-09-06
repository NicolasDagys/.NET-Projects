using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace _1_EC
{
    public class Cliente
    {
        //atributos
        private string ciCli;
        private string nomCli;
        private string numTarjCli;
        private string telCli;

        // propiedades
        public string CiCli
        {
            get { return ciCli; }
            set { ciCli = value; }
        }

        public string NomCli
        {
            get { return nomCli; }
            set { nomCli = value; }
        }

        public string NumTarjCli
        {
            get { return numTarjCli; }
            set { numTarjCli = value; }
        }

        public string TelCli
        {
            get { return telCli; }
            set { telCli = value; }
        }
        
        //constructor completo
        public Cliente (string pCiCli, string pNomCli, string pNumTarjCli, string pTelCli)
        {
            CiCli = pCiCli;
            NomCli = pNomCli;
            NumTarjCli = pNumTarjCli;
            TelCli = pTelCli;
        }

        //constructor por defecto
        public Cliente() { }

        //validaciones
        public void Validar()
        {
            if (!Regex.IsMatch(this.CiCli, "[0-6]{0,1}[0-9]{6}-[0-9]"))
                throw new Exception("Error - número de cédula inválido");

            if (!Regex.IsMatch(this.NomCli, "[a-zA-Z]{3,20}"))
                throw new Exception("Error - El nombre del Paciente debe tener entre 3 y 20 caracteres");

            if (!Regex.IsMatch(this.NumTarjCli, "[0-9]{16}"))
                throw new Exception("Error - Formato de tarjeta de crédito - 16 números");

            if (!Regex.IsMatch(this.TelCli, "[0-9]{4,9}"))
                throw new Exception("Error - teléfono entre 4 y 9 números");
        }
    }
}
