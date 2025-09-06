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

public partial class AltaVuelo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.LimpioFormulario();

            Session["ListaAeropuertosP"]=Logica.LogicaAeropuerto.ListarAeropuertos();
            DdlAeropuertosP.DataSource = Session["ListaAeropuertosP"];
            DdlAeropuertosP.DataTextField = "CodigoA";
            DdlAeropuertosP.DataValueField = "CodigoA";
            DdlAeropuertosP.DataBind();

            Session["ListaAeropuertosL"] = Logica.LogicaAeropuerto.ListarAeropuertos();
            DdlAeropuertoL.DataSource = Session["ListaAeropuertosL"];
            DdlAeropuertoL.DataTextField = "CodigoA";
            DdlAeropuertoL.DataValueField = "CodigoA";
            DdlAeropuertoL.DataBind();

            //Session["ListaAeropuertos"] = Logica.LogicaAeropuerto.ListarAeropuertos();
            //DdlAeropuertosP.DataSource = Session["ListaAeropuertos"];
            //DdlAeropuertoL.DataSource = Session["ListaAeropuertos"];
            //DdlAeropuertosP.DataTextField = "Nombre";
            //DdlAeropuertoL.DataTextField = "Nombre"; 
            //DdlAeropuertosP.DataBind();
            //DdlAeropuertoL.DataBind();

        }
    }
    private void LimpioFormulario()
    {
        txtFechaP.Text = "";
        txtFechaL.Text = "";
        txtPrecioV.Text="";
        txtCantA.Text = "";
        lblCodigoV.Text = "";
        lblError.Text = "";

  
    }
    protected void BtnLimpiar_Click(object sender, EventArgs e)
    {
        this.LimpioFormulario();
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        try
        {

        Aeropuerto _unAP = ((List<Aeropuerto>)Session["ListaAeropuertosP"])[DdlAeropuertosP.SelectedIndex];
        Aeropuerto _unAL = ((List<Aeropuerto>)Session["ListaAeropuertosL"])[DdlAeropuertoL.SelectedIndex];

        //verifico seleccion de datos
        if (_unAP == null)
            throw new Exception("Debe seleccionar un Aeropuerto de Partida");
        else if (_unAL == null)
            throw new Exception("Debe seleccionar un Aeropuerto de Llegada");
        else if (_unAP.CodigoA == _unAL.CodigoA)
            throw new Exception("El Aeropuerto de Partida y el Aeropuerto de Llegada no pueden ser el mismo");

            Vuelo _unV = null;

            //_unV.CodigoV= (txtFechaP.Text.ToString()+""+DdlAeropuertosP.SelectedIndex.ToString());
            //string CodigoV = Convert.ToInt64(txtFechaP.Text.Trim()) + _unAP.CodigoA.ToString();

            
            DateTime FechaPartida = Convert.ToDateTime(txtFechaP.Text);
            string CodigoV = FechaPartida.ToString("yyyyMMddHHmm") + _unAP.CodigoA.ToString();

            //creo el objeto

            _unV = new Vuelo(CodigoV, Convert.ToDateTime(txtFechaP.Text), Convert.ToDateTime(txtFechaL.Text),
                Convert.ToInt32(txtPrecioV.Text),Convert.ToInt32(txtCantA.Text),_unAP,_unAL);
            LogicaVuelo.Agregar(_unV);
            this.LimpioFormulario();
            lblError.Text = "Alta con éxito.";
            
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }
}