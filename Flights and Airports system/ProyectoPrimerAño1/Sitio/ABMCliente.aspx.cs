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

public partial class ABMCliente : System.Web.UI.Page
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

        txtPasaporte.Enabled = false;
    }

    private void ActivoBotonesA()
    {
        btnModificar.Enabled = false;
        btnEliminar.Enabled = false;

        btnAgregar.Enabled = true;
        btnBuscar.Enabled = false;

        txtPasaporte.Enabled = false;
    }

    private void LimpioFormulario()
    {
        btnAgregar.Enabled = false;
        btnModificar.Enabled = false;
        btnEliminar.Enabled = false;

        btnBuscar.Enabled = true;

        txtPasaporte.Enabled = true;

        txtPasaporte.Text = "";
        txtNombre.Text = "";
        txtContrasenia.Text = "";
        txtNumTarjeta.Text = "";

        lblError.Text = "";
    }
    protected void BtnLimpiar_Click(object sender, EventArgs e)
    {
        this.LimpioFormulario();
    }
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        try
        {
            Int64 Pasaporte = Convert.ToInt64(txtPasaporte.Text);
            Cliente _objeto = Logica.LogicaCliente.BuscarCliente(Pasaporte);

            //determino acciones
            if (_objeto == null)
            {
                //alta
                this.ActivoBotonesA();
                Session["ClienteABM"] = null;
            }
            else
            {
                this.ActivoBotonesBM();
                Session["ClienteABM"] = _objeto;

                txtNombre.Text = _objeto.Nombre;
                //EL NUMERO DE TARJETA DEBERIA APARECER?? ME IMAGINO QUE LA CONTRASEÑA NO
                txtNumTarjeta.Text = _objeto.NumeroTarj.ToString();
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
            Cliente _unC = new Cliente(Convert.ToInt64(txtPasaporte.Text), txtNombre.Text.Trim(),txtContrasenia.Text.Trim(),Convert.ToInt64(txtNumTarjeta.Text));

            Logica.LogicaCliente.AltaCliente(_unC);
            lblError.Text = "Alta con exito";

            //limpio pantalla
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
            Cliente _unC = (Cliente)Session["ClienteABM"];

            //modifico el objeto
            _unC.Nombre = txtNombre.Text.Trim();
            _unC.Contrasenia = txtContrasenia.Text.Trim();
            _unC.NumeroTarj=Convert.ToInt64(txtNumTarjeta.Text);

            Logica.LogicaCliente.ModificarCliente(_unC);
            lblError.Text = "Modificacion con éxito";
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
            Cliente _unC = (Cliente)Session["ClienteABM"];

            Logica.LogicaCliente.EliminarCliente(_unC);

            lblError.Text = "Eliminacion con éxito";
            this.LimpioFormulario();
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }
}