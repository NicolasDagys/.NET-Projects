using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades_Compartidas
{
    public class Ciudad
    {
        //atributos
        string _CodigoC;
        string _Nombre;
        string _Pais;

        //propiedades
        public string CodigoC
        {
            get { return _CodigoC; }
            set {
                if (value.Trim().Length==6)
                    _CodigoC = value;
                else
                    throw new Exception("El Codigo de la Ciudad debe tener 6 caracteres");
            }
        }

            public string Nombre
        {
            get { return _Nombre; }
            set {
                if (value.Trim().Length>0 && value.Trim().Length<=20) 
                    _Nombre = value;
                else
                    throw new Exception("El Nombre de la Ciudad debe tener hasta 20 caracteres");
            }
        }
            public string Pais
            {
                get { return _Pais; }
                set { 
                    if(value.Trim().Length>0 && value.Trim().Length<=20)
                        _Pais = value;
                    else
                        throw new Exception("El Pais de la Ciudad debe tener hasta 20 caracteres");
                    }
            }

        //Constructor
            public Ciudad(string pCodigoC, string pNombre, string pPais)
            { 
                CodigoC=pCodigoC;
                Nombre=pNombre;
                Pais = pPais;
            }

        //operaciones
            public override string ToString()
            {
                return (CodigoC + " - " + Nombre + " - " + Pais);


            }


    }
}
