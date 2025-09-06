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


public partial class ABMCiudad : System.Web.UI.Page
{
        protected void Page_Load(object sender, EventArgs e)
    {
         if (!IsPostBack)
            this.LimpioFormulario();
        
    }
    private void ActivoBotonesBM()
    {
        btnModificar.Enabled = true;
        btnEliminar.Enabled = true;

        btnAgregar.Enabled = false;
        btnBuscar.Enabled = false;

        txtCodigo .Enabled = false;
    }
     private void ActivoBotonesA()
    {
        btnModificar.Enabled = false;
        btnEliminar.Enabled = false;

        btnAgregar.Enabled = true;
        btnBuscar.Enabled = false;

        txtCodigo.Enabled = false;
    }

    private void LimpioFormulario()
    {
        btnAgregar.Enabled = false;
        btnModificar.Enabled = false;
        btnEliminar.Enabled = false;

        btnBuscar.Enabled = true;

        txtCodigo.Enabled = true;

        txtCodigo.Text = "";
        txtNombre.Text = "";
        txtPais.Text = "";
    }
    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        this.LimpioFormulario();
    }
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        try
        {
            string _CodigoV = txtCodigo.Text.Trim();
            Ciudad _objeto = LogicaCiudad.BuscarCiudad(_CodigoV);

            //determino acciones
            if (_objeto == null)
            {
                //alta
                this.ActivoBotonesA();
                Session["CiudadABM"] = null;
            }
            else
            {
                this.ActivoBotonesBM();
                Session["CiudadABM"] = _objeto;

                txtNombre.Text = _objeto.Nombre;
                txtPais.Text = _objeto.Pais;
             }
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }
    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        try
        {
            Ciudad _unaC = new Ciudad(txtCodigo.Text.Trim().ToUpper(), txtNombre.Text, txtPais.Text.Trim());

             Logica.LogicaCiudad.AltaCiudad(_unaC);
             lblError.Text = "Alta con exito";

             //limpio pantalla
             this.LimpioFormulario();
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }
    protected void btnEliminar_Click(object sender, EventArgs e)
    {
        try
        {
            Ciudad _unaC = (Ciudad)Session["CiudadABM"];

            Logica.LogicaCiudad.EliminarCiudad(_unaC);

            lblError.Text = "Eliminacion con éxito";
            this.LimpioFormulario();
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }
    protected void btnModificar_Click(object sender, EventArgs e)
    {
        try
        {
            Ciudad _unaC = (Ciudad)Session["CiudadABM"];

            //modifico el objeto
            _unaC.Nombre = txtNombre.Text.Trim();
            _unaC.Pais = txtPais.Text.Trim();

            Logica.LogicaCiudad.ModificarCiudad(_unaC);
            lblError.Text = "Modificacion con éxito";
            this.LimpioFormulario();
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }
}
