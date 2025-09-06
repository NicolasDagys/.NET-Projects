using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades_Compartidas
{
    public class Cliente
    {
        //atributos
        Int64 _Pasaporte;
        string _Nombre;
        string _Contrasenia;
        Int64 _NumeroTarj;

        //propiedades
        public Int64 Pasaporte
        {
            get { return _Pasaporte; }
            set {
                try
                {
                    if (value.ToString().Trim().Length >=7 && value.ToString().Trim().Length <= 12)   //RNE
                        _Pasaporte = value;
                    else
                        throw new Exception("El Pasaporte del Cliente debe tener entre 7 y 12 numeros");
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                }
        }
        public string Nombre
        {
            get { return _Nombre; }
            set
            {
                if (value.Trim().Length > 0 && value.Trim().Length <= 30)
                    _Nombre = value;
                else
                    throw new Exception("El Nombre del Cliente debe tener hasta 30 caracteres");
            }
        }
        public string Contrasenia
        {
            get { return _Contrasenia; }
            set {
                if (value.Trim().Length >= 8 && value.Trim().Length <= 30)
                    _Contrasenia = value;
                else
                    throw new Exception("La contraseña del Cliente debe tener entre 8 y 30 caracteres");
            }
        }
        public Int64 NumeroTarj
        {
            get { return _NumeroTarj; }
            set
            {
                try
                {
                    if (value.ToString().Trim().Length == 16)
                         _NumeroTarj = value;
                    else
                        throw new Exception("La Tarjeta del Cliente debe tener 16 numeros");
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                
                }
        }
        
        //Constructor
        public Cliente(Int64 pPasaporte, string pNombre, string pContrasenia, Int64 pNumeroTarj) 
    {
        Pasaporte = pPasaporte;
        Nombre = pNombre;
        Contrasenia = pContrasenia;
        NumeroTarj = pNumeroTarj;
    }
        //operaciones
        public override string ToString()
        {
            return (Pasaporte + " - "+ Nombre);
        }
    }
}
