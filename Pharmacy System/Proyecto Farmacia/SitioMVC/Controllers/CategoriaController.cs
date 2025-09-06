using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using _1_EC;
using _2_Logica;

namespace SitioMVC.Controllers
{
    public class CategoriaController : Controller
    {
        // Listado de Categorias 
        public ActionResult FormCategoriasListar(string DatoFiltro)
        {
            try
            {
                Empleado empLog = (Empleado)Session["Usuario"];
                List<Categoria> _lista = FabricaLogica.GetLogicaCategoria().ListarActivas(empLog);

                if (_lista.Count >= 1)
                {
                    //reviso si hay que filtrar
                    if (String.IsNullOrEmpty(DatoFiltro))
                        return View(_lista); //no hay filtro, muestro completo
                    else
                    {
                        //hay dato para filtro
                        _lista = (from unaC in _lista
                                  where unaC.NomCat.ToUpper().StartsWith(DatoFiltro.ToUpper())
                                  select unaC).ToList();
                        return View(_lista);
                    }
                }
                else
                    throw new Exception("No hay categorías para mostrar");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new List<Categoria>());
                //la vista espera una colección - no tengo datos, pero se parte sin una colección
            }
        }

        //Alta
        [HttpGet] //op de solicitar la vista - solo pide datos para alta
        public ActionResult FormCrearCategoria()
        {
            return View();
        }

        [HttpPost]//tengo que asegurar de que la categoría no se repita
        public ActionResult FormCrearCategoria(Categoria C)
        {
             try
             {
                Empleado empLog = (Empleado)Session["Usuario"];
                if (ModelState.IsValid)
                {                     
                    FabricaLogica.GetLogicaCategoria().Alta(C, empLog);
                    return RedirectToAction("FormCategoriasListar", "Categoria");
                }
                return View();
             }
             catch (Exception ex)
             {
                 ViewBag.Mensaje = ex.Message;
                 return View();
             }
        }


        //Modificar
        [HttpGet]
        public ActionResult FormModificarCategoria(string CodCat)
        {  
            try
            {
                Empleado empLog = (Empleado)Session["Usuario"];
                //obtengo la categoría
                Categoria unaCat = null;
                unaCat = FabricaLogica.GetLogicaCategoria().BuscarActiva(CodCat, empLog);
                if (unaCat != null)
                    return View(unaCat);
                else
                    throw new Exception("No existe la categoría");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Categoria());
            }
        }

        [HttpPost]
        public ActionResult FormModificarCategoria(Categoria C)
        {
            try
            {
                Empleado empLog = (Empleado)Session["Usuario"];
                //Valido objeto correcto
                C.Validar();

                //intento modificar
                FabricaLogica.GetLogicaCategoria().Modificar(C, empLog);
                return RedirectToAction("FormCategoriasListar", "Categoria");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Categoria());
            }
        }


        //Eliminar
        [HttpGet]
        public ActionResult FormEliminarCategoria(string CodCat)
        {
            try
            {
                Empleado empLog = (Empleado)Session["Usuario"];

                //obtengo la categoría
                Categoria unaCat = FabricaLogica.GetLogicaCategoria().BuscarActiva(CodCat, empLog);
                if (unaCat != null)
                    return View(unaCat);
                else
                    throw new Exception("No existe la categoría");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Categoria());
            }

        }

        [HttpPost]
        public ActionResult FormEliminarCategoria(Categoria unaC)
        {
            try
               
            {
                Empleado empLog = (Empleado)Session["Usuario"];
                //intento eliminar
                FabricaLogica.GetLogicaCategoria().Baja(unaC, empLog);
                return RedirectToAction("FormCategoriasListar", "Categoria");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Categoria());
            }
        }

        //Consultar

        [HttpGet]
        public ActionResult FormCategoriaConsultar(string CodCat)
        {
            try
            {
                Empleado empLog = (Empleado)Session["Usuario"];

                //obtengo la categoría
                Categoria unaCat = new Categoria();
                unaCat = FabricaLogica.GetLogicaCategoria().BuscarActiva(CodCat, empLog);
                if (unaCat != null)
                    return View(unaCat);
                else
                    throw new Exception("No existe la categoría");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Categoria());
            }
        }
    }
}