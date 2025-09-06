using System;
using System.Configuration;
using System.Data;
//using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
//using System.Xml.Linq;

using Entidades_Compartidas;
using Logica;

public partial class LogeoEmpleado : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //por defecto cada vez q vuelvo o paso por la defaul, determino q se realizo un Deslogueo
        //Session["Empleado"] = null;
    }
    protected void BtnLogeo_Click(object sender, EventArgs e)
    {
        try
        {
            //verifico usuario
            Empleado unE= LogicaEmpleado.Logueo(TxtUsuario.Text.Trim(), TxtContra.Text.Trim());
            if (unE != null)
            {
                //si llego aca es pq es valido
                Session["Empleado"] = unE;
                Response.Redirect("Principal.aspx");
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