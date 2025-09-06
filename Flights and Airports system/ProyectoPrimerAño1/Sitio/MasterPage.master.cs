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

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //verifico q el usuario tenga permiso de ingreso
            if(!(Session["Empleado"] is Empleado))
                Response.Redirect("Principal.aspx");
        }
        catch
        {
            Response.Redirect("Default.aspx");
        }
    }
}
