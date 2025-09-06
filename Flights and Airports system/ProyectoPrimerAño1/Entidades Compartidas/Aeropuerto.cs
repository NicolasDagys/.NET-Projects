using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades_Compartidas
{
    public class Aeropuerto
    {
        //atributos
        string _CodigoA;
        string _Nombre;
        string _Direccion;
        Int64 _ImpuestoOri;
        Int64 _ImpuestoDes;

        //artibuto de asociacion
        Ciudad _unaC; 

        //propiedades
        public string CodigoA
        {
            get { return _CodigoA; }
            set { 
                if(value.Trim().Length==3)
                _CodigoA = value; 
                else
                    throw new Exception("El Codigo del Aeropuerto debe tener 3 caracteres");
            }
        }
        public string Nombre
        {
            get { return _Nombre; }
            set {
                if (value.Trim().Length > 0 && value.Trim().Length <= 40)
                    _Nombre = value;
                else
                    throw new Exception("El Codigo del Aeropuerto debe tener hasta 40 caracteres");
            }
        }
        public string Direccion
        {
            get { return _Direccion; }
            set {
                if (value.Trim().Length > 0 && value.Trim().Length <= 40) 
                    _Direccion = value;
                else
                    throw new Exception("El Codigo del Aeropuerto debe tener hasta 20 caracteres");
            }
        }
        public Int64 ImpuestoOri
        {
            get { return _ImpuestoOri; }
            set {
                if (value >= 0) 
                    _ImpuestoOri = value;
                else
                    throw new Exception("El valor del impuesto de Origen no puede ser Negativo.");
            }
        }
        public Int64 ImpuestoDes
        {
            get { return _ImpuestoDes; }
            set
            {
                if (value >= 0 )
                    _ImpuestoDes = value;
                else
                    throw new Exception("El valor del impuesto de Destino no puede ser Negativo.");
            }
        }
            public Ciudad unaC
       {
           get { return _unaC; }
           set {
               if (value != null)
                   _unaC = value;
               else
                   throw new Exception("Debe saberse En que Ciudad esta el Aeropuerto");
           }
        }
        //Constructor
            public Aeropuerto(string pCodigoA, string pNombre, string pDireccion, Int64 pImpuestoOri, Int64 pImpuestoDes, Ciudad pUnaC)
        {
            CodigoA = pCodigoA;
            Nombre=pNombre;
            Direccion = pDireccion;
            ImpuestoOri = pImpuestoOri;
            ImpuestoDes = pImpuestoDes;
            unaC=pUnaC;
        }

        //operaciones
        public override string ToString()
        {
            return (CodigoA + " - " + Nombre + " - " + Direccion + " - " + ImpuestoOri + " - " + ImpuestoDes + " - " + unaC);
        }
    }
}
