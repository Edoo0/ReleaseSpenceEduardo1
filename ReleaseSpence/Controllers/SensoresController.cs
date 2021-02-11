using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ReleaseSpence.Models;

namespace ReleaseSpence.Controllers
{
    [Authorize]
    public class SensoresController : ControladorBase
	{
		private MonitoreoIntegradoEntities db = new MonitoreoIntegradoEntities();

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Lectura + "," + RolesSistema.Modificacion + "," + RolesSistema.Escritura)]
        public ActionResult Index()
		{
			var sensores = db.Sensores.Include(s => s.Figuras).Include(s => s.Marcadores).Include(s => s.TipoSensores).Include(s => s.Punto_de_Monitoreo);
			return View(sensores.ToList());
		}

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Lectura)]
        public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Sensores sensores = db.Sensores.Find(id);
			if (sensores == null)
			{
				return HttpNotFound();
			}
			return View(sensores);
		}

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Escritura)]
        public ActionResult Tipo()
        {
            ViewBag.idTipo = new SelectList(db.TipoSensores, "idTipo", "nombre");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Escritura)]
        public ActionResult Tipo([Bind(Include = "idTipo")] Sensores sensores)
        {
            if(sensores.idTipo == 7)
            {
                return RedirectToAction("CreatePiezometro", new { idTipo = sensores.idTipo });
            }
            else
            {
                return RedirectToAction("Create", new { idTipo = sensores.idTipo });
            }
        }

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Escritura)]
        public ActionResult CreatePiezometro(int idTipo)
        {
            ViewBag.idFigura = new SelectList(db.Figuras, "idFigura", "nombre");
            ViewBag.idTipo = idTipo;
            ViewBag.idPuntoMonitoreo = 0;
            ViewBag.nombreTipo = db.TipoSensores.Find(idTipo).nombre;
            ModelState.Clear();
            return View();
        }

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Escritura)]
        public ActionResult Create(int idTipo)
        {
            ViewBag.idFigura = new SelectList(db.Figuras, "idFigura", "nombre");
            ViewBag.idTipo = idTipo;
            ViewBag.idPuntoMonitoreo = 0;
            ViewBag.nombreTipo = db.TipoSensores.Find(idTipo).nombre;
            ModelState.Clear();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Escritura)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idTipo,idFigura,idPuntoMonitoreo,nombre,maxLimit,minLimit")] Sensores sensores)
        {
            if (ModelState.IsValid)
            {
                sensores.idPuntoMonitoreo = 0;
                SensoresRep.Create(sensores);
                return RedirectToAction("Index");
            }
            ViewBag.idFigura = new SelectList(db.Figuras, "idFigura", "nombre", sensores.idFigura);
            ViewBag.idPuntoMonitoreo = 0;
            ViewBag.nombreTipo = db.TipoSensores.Find(sensores.idTipo).nombre;
            return View(sensores);
        }

        [HttpPost]
        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Escritura)]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePiezometro(SensoresViewModel sensorVM)
        {
            sensorVM.Sensores_Piezometros.freqRead = sensorVM.Sensores_Piezometros.freqRead * 1000 * 60;
            sensorVM.Sensores_Piezometros.freqSend = sensorVM.Sensores_Piezometros.freqSend * 1000 * 60 * 60;
            if (ModelState.IsValid)
            {
                Sensores sensor = new Sensores(sensorVM);
                sensor.idPuntoMonitoreo = 0;
                Sensores_PiezometrosRep.Create(sensor);
                return RedirectToAction("Index");
            }
            ViewBag.idFigura = new SelectList(db.Figuras, "idFigura", "nombre", sensorVM.idFigura);
            ViewBag.idPuntoMonitoreo = 0;
            ViewBag.nombreTipo = db.TipoSensores.Find(sensorVM.idTipo).nombre;
            return View(sensorVM);
        }

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Modificacion)]
        public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
            Sensores sensor = db.Sensores.Find(id);
            if (sensor == null)
            {
                return HttpNotFound();
            }
            if (sensor.idTipo == 7)
            {
                return RedirectToAction("EditPiezometro", new { id = id });
            }
            else
            {
                ViewBag.nombreTipo = db.TipoSensores.Find(sensor.idTipo).nombre;
                ViewBag.idFigura = new SelectList(db.Figuras, "idFigura", "nombre", sensor.idFigura);
                ViewBag.idPuntoMonitoreo = db.Punto_de_Monitoreo.Find(sensor.idPuntoMonitoreo).idPuntoMonitoreo;
                return View(sensor);
            }
        }

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Modificacion)]
        public ActionResult EditPiezometro(int? id)
        {
            SensoresViewModel sensor = new SensoresViewModel(db.Sensores.Find(id));
            sensor.Sensores_Piezometros.freqRead = sensor.Sensores_Piezometros.freqRead / (1000 * 60);
            sensor.Sensores_Piezometros.freqSend = sensor.Sensores_Piezometros.freqSend / (1000 * 60 * 60);
            ViewBag.idFigura = new SelectList(db.Figuras, "idFigura", "nombre", sensor.idFigura);
            ViewBag.nombreTipo = db.TipoSensores.Find(sensor.idTipo).nombre;
            return View(sensor);
        }

        [HttpPost]
        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Modificacion)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idSensor,idFigura,idPuntoMonitoreo,nombre,maxLimit,minLimit")] Sensores sensores)
        {
            if (ModelState.IsValid)
            {
                SensoresRep.Update(sensores);
                return RedirectToAction("Index");
            }
            ViewBag.idFigura = new SelectList(db.Figuras, "idFigura", "nombre", sensores.idFigura);
            return View(sensores);
        }

        [HttpPost]
        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Modificacion)]
        [ValidateAntiForgeryToken]
        public ActionResult EditPiezometro(SensoresViewModel sensorVM)
        {
            sensorVM.Sensores_Piezometros.freqRead = sensorVM.Sensores_Piezometros.freqRead * 1000 * 60;
            sensorVM.Sensores_Piezometros.freqSend = sensorVM.Sensores_Piezometros.freqSend * 1000 * 60 * 60;
            //ModelState[nameof(Sensores_Piezometros) + "." + nameof(Sensores_Piezometros.tempK)].Errors.Clear();//sin estoy hay problemas al recibir notacion cientifica que se produce sola al ingresar muchos decimales
            if (ModelState.IsValid)
            {
                Sensores sensor = new Sensores(sensorVM);
                Sensores_PiezometrosRep.Update(sensor);
                return RedirectToAction("Index");
            }
            ViewBag.idFigura = new SelectList(db.Figuras, "idFigura", "nombre", sensorVM.idFigura);
            ViewBag.nombreTipo = db.TipoSensores.Find(sensorVM.idTipo).nombre;
            return View(sensorVM);
        }

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Modificacion)]
        public ActionResult DatosPiezo(int? id)
        {
            CorregirPiezo correccion = new CorregirPiezo(db.Sensores.Find(id));
            return View(correccion);
        }

        [HttpPost]
        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Modificacion)]
        [ValidateAntiForgeryToken]
        public ActionResult DatosPiezo(CorregirPiezo correccion)
        {
            //ModelState[nameof(Sensores_Piezometros) + "." + nameof(Sensores_Piezometros.tempK)].Errors.Clear();//sin estoy hay problemas al recibir notacion cientifica que se produce sola al ingresar muchos decimales
            if (ModelState.IsValid)
            {
                Datos_piezometroRep.Corregir(correccion);
                return RedirectToAction("Index");
            }
            return View(correccion);
        }

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Modificacion)]
        public PartialViewResult Delete(int? id)
		{
			if (id == null)
			{
				return PartialView(HttpStatusCode.BadRequest);
			}
			Sensores sensores = db.Sensores.Find(id);
			return PartialView(sensores);
		}

		[HttpPost, ActionName("Delete")]
        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Modificacion)]
        [ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
            Sensores sensores = db.Sensores.Find(id);
            var a = db.Marcadores.Where(m => m.idSensor == id).Count();
            if (a > 0)
            {
                Marcadores marcadorSensor = new Marcadores();
                marcadorSensor.idSensor = sensores.idSensor;
                BorraMarcadorSensor(marcadorSensor);
            }
            
			db.Sensores.Remove(sensores);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}
        private void BorraMarcadorSensor([Bind(Include = "idSensor")] Marcadores marcadorSensor)
        {
            if (ModelState.IsValid)
            {
                MarcadoresRep.Delete(marcadorSensor);
            }
        }
    }
}