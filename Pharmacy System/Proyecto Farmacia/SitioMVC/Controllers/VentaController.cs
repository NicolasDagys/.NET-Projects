using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using _1_EC;
using _2_Logica;

namespace SitioMVC.Controllers
{
    public class VentaController : Controller
    {
        [HttpGet]
        public ActionResult FormVentaAlta()
        {
            try
            {
                //cargo lista de clientes para mostrar en una lista desplegable en la vista
                Empleado empLog = (Empleado)Session["Usuario"];
                List<Cliente> _ListaC = new List<Cliente>();
                _ListaC = FabricaLogica.GetLogicaCliente().Listar(empLog);

                ViewBag.ListaClientes = new SelectList(_ListaC, "CiCli", "NomCli");
                //SelectList es el DDL
                //_ListaC es la fuente de datos (DataSource)
                //CiCli es el dato que devuelve (DataValueField)
                //NomCli es el valor que va a mostrar (DataTextField)
                //ViewBag es para pasarle el DDL a la vista

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ListaClientes = new SelectList(null);
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }


        [HttpPost]
        public ActionResult FormVentaAlta(Venta V, string cedulaCli)
        {
            try
            {
                //Cargo fecha de la venta 
                V.FechaVent = DateTime.Today.Date;

                //Obtengo el Cliente
                Empleado empLog = (Empleado)Session["Usuario"];
                if (cedulaCli.Trim().Length > 0)
                {
                    V.UnCliente = FabricaLogica.GetLogicaCliente().Buscar(cedulaCli, empLog);
                }

                //Asigno el primer estado
                Asignacion _PrimAsig = new Asignacion();
                List<Asignacion> _listaAsignaciones = new List<Asignacion>();
                _PrimAsig.FyHEst = DateTime.Today;
                _PrimAsig.UnEstado = FabricaLogica.GetLogicaEstado().Buscar(1, empLog);
                _listaAsignaciones.Add(_PrimAsig);
                V.ListaAsignaciones = _listaAsignaciones;

                //Lineas
                V.ListaLineas = new List<Linea>(); //creo la lista de líneas
                Session["Venta"] = V; //guardo la Venta en la session para agregar lineas                
                return RedirectToAction("FormLineasVentaAlta", "Venta"); //enruto a la vista que me va pidiendo las líneas de a una
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public ActionResult FormLineasVentaAlta()
        {
            try
            {
                //cargo lista de articulos para mostrar en lista desplegable en la vista
                Empleado empLog = (Empleado)Session["Usuario"];
                List<Articulo> _ListaA = new List<Articulo>();
                _ListaA = FabricaLogica.GetLogicaArticulo().ListarActivos(empLog);
                ViewBag.ListaArticulos = new SelectList(_ListaA, "CodArt", "NomArt");

                //muestro la vista
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ListaArticulos = new SelectList(null);
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }

        [HttpPost]
        public ActionResult FormLineasVentaAlta(Linea L, string codigoArt)
        {
            Empleado empLog = (Empleado)Session["Usuario"];
            try
            {
                if (codigoArt.Trim().Length > 0)
                {
                    L.UnArt = FabricaLogica.GetLogicaArticulo().BuscarActivo(codigoArt, empLog);
                }

                L.Validar(); //Valido línea


                //Agrego línea a la venta en la session
                ((Venta)Session["Venta"]).ListaLineas.Add(L);

                //Calculo el total de la venta
                int _totalVent = ((Venta)Session["Venta"]).TotalVent;
                _totalVent = _totalVent + (L.Cant * L.UnArt.PrecioArt);
                ((Venta)Session["Venta"]).TotalVent = _totalVent;

                //muestro la vista de nuevo, pero vacía, provoco el GET
                return RedirectToAction("FormLineasVentaAlta", "Venta");
            }
            catch (Exception ex)
            {
                List<Articulo> _ListaA = FabricaLogica.GetLogicaArticulo().ListarActivos(empLog);
                ViewBag.ListaArticulos = new SelectList(_ListaA, "CodArt", "NomArt");
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public ActionResult GuardarVenta()
        {
            Empleado empLog = (Empleado)Session["Usuario"];
            try
            {
                //obtengo la venta armada
                Venta V = ((Venta)Session["Venta"]);

                //valido el modelo 
                V.Validar();

                //mando a dar de alta la venta a la BD
                FabricaLogica.GetLogicaVenta().Alta(V, empLog);

                //redirecciono a vista que solo tiene un mensaje de éxito
                //con RedirectToAction porque esta op no tiene vista
                return RedirectToAction("FormAltaExito", "Venta");
            }
            catch (Exception ex)
            {
                //muestro una vista con los errores pq esta op no tiene vista
                //uso la session para comunicar datos entre operaciones
                Session["ErrorVenta"] = ex.Message;
                return RedirectToAction("FormAltaError", "Venta");
                //redirecciono a vista que solo tiene mensaje de error
                //con RedirectToAction porque esta op no tiene vista
                //El mensaje no lo puedo guardar en el ViewBag pq éste solo se comunica con su vista
            }
        }

        public ActionResult FormAltaExito()
        {
            return View();
        }

        public ActionResult FormAltaError()
        {
            //bajo de session el error ylo pongo en el comunicador operación - vista
            ViewBag.Mensaje = Session["ErrorVenta"].ToString();
            return View();
        }

        public ActionResult FormVentasAsociadasArt(string CodArt)
        {
            try
            {
                Empleado empLog = (Empleado)Session["Usuario"];
                List<Venta> _listaVentas = FabricaLogica.GetLogicaVenta().ListarVentasTodas(empLog);

                Articulo _UnA = new Articulo();
                _UnA = FabricaLogica.GetLogicaArticulo().BuscarActivo(CodArt, empLog);

                if (_UnA == null)
                    throw new Exception("No existe artículo con ese código");

                _listaVentas = (from unaV in _listaVentas
                                from unaL in unaV.ListaLineas
                                where unaL.UnArt.CodArt == CodArt
                                orderby unaV.FechaVent
                                select unaV).ToList();

                if (_listaVentas.Count == 0)
                    throw new Exception("No hay ventas asociadas a este artículo");

                return View(_listaVentas);
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new List<Venta>());
            }
        }

        public ActionResult FormVentasListarxCli(string CiCli)
        {
            try
            {
                // Obtener el empleado logueado
                Empleado empLog = (Empleado)Session["Usuario"];

                // Obtener la lista de todas las ventas
                List<Venta> listaVentas = FabricaLogica.GetLogicaVenta().ListarVentasTodas(empLog);

                // Buscar el cliente específico
                Cliente unCliente = FabricaLogica.GetLogicaCliente().Buscar(CiCli, empLog);

                if (unCliente == null)
                    throw new Exception("No existe un cliente con esa cédula.");

                // Filtrar las ventas asociadas al cliente
                listaVentas = (from unaV in listaVentas
                               where unaV.UnCliente.CiCli == CiCli
                               orderby unaV.FechaVent
                               select unaV).ToList();

                if (listaVentas.Count == 0)
                    throw new Exception("No hay ventas asociadas a este cliente.");

                return View(listaVentas);
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new List<Venta>());
            }
        }

        public ActionResult FormVentasListarxArt(string CiCli)
        {
            try
            {
                Empleado empLog = (Empleado)Session["Usuario"];


                List<Venta> listaVentas = FabricaLogica.GetLogicaVenta().ListarVentasTodas(empLog);

                // Ventas asociadas al cliente
                List<Venta> ventasCliente = (from unaV in listaVentas
                                             where unaV.UnCliente.CiCli == CiCli
                                             select unaV).ToList();

                if (ventasCliente.Count == 0)
                    throw new Exception("No hay ventas asociadas a este cliente.");

                // Obtener todos los artículos comprados por el cliente
                List<Articulo> articulosComprados = new List<Articulo>();
                foreach (var venta in ventasCliente)
                {
                    foreach (var linea in venta.ListaLineas)
                    {
                        articulosComprados.Add(linea.UnArt);
                    }
                }

                // Eliminar duplicados y ordenar por nombre
                var articulosUnicos = articulosComprados
                    .GroupBy(a => a.CodArt)
                    .Select(g => g.First())
                    .OrderBy(a => a.NomArt)
                    .ToList();

                if (articulosUnicos.Count == 0)
                    throw new Exception("El cliente no ha comprado ningún artículo.");

                return View(articulosUnicos);
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new List<Articulo>());
            }
        }

        public ActionResult FormSeguimientoVenta(string Fecha, string numEst)
        {
            try
            {
                //1- Obtengo fuente de datos para el listado
                Empleado empLog = (Empleado)Session["Usuario"];
                List<Venta> _listaVent = FabricaLogica.GetLogicaVenta().ListarVentasTodas(empLog);

                if (_listaVent.Count == 0)
                    throw new Exception("No hay Artículos para mostrar");

                //2- Armo un drop por la categoría
                List<Estado> _listaE = FabricaLogica.GetLogicaEstado().ListarEstados(empLog);
                _listaE.Insert(0, new Estado(0, "Seleccione Estado"));
                ViewBag.ListaE = new SelectList(_listaE, "numEst", "nomEst");

                //3- filtros

                if (_listaVent.Count >= 1)
                {
                    if (!String.IsNullOrEmpty(Fecha) && Fecha.Trim() != "0")
                    {

                        _listaVent = (from unaV in _listaVent
                                      where unaV.FechaVent.Date == Convert.ToDateTime(Fecha).Date //Formato "01/01/2025"
                                      select unaV).ToList();
                    }
                    if (!String.IsNullOrEmpty(numEst) && numEst.Trim() != "0")
                    {
                        //quiero las ventas que tienen el estado numEst. 
                        _listaVent = (from unaV in _listaVent
                                      from unE in unaV.ListaAsignaciones
                                      where unE.UnEstado.NumEst == Convert.ToInt32(numEst)
                                      select unaV).ToList();
                    }
                    return View(_listaVent);
                }
                else
                    throw new Exception("No hay Ventas para mostrar");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = Session["MensajeCambioEstado"].ToString();
                ViewBag.Mensaje = ex.Message;
                //ViewBag.ListaE = new SelectList(null);
                return View(new List<Venta>());
            }
        }

        //OP SIN VISTA ASOCIADA
        public ActionResult CambiarEstado(int NumVent)
        {
            try
            {
                Empleado empLog = (Empleado)Session["Usuario"];
                List<Venta> _listaVent = FabricaLogica.GetLogicaVenta().ListarVentasTodas(empLog);
                Venta unaV = _listaVent.Where(V => V.NumVent == NumVent).FirstOrDefault();
                Session["Venta"] = unaV; //la voy a usar en FormCambioEstExito

                if (unaV != null)
                {
                    FabricaLogica.GetLogicaVenta().CambioEstadoVenta(unaV, empLog);
                    ViewBag.Mensaje = Session["MensajeCambioEstado"] = "Cabio de estado exitoso";
                }
                else
                    throw new Exception("No Existe la Venta");

                return RedirectToAction("FormCambioEstExito", "Venta"); //redirige a FormCambioEstExito

            }
            catch (Exception ex)
            {
                Session["MensajeCambioEstado"] = ex.Message;
                //ViewBag.Mensaje = ex.Message;
                return RedirectToAction("FormSeguimientoVenta", "Venta");  //redirige a FormCambioEstError
            }
        }


    }
}
