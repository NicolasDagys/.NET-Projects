using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades_Compartidas
{
    public class Empleado
    {
        //atributos
        string _Usuario;
        string _NombreComp;
        string _Contrasenia;
        string _Cargo;

        //propiedades
        public string Usuario
        {
            get { return _Usuario; }
            set {
                if (value.Trim().Length > 4 && value.Trim().Length <= 30)// minimo 4. para que no pongan usuarios de una o 3 letras
                    _Usuario = value;
                else
                    throw new Exception("El Usuario debe tener entre 4 caracteres y 30");
            }
        }
        public string NombreComp
        {
            get { return _NombreComp; }
            set {
                if (value.Trim().Length > 7 && value.Trim().Length <= 30)
                    _NombreComp = value;
                else
                    throw new Exception("El Nombre completo debe tener entre 8 caracteres y 30"); 
            }
        }
        public string Contrasenia
        {
            get { return _Contrasenia; }
            set {
                if (value.Trim().Length > 7 && value.Trim().Length <= 20)
                    _Contrasenia = value; 
                else
                    throw new Exception("La Contraseña debe tener entre 8 caracteres y 30");
                
            }
        }
        public string Cargo
        {
            get { return _Cargo; }
            set {
                if (value.Trim().ToLower() == "gerente" || value.Trim().ToLower() == "vendedor" || value.Trim().ToLower() == "admin")
                    _Cargo = value;
                else
                    throw new Exception("El cargo debe ser Gerente, Vendedor o Admin");
                }
        }
        //Constructor
        public Empleado(string pUsuario, string pNombreComp, string pContrasenia, string pCargo)
        {
            Usuario = pUsuario;
            NombreComp = pNombreComp;
            Contrasenia = pContrasenia;
            Cargo = pCargo;
        }
        //operaciones
        public override string ToString()
        {
            return this.Usuario;
        }
    }
}
