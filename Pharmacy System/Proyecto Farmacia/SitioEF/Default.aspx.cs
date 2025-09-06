using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ModeloEF;

public partial class _Default : System.Web.UI.Page
{
    //variables globales
    FarmaciaEntities Micontexto = null;
    List<Articulo> _todosLosArticulos = null;
    List<Categoria> _todasLasCategorias = null;
    Articulo unA = null; //art seleccionado en la grilla

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack) //primer ingreso a la página
            {
                //verifico si tengo que armar nuevo contexto
                ArmadoContexto();

                //cargo todos los articulos
                Session["todosLosArticulos"] = _todosLosArticulos =
                    Micontexto.Articulo.Where(A => A.FechaVtoArt > DateTime.Now && A.Activo).OrderBy(A => A.NomArt).ToList();
                CargarGrillaCompleta();


                //cargo todas las categorías en ddl
                Session["todasLasCategorias"] = _todasLasCategorias = Micontexto.Categoria.Where(C => C.Activo).OrderBy(C => C.NomCat).ToList();
                DDLCategorias.DataSource = _todasLasCategorias;
                DDLCategorias.DataTextField = "NomCat";
                DDLCategorias.DataValueField = "CodCat";
                DDLCategorias.DataBind();
                DDLCategorias.Items.Insert(0, "Seleccionar Categoría");
            }
            else // es un post del formulario
            {
                //me traigo el contexto para trabajar
                Micontexto = Application["Contexto"] as FarmaciaEntities;

                //recarga
                _todosLosArticulos = (List<Articulo>)Session["todosLosArticulos"];
            }
        }
        catch (Exception ex)
        {
            LblError.Text = ex.Message;
        }
    }

    private void ArmadoContexto()
    {
        try
        {
            if (Application["Contexto"] == null)
                Application["Contexto"] = Micontexto = new FarmaciaEntities();
        }
        catch (Exception ex)
        {
            LblError.Text = ex.Message;
        }

    }

    public void CargarGrillaCompleta()
    {
        try
        {
            if (_todosLosArticulos.Count() > 0)
            {
                GVArticulos.DataSource = _todosLosArticulos;
                GVArticulos.DataBind();
            }
            else
            {
                LblError.Text = "No hay artículos para mostrar";
            }
        }
        catch (Exception ex)
        {
            LblError.Text = ex.Message;
        }
    }



    protected void GVArticulos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GVArticulos.PageIndex = e.NewPageIndex;

            if (Session["articulosFiltrados"] != null)
            {
                GVArticulos.DataSource = Session["articulosFiltrados"];
            }
            else
            {
                GVArticulos.DataSource = Session["todosLosArticulos"];
            }
            GVArticulos.DataBind();
        }
        catch (Exception ex)
        {
            LblError.Text = ex.Message;
        }
    }


    protected void GVArticulos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int pos;

            if (Session["articulosFiltrados"] != null)
            {
                pos = (GVArticulos.PageSize * GVArticulos.PageIndex) + GVArticulos.SelectedIndex;
                unA = ((List<Articulo>)Session["articulosFiltrados"])[pos];
            }
            else
            {
                pos = (GVArticulos.PageSize * GVArticulos.PageIndex) + GVArticulos.SelectedIndex;
                unA = ((List<Articulo>)Session["todosLosArticulos"])[pos];
            }

            LblNombre.Text = unA.NomArt;
            LblCodigo.Text = unA.CodArt;
            LblCategoria.Text = unA.Categoria.NomCat;
            LblFVenc.Text = unA.FechaVtoArt.ToString();
            LblPresentacion.Text = unA.TipoPArt;

            string unidad = "";
            switch (unA.TipoPArt.ToLower())
            {
                case "unidad":
                    unidad = "unidades";
                    break;
                case "blister":
                    unidad = "pastillas";
                    break;
                case "sobre":
                    unidad = "gramos";
                    break;
                case "frasco":
                    unidad = "ml";
                    break;
                default:
                    unidad = "desconocido";
                    break;
            }


            LblTamanio.Text = unA.TamArt.ToString() + " " + unidad;
            LblPrecio.Text = unA.PrecioArt.ToString() + " $";

            var _CantVent = (from unaL in Micontexto.Linea
                             where unaL.CodArt == unA.CodArt
                             select unaL).Count();
            LblCantVentas.Text = _CantVent.ToString();
        }
        catch (Exception ex)
        {
            LblError.Text = ex.Message;
        }
    }

    protected void BtnFiltrar_Click(object sender, EventArgs e)
    {
        try
        {
            _todosLosArticulos = (List<Articulo>)Session["todosLosArticulos"];
            List<Articulo> _articulosFiltrados = _todosLosArticulos;

            if (TxtArtNom.Text.Length > 0)
            {
                // filtro articulos por nombre del articulo
                _articulosFiltrados = (from unA in _articulosFiltrados
                                       where unA.NomArt.Trim().ToLower().Contains(TxtArtNom.Text.Trim().ToLower())
                                       select unA).ToList();
            }
            if (DDLCategorias.SelectedIndex > 0)
            {
                //filtro artículos por Categoría
                string selectedCodCat = DDLCategorias.SelectedValue;
                _articulosFiltrados = (from unA in _articulosFiltrados
                                       where unA.Categoria.CodCat == selectedCodCat
                                       select unA).ToList();
            }
            else
            {
                LblError.Text = "No hay coincidencias";

            }
            Session["articulosFiltrados"] = _articulosFiltrados;
            GVArticulos.DataSource = _articulosFiltrados;
            GVArticulos.DataBind();

        }
        catch (Exception ex)
        {
            LblError.Text = ex.Message;
        }
    }
    protected void BtnLimpiar_Click(object sender, EventArgs e)
    {
        try
        {
            CargarGrillaCompleta();
            Session["articulosFiltrados"] = null;
            unA = null;
            TxtArtNom.Text = "";
            DDLCategorias.SelectedIndex = -1;

            LblNombre.Text = "";
            LblCodigo.Text = "";
            LblCategoria.Text = "";
            LblFVenc.Text = "";
            LblPresentacion.Text = "";
            LblTamanio.Text = "";
            LblPrecio.Text = "";
            LblCantVentas.Text = "";

            LblError.Text = "";
        }
        catch (Exception ex)
        {
            LblError.Text = ex.Message;
        }
    }
}