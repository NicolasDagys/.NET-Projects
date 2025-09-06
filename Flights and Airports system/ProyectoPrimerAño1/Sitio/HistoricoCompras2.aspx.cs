using System;
using System.Collections;
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

public partial class HistoricoCompras : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            Cliente _unC = ((Cliente)Session["Cliente"]);
            Session["Pasajes"] = LogicaPasaje.ListadoPasajeXCliente(_unC);

            GVPasaje.DataSource = (List<Pasaje>)Session["Pasajes"];
            GVPasaje.DataBind();
        }
    }
    protected void GVVuelos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //Aeropuerto de Partida (Ciudad y pais),vuelo y  Aeropuerto Llegada (Ciudad y Pais)
            Pasaje _unP = ((List<Pasaje>)Session["Pasajes"])[GVPasaje.SelectedIndex];

            if (_unP != null)
            {
                lblAeropuertoP.Text = _unP.unV.unAPartida.ToString();
                lblAeropuertoL.Text = _unP.unV.unALlegada.ToString();
                lblViaje.Text = _unP.unV.ToString();
            }
            else
                throw new Exception(" No se han encontrado Pasajes");
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }
}