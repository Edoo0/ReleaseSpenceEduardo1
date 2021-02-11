using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ReleaseSpence.Models;

namespace ReleaseSpence.Controllers
{
    [Authorize]
    public class PuntoDeMonitoreoController : ControladorBase
    {
        private MonitoreoIntegradoEntities db = new MonitoreoIntegradoEntities();

        [Authorize(Roles = "Administrador, Lectura, Modificacion, Escritura")]
        public ActionResult Index()
        {

            List<Punto_de_Monitoreo> puntoList = db.Punto_de_Monitoreo.Where(p => p.idPuntoMonitoreo != 0).ToList();

            List<Sensores> sensoresList = new List<Sensores>();

            List<int> puntosAGraficar = new List<int>();

            for (int i = 0; i < puntoList.Count(); i++)
            {
                int test = puntoList[i].idPuntoMonitoreo;
                sensoresList = db.Sensores.Where(s => s.idPuntoMonitoreo == test).ToList();

                if (sensoresList.Count() > 0)
                {
                    puntosAGraficar.Add(test);
                }
            }

            ViewBag.puntosAGraficar = puntosAGraficar;

            var PuntoMonitoreo = db.Punto_de_Monitoreo;
            return View(PuntoMonitoreo.ToList());
        }

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Lectura + "," + RolesSistema.Modificacion + "," + RolesSistema.Escritura)]
        public ActionResult SensoresAsociados(int? id)
        {
            List<Sensores> contSensores = db.Sensores.Where(s => s.idPuntoMonitoreo == id).ToList();

            ViewBag.cont = contSensores.Count();

            var sensor = db.Sensores;
            ViewBag.idPuntoMonitoreo = id;
            ViewBag.nombrePunto = db.Punto_de_Monitoreo.Find(id).nombre;
            return View(sensor.ToList());
        }

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Escritura)]
        public ActionResult Create()
        {
            ModelState.Clear();
            ViewBag.idFigura = new SelectList(db.Figuras, "idFigura", "nombre");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Escritura)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PuntoMonitoreoViewModel PuntoMonitoreoVM)
        {
            if (ModelState.IsValid)
            {
                
                Punto_de_Monitoreo puntoMonitoreo = new Punto_de_Monitoreo(PuntoMonitoreoVM);
                PuntoMonitoreoRep.Create(puntoMonitoreo);
                return RedirectToAction("Index");
            }
            ViewBag.idFigura = new SelectList(db.Figuras, "idFigura", "nombre");
            return View();
        }
               
        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Modificacion)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Punto_de_Monitoreo puntoMonitoreo = db.Punto_de_Monitoreo.Find(id);
            if (puntoMonitoreo == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.idFigura = new SelectList(db.Figuras, "idFigura", "nombre", puntoMonitoreo.idFigura);
                return RedirectToAction("EditPuntoMonitoreo", new { id = id });
            }

        }

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Modificacion)]
        public ActionResult EditPuntoMonitoreo(int? id)
        {
            PuntoMonitoreoViewModel puntoMonitoreo = new PuntoMonitoreoViewModel(db.Punto_de_Monitoreo.Find(id));
            ViewBag.idFigura = new SelectList(db.Figuras, "idFigura", "nombre", puntoMonitoreo.idFigura);
            ViewBag.nombrePuntoMonitoreo = db.Punto_de_Monitoreo.Find(puntoMonitoreo.idPuntoMonitoreo).nombre;
            ViewBag.carpetaPuntoMonitoreo = db.Punto_de_Monitoreo.Find(puntoMonitoreo.idPuntoMonitoreo).carpeta;
            ViewBag.cotaTierraPuntoMonitoreo = db.Punto_de_Monitoreo.Find(puntoMonitoreo.idPuntoMonitoreo).cotaTierra;
            return View(puntoMonitoreo);
        }

        [HttpPost]
        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Modificacion)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idSensor,idFigura,nombre,maxLimit,minLimit")] Punto_de_Monitoreo puntoMonitoreo)
        {
            if (ModelState.IsValid)
            {
                PuntoMonitoreoRep.Update(puntoMonitoreo);
                return RedirectToAction("Index");
            }

            return View(puntoMonitoreo);
        }

        [HttpPost]
        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Modificacion)]
        [ValidateAntiForgeryToken]
        public ActionResult EditPuntoMonitoreo(PuntoMonitoreoViewModel puntoMonitoreoVM)
        {

            if (ModelState.IsValid)
            {
                Punto_de_Monitoreo puntoMonitoreo = new Punto_de_Monitoreo(puntoMonitoreoVM);
                PuntoMonitoreoRep.Update(puntoMonitoreoVM);
                return RedirectToAction("Index");
            }
            return View(puntoMonitoreoVM);
        }

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Modificacion)]
        public PartialViewResult Delete(int? id)
        {
            if (id == null)
            {
                return PartialView(HttpStatusCode.BadRequest);
            }

            bool verif = false;
            //List<Sensores> sensoresList = db.Sensores.ToList();
            //List<Sensores> sensoresListPunto = new List<Sensores>();
            List<Sensores> sensoresListPunto = db.Sensores.Where(s => s.idPuntoMonitoreo == id).ToList();

            //foreach (var item in sensoresList)
            //{
            //    if (item.idPuntoMonitoreo == id)
            //    {
            //        sensoresListPunto.Add(item);
            //    }
            //}

            if (sensoresListPunto.Count > 0)
            {
                verif = true;
            }

            ViewBag.sensoresList = sensoresListPunto;
            ViewBag.verif = verif;

            Punto_de_Monitoreo puntoMonitoreo = db.Punto_de_Monitoreo.Find(id);
            return PartialView(puntoMonitoreo);

        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Modificacion)]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            List<Sensores> listSensores = db.Sensores.Where(s => s.idPuntoMonitoreo == id).ToList();

            var a = db.MarcadoresMpz.Where(m => m.idPuntoMonitoreo == id).Count();
            
            if (a > 0) {
                MarcadoresMpz marcadorPunto = new MarcadoresMpz();
                marcadorPunto.idPuntoMonitoreo = id;
                BorraMarcadorPunto(marcadorPunto);
            }
            
            
            
            for (int i = 0; i < listSensores.Count; i++)
            {
                Sensores sensor = db.Sensores.Find(listSensores[i].idSensor);

                if (sensor == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    sensor.idPuntoMonitoreo = 0;

                    if (ModelState.IsValid)
                    {
                        SensoresRep.Update(sensor);
                    }
                }               
            }
            //}
            Punto_de_Monitoreo puntoMonitoreo = db.Punto_de_Monitoreo.Find(id);
            db.Punto_de_Monitoreo.Remove(puntoMonitoreo);
            db.SaveChanges();
            

            return RedirectToAction("Index");

        }
        private void BorraMarcadorPunto ([Bind(Include = "idPuntoMonitoreo")] MarcadoresMpz marcadorPunto)
        {
          
                if (ModelState.IsValid)
                {
                    MarcadoresMpzRep.Delete(marcadorPunto);
                }
           
        }

        private void BorraMarcadorSensor([Bind(Include = "idSensor")] Marcadores marcadorSensor)
        {
            if (ModelState.IsValid)
            {
                MarcadoresRep.Delete(marcadorSensor);
            }
        }

        //Administracion de Sensores del punto de Monitoreo

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Modificacion)]
        public PartialViewResult EliminarAsociacion(int? id)
        {
            if (id == null)
            {
                return PartialView(HttpStatusCode.BadRequest);
            }

            Sensores sensor = db.Sensores.Find(id);
            return PartialView(sensor);
        }


        [HttpPost, ActionName("EliminarAsociacion")]
        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Modificacion)]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarAsociacionConfirmed(int id)
        {
            Sensores sensor = db.Sensores.Find(id);

            if (sensor == null)
            {
                return HttpNotFound();
            }
            else
            {

                id = sensor.idPuntoMonitoreo;
                sensor.idPuntoMonitoreo = 0;

                if (ModelState.IsValid)
                {
                    SensoresRep.Update(sensor);

                    ViewBag.Added = true;
                    return RedirectToAction("SensoresAsociados", "PuntoDeMonitoreo", new { id = id });
                }

                ViewBag.Added = false;
                return RedirectToAction("SensoresAsociados", "PuntoDeMonitoreo", new { id = id });
            }
        }


        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Lectura + "," + RolesSistema.Modificacion + "," + RolesSistema.Escritura)]
        public ActionResult AsociarSensor(int? id)
        {
            var sensores = db.Sensores;
            ViewBag.idPuntoMonitoreo = id;
            return View(sensores.ToList());
        }


        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Modificacion)]
        public ActionResult AsociarSensorAdd(int? id, int idPunto)
        {           
            Sensores sensor = db.Sensores.Find(id);
            
            var a = db.Marcadores.Where(m => m.idSensor == id).Count();
            if (a > 0)
            {
                Marcadores marcadorSensor = new Marcadores();
                marcadorSensor.idSensor = sensor.idSensor;
                BorraMarcadorSensor(marcadorSensor);
            }

            if (sensor == null)
            {
                return HttpNotFound();
            }
            else
            {
                sensor.idPuntoMonitoreo = idPunto;
                id = sensor.idPuntoMonitoreo;

                if (ModelState.IsValid)
                {
                    
                    SensoresRep.Update(sensor);
                    
                    ViewBag.Added = true;
                    return RedirectToAction("SensoresAsociados", "PuntoDeMonitoreo", new { id = id });
                }

                ViewBag.Added = false;
                return RedirectToAction("SensoresAsociados", "PuntoDeMonitoreo", new { id = id });
            }
        }

    }
}
