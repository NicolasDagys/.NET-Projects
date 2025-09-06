using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using _1_EC;
using _2_Logica;

namespace SitioMVC.Controllers
{
    public class EmpleadoController : Controller
    {
        // GET: Formulario de Logueo
        [HttpGet]
        public ActionResult FormLogueo()
        {
            return View();
        }

        // POST: Proceso de Logueo
        [HttpPost]
        public ActionResult FormLogueo(string usuEmp, string passEmp)
        {
            try
            {

                Empleado empLog = FabricaLogica.GetLogicaEmpleado().Logueo(usuEmp, passEmp);

                if (empLog != null)
                {

                    Session["Usuario"] = empLog;

                    // Redirigir al listado de artí­culos
                    return RedirectToAction("FormCategoriasListar", "Categoria");
                }
                else
                {
                    ViewBag.Mensaje = "Usuario o contraseña incorrectos.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = "Error al procesar el inicio de sesión: " + ex.Message;
                return View();
            }
        }

        // Cerrar sesión
        public ActionResult CerrarSesion()
        {
            Session.Clear();
            return RedirectToAction("FormLogueo", "Empleado");
        }
    }
}