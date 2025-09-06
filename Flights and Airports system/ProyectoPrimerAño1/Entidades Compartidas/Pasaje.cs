using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades_Compartidas
{
    public class Pasaje
    {
        //atributos
        int _NumVenta;
        DateTime _FechaVenta;
        Int64 _PrecioTot;   //tributo calculado

        //atributos de asociacion
        Vuelo _unV;
        Cliente _unC;

        //propiedades
        public int NumVenta //prop de lectura
        {
            get { return _NumVenta; }
            set { _NumVenta = value; }
        }
        public DateTime FechaVenta
        {
            get { return _FechaVenta; }
            set
            {_FechaVenta = value;} //no tiene validacion porque se saca automaticamente del sistema. el usuario no introduce una fecha
        }
        
        public Int64 PrecioTot
        {
            get { return _PrecioTot; } //atributo calculado
            set {
                if (value>0)
                _PrecioTot = value; }
        }
        public Vuelo unV
        {
            get { return _unV; }
            set
            {
                if (value != null)
                    _unV = value;
                else
                    throw new Exception("El Pasaje debe tener un Vuelo asociado");
            }
        }
        public Cliente unC
        {
            get { return _unC; }
            set
            {
                if (value != null)
                    _unC = value;
                else
                    throw new Exception("El Pasaje bebe tener un Cliente asociado");
            }
        }
        public string Desplegar//propiedad solo lectura para desplegar info en la grilla
        {
            get { return this.ToString(); }
        }
        //Constructor
        public Pasaje( int pNumVenta, DateTime pFechaVenta, Int64 pPrecioTot, Cliente pUnC, Vuelo pUnV)
        {
            NumVenta = pNumVenta; 
            FechaVenta = pFechaVenta;
            NumVenta = pNumVenta;
            PrecioTot = pPrecioTot;
            unC = pUnC;
            unV = pUnV;
        }
        
        //operaciones
        public override string ToString()
        {
            return (NumVenta + " - " + FechaVenta + " - " + PrecioTot + " - " + unC + " - " + unV);
        }
    }
}
