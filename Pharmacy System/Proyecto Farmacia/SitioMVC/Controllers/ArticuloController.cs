using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using _1_EC;
using _2_Logica;

namespace SitioMVC.Controllers
{
    public class ArticuloController : Controller
    {
        public ActionResult FormArticulosListar(string DatoFiltro)
        {
            try
            {
                Empleado empLog = (Empleado)Session["Usuario"];
                List<Articulo> _lista = FabricaLogica.GetLogicaArticulo().ListarActivos(empLog);
                if (_lista.Count >= 1)
                {
                    if (String.IsNullOrEmpty(DatoFiltro))
                        return View(_lista);
                    else
                    {
                        _lista = (from unA in _lista
                                  where unA.NomArt.ToUpper().StartsWith(DatoFiltro.ToUpper())
                                  select unA).ToList();
                        return View(_lista);
                    }
                }
                else
                    throw new Exception("No hay artículos para mostrar");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new List<Articulo>());
            }
        }


        //Alta
        [HttpGet]
        public ActionResult FormCrearArticulo()
        {
            try
            {
                //Armo un ddl de tipos de presentación
                List<string> _listaP = new List<string>();
                _listaP.Add("Unidad");
                _listaP.Add("Blister");
                _listaP.Add("Sobre");
                _listaP.Add("Frasco");
                ViewBag.ListaP = new SelectList(_listaP);

                Empleado empLog = (Empleado)Session["Usuario"];
                //cargo lista de categorías para mostrar en ddl en la vista
                List<Categoria> _listaCat = FabricaLogica.GetLogicaCategoria().ListarActivas(empLog);
                ViewBag.ListaC = new SelectList(_listaCat, "CodCat", "NomCat");

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                ViewBag.ListaP = new SelectList(null);
                ViewBag.ListaC = new SelectList(null);
                return View();
            }
        }

        [HttpPost]
        public ActionResult FormCrearArticulo(Articulo A, string TipoP, string codigoCat)
        {
            try
            {
                A.TipoPArt = TipoP.ToLower();
                Empleado empLog = (Empleado)Session["Usuario"];

                //obtengo la categoría
                if (codigoCat.Trim().Length > 0)
                {
                    A.UnaCat = FabricaLogica.GetLogicaCategoria().BuscarActiva(codigoCat, empLog);
                }

                A.Validar();

                FabricaLogica.GetLogicaArticulo().Alta(A, empLog);
                return RedirectToAction("FormArticulosListar", "Articulo");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;

                List<string> _listaP = new List<string> { "Unidad", "Blister", "Sobre", "Frasco" };
                ViewBag.ListaP = new SelectList(_listaP);

                Empleado empLog = (Empleado)Session["Usuario"];
                List<Categoria> _listaCat = FabricaLogica.GetLogicaCategoria().ListarActivas(empLog);
                ViewBag.ListaC = new SelectList(_listaCat, "CodCat", "NomCat");
                return View();
            }
        }


        //Modificar
        [HttpGet]
        public ActionResult FormModificarArticulo(string CodArt)
        {
            try
            {
                //obtengo el artículo
                Empleado empLog = (Empleado)Session["Usuario"];
                Articulo _A = FabricaLogica.GetLogicaArticulo().BuscarActivo(CodArt, empLog);
                if (_A != null)
                {
                    //cargo lista de categorías para mostrar en ddl en la vista
                    List<Categoria> _listaCat = FabricaLogica.GetLogicaCategoria().ListarActivas(empLog);
                    ViewBag.ListaC = new SelectList(_listaCat, "CodCat", "NomCat", _A.UnaCat.CodCat); 
                    return View(_A);
                }
                else
                    throw new Exception("No existe el artículo");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                Empleado empLog = (Empleado)Session["Usuario"];
                List<Categoria> _listaCat = FabricaLogica.GetLogicaCategoria().ListarActivas(empLog); 
                ViewBag.ListaC = new SelectList(_listaCat, "CodCat", "NomCat");
                return View(new Articulo());
            }
        }

