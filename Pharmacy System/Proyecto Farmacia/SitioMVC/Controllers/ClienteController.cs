using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using _1_EC;
using _2_Logica;

namespace SitioMVC.Controllers
{
    public class ClienteController : Controller
    {
        //Alta
        [HttpGet]
        public ActionResult FormCrearCliente()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FormCrearCliente(Cliente C)
        {
            try
            {
                Empleado empLog = (Empleado)Session["Usuario"];
                if (ModelState.IsValid)
                {
                    FabricaLogica.GetLogicaCliente().Alta(C, empLog);
                    return RedirectToAction("MantenimientoClientes", "Cliente");
                }
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public ActionResult FormModificarCliente()
        {
            // Mostrar el formulario de búsqueda inicial
            return View();
        }

        [HttpPost]
        public ActionResult BuscarCliente(string ciCli)
        {
            try
            {
                Empleado empLog = (Empleado)Session["Usuario"];
                // Buscar el cliente por su CI
                Cliente cliente = FabricaLogica.GetLogicaCliente().Buscar(ciCli, empLog);

                if (cliente == null)
                {
                    ViewBag.Mensaje = "No se encontró un cliente con la cédula proporcionada.";
                    return View("FormModificarCliente"); // Retornar la vista de búsqueda con el mensaje de error
                }

                // Mostrar el formulario de modificación con los datos del cliente
                return View("FormModificarCliente", cliente);
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View("FormModificarCliente"); // Retornar la vista de búsqueda con el mensaje de error
            }
        }

        [HttpPost]
        public ActionResult ModificarCliente(Cliente cliente)
        {
            try
            {
                Empleado empLog = (Empleado)Session["Usuario"];
                // Validar los datos del cliente
                cliente.Validar();

                // Modificar el cliente en la base de datos
                FabricaLogica.GetLogicaCliente().Modificar(cliente, empLog);

                ViewBag.Mensaje = "Cliente modificado correctamente.";
                return View("FormModificarCliente", cliente); // Retornar la vista de modificación con el mensaje de éxito
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View("FormModificarCliente", cliente);
            }
        }

        public ActionResult MantenimientoClientes()
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult FormConsultarCliente()
        {
            return View();
        }


        [HttpPost]
        public ActionResult FormConsultarCliente(string ciCli)
        {
            try
            {
                Empleado empLog = (Empleado)Session["Usuario"];
                List<Cliente> listaClientes = FabricaLogica.GetLogicaCliente().Listar(empLog);

                Cliente cliente = FabricaLogica.GetLogicaCliente().Buscar(ciCli, empLog);

                if (cliente == null)
                {
                    ViewBag.Mensaje = "No se encontró un cliente con la cédula proporcionada.";
                    return View(); // Retornar la misma vista con el mensaje de error
                }

                return View("FormDetallesCliente", cliente);
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }

        public ActionResult FormDetallesCliente(Cliente cliente)
        {
            return View(cliente);
        }
        public ActionResult ListadoInteractivoCliente(string filtroNombre)
        {
            try
            {
                // 1- Obtener la lista de clientes
                Empleado empLog = (Empleado)Session["Usuario"];
                List<Cliente> listaClientes = FabricaLogica.GetLogicaCliente().Listar(empLog);

                if (listaClientes.Count == 0)
                    throw new Exception("No hay clientes para mostrar");

                // 2- Aplicar filtro por nombre
                if (!string.IsNullOrEmpty(filtroNombre))
                {
                    listaClientes = listaClientes
                        .Where(c => c.NomCli.ToUpper().Contains(filtroNombre.ToUpper()))
                        .OrderBy(c => c.NomCli)
                        .ToList();
                }
                else
                {
                    // Ordenar por nombre si no hay filtro
                    listaClientes = listaClientes.OrderBy(c => c.NomCli).ToList();
                }

                return View(listaClientes);
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new List<Cliente>());
            }
        }
    }
}