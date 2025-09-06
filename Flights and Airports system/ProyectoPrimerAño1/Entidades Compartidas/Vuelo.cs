using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades_Compartidas
{
    public class Vuelo
    {
        //atributos
        string _CodigoV;
        DateTime _FechaPartida;
        DateTime _FechaLlegada;
        Int64 _Precio;
        int _CantAsientos;

        //atributos de asociacion
        Aeropuerto _unAPartida;
        Aeropuerto _unALlegada;

        //propiedades
        public string CodigoV
        {
            get { return _CodigoV; }
            set {
                if (value.Trim().Length == 15)
                    _CodigoV = value;
                else
                    throw new Exception("El Codigo de Vuelo debe tener 15 caracteres. Formato yyyyMMddHHmmAER");
            }
        }
        public DateTime FechaPartida
        {
            get { return _FechaPartida; }
            set { _FechaPartida = value; }//validaciones de fecha van en logica (reglas de negocio)
        }
        public DateTime FechaLlegada
        {
            get { return _FechaLlegada; }
            set {
                if (value > FechaPartida)
                    _FechaLlegada = value;
                else
                    throw new Exception("La fecha de llegada del Vuelo no puede ser anterior a la fecha de Partida");
                 }
        }
        public Int64 Precio
        {
            get { return _Precio; }
            set {
                if (value > 0)
                    _Precio = value;
                else
                    throw new Exception("El precio del Vuelo debe ser un numero positivo");
            }
        }
        public int CantAsientos
        {
            get { return _CantAsientos; }
            set {
                if (value >= 100 && value <= 300)
                    _CantAsientos = value;
                else
                    throw new Exception("La Cantidad de Asientos para el vuelo debe de estar entre 100 y 300");
            }
        }
        public Aeropuerto unAPartida
        {
            get { return _unAPartida; }
            set
            {
                if (value != null)
                    _unAPartida = value;
                else
                    throw new Exception("El Vuelo debe tener un Aeropuerto de Origen");
            }
        }
        public Aeropuerto unALlegada
        {
            get { return _unALlegada; }
            set
            {
                if (value != null)
                    _unALlegada = value;
                else
                    throw new Exception("El Vuelo debe tener un Aeropuerto de Destino");
            }
        }
       
        //Constructor
        public Vuelo(string pCodigoV, DateTime pFechaPartida, DateTime pFechaLlegada, Int64 pPrecio, int pCantAsientos, Aeropuerto pUnAP, Aeropuerto pUnAL)
        {
            CodigoV = pCodigoV;
            FechaPartida = pFechaPartida;
            FechaLlegada = pFechaLlegada;
            Precio = pPrecio;
            CantAsientos = pCantAsientos;
            unAPartida = pUnAP;
            unALlegada = pUnAL;
        }
        //operaciones
        public override string ToString()
        {
            return (CodigoV + " - " + FechaPartida + " - " + FechaLlegada + " - " + Precio + " - " + CantAsientos + " - " + unAPartida + " - " + unALlegada);
        }
    }
}
