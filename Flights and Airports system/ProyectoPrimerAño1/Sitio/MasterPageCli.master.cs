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

public partial class MasterPageCli : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            if (!(Session["Cliente"] is Cliente))
               
                Response.Redirect("Default.aspx");
        } 
        catch
        {
            Response.Redirect("Default.aspx");
        }
    }
}
