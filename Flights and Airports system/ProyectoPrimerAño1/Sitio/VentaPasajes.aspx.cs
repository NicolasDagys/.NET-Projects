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
public partial class VentaPasajes : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.LimpioFormulario();
        }
    }
    private void LimpioFormulario()
    {
        txtCodigoV.Text = "";
        txtPasaporte.Text = "";
        lblCodigoV.Text = "";
        lblPasaporte.Text = "";

        lblFechaV.Text = "";
        lblPrecioT.Text = "";
        lblNumeroV.Text = "";
        lblError.Text = "";

        Session["Vuelo"] = null;
        Session["Cliente"] = null;

    }
    protected void BtnLimpiar_Click(object sender, EventArgs e)
    {
        this.LimpioFormulario();
    }
    protected void btnBuscarV_Click(object sender, EventArgs e)
    {
        try
        {
            string _CodigoV = txtCodigoV.Text;
            Vuelo _unV = LogicaVuelo.Buscar(_CodigoV);

            //determino acciones
            if (_unV == null)
            {
                Session["Vuelo"] = null;
                lblCodigoV.Text = "No existe dicho Vuelo en el sistema";
            }
            else
            {
                Session["Vuelo"] = _unV;
                lblCodigoV.Text = _unV.ToString();
            }

        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }
    protected void btnBuscarP_Click(object sender, EventArgs e)
    {
        try
        {
            Int64 _Pasaporte = Convert.ToInt64(txtPasaporte.Text);
            Cliente _unC = LogicaCliente.BuscarCliente(_Pasaporte);

            //determino acciones
            if (_unC == null)
            {
                Session["Cliente"] = null;
                lblCodigoV.Text = "No existe dicho Cliente en el sistema";
            }
            else
            {
                Session["Cliente"] = _unC;
                lblPasaporte.Text = _unC.ToString();
            }

        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }
    protected void btnAgregarP_Click(object sender, EventArgs e)
    {
        try
        {

            Vuelo _unV =((Vuelo)Session["Vuelo"]);
            Cliente _unC= ((Cliente)Session["Cliente"]);
            

            //verifico seleccion de datos
            if (_unV == null)
                throw new Exception("Debe seleccionar un Vuelo");
            if (_unC == null)
                throw new Exception("Debe seleccionar un Cliente");

            Int64 PrecioT = _unV.Precio + _unV.unAPartida.ImpuestoOri + _unV.unALlegada.ImpuestoDes;
            DateTime FechaV = DateTime.Now;

            //creo el objeto
            Pasaje _unP = new Pasaje(0, FechaV,
                PrecioT, _unC, _unV);
            lblNumeroV.Text = _unP.NumVenta.ToString();
            lblPrecioT.Text = _unP.PrecioTot.ToString();
            lblFechaV.Text = _unP.FechaVenta.ToString();  

            LogicaPasaje.Agregar(_unP);
            this.LimpioFormulario();
            lblError.Text = "Alta con éxito.";
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }
}
