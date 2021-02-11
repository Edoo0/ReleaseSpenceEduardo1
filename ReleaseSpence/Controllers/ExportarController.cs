using ReleaseSpence.Models;
using ReleaseSpence.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReleaseSpence.Controllers
{
    [Authorize]
    public class ExportarController : ControladorBase
    {
        private MonitoreoIntegradoEntities db = new MonitoreoIntegradoEntities();

        // GET: Exportar
        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Lectura + "," + RolesSistema.Modificacion + "," + RolesSistema.Escritura)]
        public ActionResult Index()
        {
            ViewbagContenedor();
            return View();
        }
        
        // POST: Exportar
        
        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Lectura + "," + RolesSistema.Modificacion + "," + RolesSistema.Escritura)]
        [HttpPost]
        public ActionResult Index(int idSensorx = 0, string desdef="", string hastaf="") 
        {
            


            if (idSensorx != 0)
            {
                FormatoDatetime(desdef, hastaf);
                int id = idSensorx;
                DateTime fdesde;
                DateTime fhasta;
                string desde = ViewBag.desdef;
                string hasta = ViewBag.hastaf;
                Funciones.DesdeHasta(desde, hasta, out fdesde, out fhasta);
                Sensores sensor = db.Sensores.Find(id);
                List<Datos_piezometro> datos_piezometro = Datos_piezometroRep.Graphics(id, true, 1, fdesde, fhasta);

                if (datos_piezometro.Count == 0)
                {
                    
                    ViewbagContenedor();
                    ViewBag.hayDatos = false;
                    return View();
                }                                             

                return RedirectToAction("Datos_piezometroCSV", "Mapas", new { id = idSensorx, desde = ViewBag.desdef, hasta = ViewBag.hastaf });
            }
            else
            {
                ViewbagContenedor();
                ViewBag.hayIdSensor = true;
                return View();
            }            
        }

        private void ViewbagContenedor()
        {
            var queryId = db.Sensores.Select(c => new { c.idSensor, c.nombre });
            ViewBag.idSensores = new SelectList(queryId.AsEnumerable(), "idSensor", "nombre");

            var desdex = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy HH:mm:ss");
            var hastax = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            ViewBag.desdex = desdex;
            ViewBag.hastax = hastax;

        }

        private void FormatoDatetime(string desdef, string hastaf)
        {
            desdef = desdef.Replace(":", "").Replace("-", "").Replace(" ", "");
            hastaf = hastaf.Replace(":", "").Replace("-", "").Replace(" ", "");
            ViewBag.desdef = desdef;
            ViewBag.hastaf = hastaf;
        }
    }
}