using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;

using Entidades_Compartidas;
using Logica;



public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //utilizo default como pagina de deslogeo

            Session["Empleado"] = null;
            Session["Cliente"] = null;

            Session["ListaAeropuertos"] = Logica.LogicaAeropuerto.ListarAeropuertos();

            DDLAeropuertos.DataSource = Session["ListaAeropuertos"];
            DDLAeropuertos.DataTextField = "Nombre";
            DDLAeropuertos.DataValueField = "Nombre";
            DDLAeropuertos.DataBind();

            
        }
    }
    protected void DDLAeropuertos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Aeropuerto _unA= ((List<Aeropuerto>)Session["ListaAeropuertos"])[DDLAeropuertos.SelectedIndex];

            //verifico seleccion de datos
            if (_unA == null)
                throw new Exception("Debe seleccionar el Aeropuerto");
            else
            {
                if (LogicaVuelo.ListaVuelosPartida(_unA).Count != 0)
                Session["Partidas"] = Logica.LogicaVuelo.ListaVuelosPartida(_unA);
                
                GVPartidas.DataSource = Session["Partidas"];
                GVPartidas.DataBind();

                Session["Llegadas"] = Logica.LogicaVuelo.ListaVuelosLlegada(_unA);
                GVLlegadas.DataSource = Session["Llegadas"];
                GVLlegadas.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }
}