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

public partial class ABMAeropuerto : System.Web.UI.Page
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
        btnBuscarA.Enabled = false;
        btnBuscarC.Enabled = true;

        txtCodigoA.Enabled = false;
    }

    private void ActivoBotonesA()
    {
        btnModificar.Enabled = false;
        btnEliminar.Enabled = false;

        btnAgregar.Enabled = true;
        btnBuscarA.Enabled = false;

        txtCodigoA.Enabled = false;
        btnBuscarC.Enabled = true;
    }

    private void LimpioFormulario()
    {
        btnAgregar.Enabled = false;
        btnModificar.Enabled = false;
        btnEliminar.Enabled = false;

        btnBuscarA.Enabled = true;
        btnBuscarC.Enabled = true;

        txtCodigoA.Enabled = true;

        txtCodigoA.Text = "";
        txtCodigoC.Text = "";
        txtDireccion.Text = "";
        
        txtNombreA.Text = "";
        txtDireccion.Text = "";
        txtImpuestoO.Text = "";
        txtImpuestoD.Text = "";

        lblCiudad.Text="";

    }
    protected void BtnLimpiar_Click(object sender, EventArgs e)
    {
        this.LimpioFormulario();
    }
    protected void btnBuscarA_Click(object sender, EventArgs e)
    {
        try
        {

            //busco Aeropuerto
            string _CodigoA = txtCodigoA.Text;
            Aeropuerto _objeto = Logica.LogicaAeropuerto.BuscarAeropuerto(_CodigoA);

            //determino acciones
            if (_objeto == null)
            {
                //alta
                this.ActivoBotonesA();
                Session["Aeropuerto"] = _objeto;
                Session["Ciudad"] = null;
            }
            else
            {
                this.ActivoBotonesBM();

                Session["Aeropuerto"] = _objeto;
                Session["Ciudad"] = _objeto.unaC;

                txtCodigoA.Text = _objeto.CodigoA;

                lblCiudad.Text = _objeto.unaC.ToString();

                txtNombreA.Text = _objeto.Nombre;
                txtDireccion.Text= _objeto.Direccion;
                txtImpuestoO.Text= _objeto.ImpuestoOri.ToString();
                txtImpuestoD.Text= _objeto.ImpuestoDes.ToString();

            }

        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }
    protected void btnBuscarC_Click(object sender, EventArgs e)
    {
        try
        {
            //busco 
            string _CodigoC = txtCodigoC.Text; ;
            Ciudad _objeto = Logica.LogicaCiudad.BuscarCiudad(_CodigoC);

            //determino acciones
            if (_objeto == null)
            {
                Session["Ciudad"] = null;
                lblCiudad.Text = "No existe Ciudad con ese Codigo en el sistema";
            }
            else
            {
                Session["Ciudad"] = _objeto;
                lblCiudad.Text = _objeto.ToString();
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
            Ciudad _unC = (Ciudad)Session["Ciudad"];

            //verifico q tenga una Ciudad
            if (_unC == null)
                throw new Exception("Debe Seleccionar una Ciudad");


            //creo el objeto
            Aeropuerto unApto = new Aeropuerto(txtCodigoA.Text.ToUpper(),txtNombreA.Text,txtDireccion.Text,Convert.ToInt64(txtImpuestoO.Text),Convert.ToInt64(txtImpuestoD.Text),_unC);


            //trato de agregar
            Logica.LogicaAeropuerto.AltaAeropuerto(unApto);
            lblError.Text = "Alta con éxito";
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
            Aeropuerto _unA = (Aeropuerto)Session["Aeropuerto"];
            Ciudad _unaC = (Ciudad)Session["Ciudad"];

            //verifico q tenga una Ciudad
            if (_unaC == null)
                throw new Exception("Debe Seleccionar una Ciudad");


            //modifico el objeto
            _unA.Nombre = txtNombreA.Text;
            _unA.Direccion = txtDireccion.Text;
            _unA.ImpuestoOri = Convert.ToInt64(txtImpuestoO.Text);
            _unA.ImpuestoDes = Convert.ToInt64(txtImpuestoD.Text);
            _unA.unaC = _unaC;

            //trato de modificar
            Logica.LogicaAeropuerto.ModificarAeropuerto(_unA);
            lblError.Text = "Modificacion con éxito";
            this.LimpioFormulario();
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }

    protected void BtnEliminar_Click(object sender, EventArgs e)
    {
        try
        {

            Aeropuerto _unA = (Aeropuerto)Session["Aeropuerto"];

            //trato de eliminar
            Logica.LogicaAeropuerto.EliminarAeropuerto(_unA);
            lblError.Text = "Eliminacion con éxito";
            this.LimpioFormulario();
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }
}