        [HttpPost]
        public ActionResult FormModificarArticulo(Articulo A, string TipoP, string codigoCat)
        {
            try
            {
                Empleado empLog = (Empleado)Session["Usuario"];
                //obtengo la categoría
                if (codigoCat.Trim().Length > 0)
                {
                    A.UnaCat = FabricaLogica.GetLogicaCategoria().BuscarActiva(codigoCat, empLog);
                }
                A.Validar();

                FabricaLogica.GetLogicaArticulo().Modificar(A, empLog);
                return RedirectToAction("FormArticulosListar", "Articulo");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                Empleado empLog = (Empleado)Session["Usuario"];
                List<Categoria> _listaCat = FabricaLogica.GetLogicaCategoria().ListarActivas(empLog);
                ViewBag.ListaC = new SelectList(_listaCat, "CodCat", "NomCat");
                return View(new Articulo());
            }
        }


        //Eliminar
        [HttpGet]
        public ActionResult FormEliminarArticulo(string CodArt)
        {
            try
            {
                //obtengo el artículo
                Empleado empLog = (Empleado)Session["Usuario"];
                Articulo _A = FabricaLogica.GetLogicaArticulo().BuscarActivo(CodArt, empLog);
                if (_A != null)
                    return View(_A);
                else
                    throw new Exception("No existe el artículo");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Articulo());
            }
        }

        [HttpPost]
        public ActionResult FormEliminarArticulo(Articulo A)
        {
            try
            {
                Empleado empLog = (Empleado)Session["Usuario"];
                FabricaLogica.GetLogicaArticulo().Baja(A, empLog);
                return RedirectToAction("FormArticulosListar", "Articulo");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Articulo());
            }
        }

        //Consultar
        [HttpGet]
        public ActionResult FormConsultarArticulo(string CodArt)
        {
            try
            {
                Empleado empLog = (Empleado)Session["Usuario"];
                Articulo _A = FabricaLogica.GetLogicaArticulo().BuscarActivo(CodArt, empLog);
                if (_A != null)
                    return View(_A);
                else
                    throw new Exception("No existe el artículo");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Articulo());
            }
        }

       public ActionResult ListadoInteractivoArticulos(string datoFiltro, string codigoCat)
        {   
            try
            {
                //1- Obtengo fuente de datos para el listado
                Empleado empLog = (Empleado)Session["Usuario"];
                List<Articulo> _listaArt = FabricaLogica.GetLogicaArticulo().ListarActivos(empLog);

                if (_listaArt.Count == 0)
                    throw new Exception("No hay Artículos para mostrar");

                //2- Armo un drop por la categoría
                List<Categoria> _listaCat = FabricaLogica.GetLogicaCategoria().ListarActivas(empLog);
                _listaCat.Insert(0, new Categoria("0", "Seleccione Categoría"));
                ViewBag.ListaC = new SelectList(_listaCat, "CodCat", "NomCat"); 

                //3- filtros
  
                if (_listaArt.Count >= 1)
                { 
                    if (!String.IsNullOrEmpty(datoFiltro) && datoFiltro.Trim() != "0")                     {
                        _listaArt = (from unA in _listaArt
                                 where unA.NomArt.ToUpper().StartsWith(datoFiltro.ToUpper())
                                 select unA).ToList();
                    }
                    if (!String.IsNullOrEmpty (codigoCat) && codigoCat.Trim() != "0")
                    {
                        _listaArt = (from unA in _listaArt
                                     where unA.UnaCat.CodCat == codigoCat.Trim()
                                     select unA).ToList();       
                    }
                    return View(_listaArt);
                }
                else
                    throw new Exception("No hay artículos para mostrar");
            }
            catch (Exception ex)
            {
                //cargo mensaje error
                ViewBag.Mensaje = ex.Message;
                //espera una colección IEnumerable de Movimeintos- Es una vista listado

                //hay que mandar algo para el drop de categorías
                ViewBag.ListaC = new SelectList(null);
                return View(new List<Articulo>());
                throw;
            }
        }
    }
}