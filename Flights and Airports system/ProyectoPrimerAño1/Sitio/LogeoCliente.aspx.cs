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

public partial class LogeoCliente : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //por defecto cada vez q vuelvo o paso por la defaul, determino q se realizo un Deslogueo
        //Session["Cliente"] = null;
    }
    protected void BtnLogeo_Click(object sender, EventArgs e)
    {
        try
        {
            //verifico usuario
            Cliente unC = LogicaCliente.Logueo(Convert.ToInt64(TxtPasaporte.Text.Trim()), TxtContra.Text.Trim());
            if (unC != null)
            {
                //si llego aca es pq es valido
                Session["Cliente"] = unC;
                Response.Redirect("HistoricoCompras2.aspx");
            }
            else
                LblError.Text = "Datos Incorrectos";
        }
        catch (Exception ex)
        {
            LblError.Text = ex.Message;
        }
    }
}