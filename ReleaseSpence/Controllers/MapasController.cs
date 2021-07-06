using ReleaseSpence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ReleaseSpence.Controllers
{
	[Authorize]
	public class MapasController : ControladorBase
	{
		private MonitoreoIntegradoEntities db = new MonitoreoIntegradoEntities();

		[Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Lectura)]
		public ActionResult Lista(int id)
		{
			ViewBag.idTipo = id;
			ViewBag.nombre = db.TipoSensores.Find(id).nombre;
			IEnumerable<Imagenes> imagenes = db.Imagen_TipoSensor.Where(i => i.idTipo == id).Select(i => i.Imagenes);
			return View(imagenes);
		}

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Lectura)]
        public PartialViewResult Detalle(int id, int idTipo, string desde, string hasta)
		{
			if (hasta == null) hasta = "1s";
			DateTime fdesde;
			DateTime fhasta;
			Funciones.DesdeHasta(desde, hasta, out fdesde, out fhasta);
			ViewBag.desde = fdesde;
			ViewBag.hasta = fhasta;
			ViewBag.Imagen = db.Imagenes.Find(id);
			ViewBag.idTipo = idTipo;
            if (idTipo == 5) ViewBag.alertas = Datos_extensometroRep.Alertas(id, idTipo, fdesde, fhasta);
            else if (idTipo == 7) ViewBag.alertas = Datos_piezometroRep.Alertas(id, idTipo, fdesde, fhasta);
            List<Sensores> sensores = db.Sensores.Where(i => i.idTipo == idTipo).Where(i => i.Marcadores.idImagen == id).ToList();
           
            List<Punto_de_Monitoreo>  puntoList= db.Punto_de_Monitoreo.Where(i => i.MarcadoresMpz.idImagen == id).ToList();
            ViewBag.PuntoMonitoreoList = puntoList;


            return PartialView(Tuple.Create(sensores, puntoList));
        }

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Lectura)]
        public PartialViewResult AlarmasList(int id, int idTipo, DateTime desde, DateTime hasta)
		{
			ViewBag.sensor = db.Sensores.Find(id);
			ViewBag.desde = desde;
			ViewBag.hasta = hasta;
            List<Datos_sensor_alarmList> datos = new List<Datos_sensor_alarmList>();
            if (idTipo == 5) datos = Datos_extensometroRep.AlertasDet(id, desde, hasta);
            else if (idTipo == 7) datos = Datos_piezometroRep.AlertasDet(id, desde, hasta);
            return PartialView(datos);
		}

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Lectura + "," + RolesSistema.Escritura)]
        public PartialViewResult Mapa_click(int? id, int idImagen, int x, int y)
		{
			if (id == null)
			{
				return PartialView(HttpStatusCode.BadRequest);
			}
			ViewBag.idImagen = idImagen;
			ViewBag.idTipo = id;
			ViewBag.x = x;
			ViewBag.y = y;

			IEnumerable<Sensores> sensores = db.Sensores.Where(i => i.idTipo == id && i.idPuntoMonitoreo == 0 && i.Marcadores.idSensor == null);
            IEnumerable<Punto_de_Monitoreo> punto = db.Punto_de_Monitoreo.Where(p => p.idPuntoMonitoreo != 0 && p.MarcadoresMpz.idPuntoMonitoreo == null);
            ViewBag.idSensor = new SelectList(sensores, "idSensor", "nombre");
            ViewBag.idPuntoMonitoreo = new SelectList(punto, "idPuntoMonitoreo", "nombre");
			return PartialView();
		}

        //POST SENSORES CREAR MARCADOR
		[HttpPost]
        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Escritura)]
        [ValidateAntiForgeryToken]
		public void Mapa_click_sensor([Bind(Include = "idSensor,idImagen,x,y")] Marcadores marcador)
		{
			if (ModelState.IsValid)
			{
				MarcadoresRep.Create(marcador);
			}
		}

        //POST PUNTOMONITOREO CREAR MARCADORMPPZ
        [HttpPost]
        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Escritura)]
        [ValidateAntiForgeryToken]
        public void Mapa_click_punto([Bind(Include = "idPuntoMonitoreo,idImagen,x,y")] MarcadoresMpz marcadorMpz)
        {
            if (ModelState.IsValid)
            {
                MarcadoresMpzRep.Create(marcadorMpz);
            }
        }

        
        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Lectura + "," + RolesSistema.Modificacion)]
        public PartialViewResult Marcador_click_sensor(int? id, int idImagen, int idTipo)
        {
            if (id == null)
            {
                return PartialView(HttpStatusCode.BadRequest);
            }
            ViewBag.idImagen = idImagen;
            ViewBag.idTipo = idTipo;
            Sensores sensor = db.Sensores.Find(id);
            return PartialView(sensor);
        }

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Lectura + "," + RolesSistema.Modificacion)]
        public PartialViewResult Marcador_click_punto(int? id, int idImagen)
        {
            if (id == null)
            {
                return PartialView(HttpStatusCode.BadRequest);
            }
            ViewBag.idImagen = idImagen;
            ViewBag.idTipo = 7;
            Punto_de_Monitoreo punto = db.Punto_de_Monitoreo.Find(id);
            return PartialView(punto);
        }

        [HttpPost]
        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Modificacion)]
        [ValidateAntiForgeryToken]
        public void Marcador_click_sensor([Bind(Include = "idSensor")] Marcadores marcador)
        {
            if (ModelState.IsValid)
            {
                MarcadoresRep.Delete(marcador);
            }
        }

        [HttpPost]
        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Modificacion)]
        [ValidateAntiForgeryToken]
        public void Marcador_click_punto([Bind(Include = "idPuntoMonitoreo")] MarcadoresMpz marcadorMpz)
        {
            if (ModelState.IsValid)
            {
                MarcadoresMpzRep.Delete(marcadorMpz);
            }
        }

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Lectura)]
        public PartialViewResult Resumen_puntoMonitoreo(int? id)
        {
            if (id == null)
            {
                return PartialView(HttpStatusCode.BadRequest);
            }
            Punto_de_Monitoreo punto = db.Punto_de_Monitoreo.Find(id);
            List<Sensores> sensoresList = db.Sensores.Where(s => s.idPuntoMonitoreo == id).ToList();
            if (sensoresList.Count > 0)
            {
                ViewBag.sensoresList = sensoresList;
                ViewBag.punto = punto;
                return PartialView();
            }
            else
            {

                ViewBag.sensoresList = null;
                ViewBag.punto = punto;
                return PartialView();
            }            
        }

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Lectura)]
        public PartialViewResult Resumen_energia(int? id)
		{
			if (id == null)
			{
				return PartialView(HttpStatusCode.BadRequest);
			}
			Sensores sensor = db.Sensores.Find(id);
			return PartialView(sensor);
        }

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Lectura)]
        public PartialViewResult Resumen_piezometro(int? id)
        {
            if (id == null)
            {
                return PartialView(HttpStatusCode.BadRequest);
            }
            Sensores sensor = db.Sensores.Find(id);
            ViewBag.fecha = db.Datos_piezometro.Where(dp => dp.idSensor == id).OrderByDescending(dp => dp.fecha).Select(dp => dp.fecha).FirstOrDefault();

            ViewBag.cotaAgua = (from dp in db.Datos_piezometro where dp.idSensor == id orderby dp.fecha descending select dp.cotaAgua).FirstOrDefault();
            ViewBag.columnaAgua = (from dp in db.Datos_piezometro where dp.idSensor == id orderby dp.fecha descending select dp.metrosSensor).FirstOrDefault();
         
            DateTime semanaPasada = DateTime.Now.AddDays(-7);

            var promedio = db.Datos_piezometro.Where(dp => dp.idSensor == id && (dp.fecha <= DateTime.Now && dp.fecha >= semanaPasada)).OrderByDescending(dp => dp.fecha);

            ViewBag.promedio = 0;
            ViewBag.promedioColumna = 0;

            if (promedio.Count() > 0)
            {
            ViewBag.promedio = promedio.Average(dp => dp.cotaAgua);
            ViewBag.promedioColumna = promedio.Average(dp => dp.metrosSensor);
            }

            return PartialView(sensor);
        }

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Lectura)]
        public PartialViewResult Resumen_extensometro(int? id)
        {
            if (id == null)
            {
                return PartialView(HttpStatusCode.BadRequest);
            }
            Sensores sensor = db.Sensores.Find(id);
            ViewBag.fecha = db.Datos_extensometro.Where(dp => dp.idSensor == id).OrderByDescending(dp => dp.fecha).Select(dp => dp.fecha).FirstOrDefault();
            ViewBag.dato = (from dp in db.Datos_extensometro where dp.idSensor == id orderby dp.fecha descending select dp.dato).FirstOrDefault();
            return PartialView(sensor);
        }

        //regulador: voltaje(22-28), corriente(0-50)
        //inversor: consumo watora(0-99999999), potencia(20-100W)
        //alertas dinamicas definidas por usuario
        //lineas de minimo y maximo en graficos
        //cambiar marcador por icono de alarma o generar un sistema de alarma en el mapa.
        //metros de profundidad debajo de la superficie tierra 300m - 1000m
        //mm de mercurio 760 - 730 megapascales 0.30 - 20
        //metro diferencial entre la cota (superficie) del agua y boca del pozo
        //http://localhost:1670/Mapas/Graphics/8?idTipo=3&precision=false&datos=220&desde=10082014000000&hasta=11082014120000

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Lectura)]
        public ActionResult Graphics(int id, int idTipo, bool precision, int datos, string desde, string hasta)
		{
            //Controlador de la página de gráficos:
			DateTime fdesde;
			DateTime fhasta;
			Funciones.DesdeHasta(desde, hasta, out fdesde, out fhasta);
			ViewBag.id = id;
			ViewBag.idTipo = idTipo;
			ViewBag.precision = precision;
			ViewBag.datos = datos;
			ViewBag.desde = fdesde;
			ViewBag.hasta = fhasta;

            switch (idTipo)
            {
                case 2:
                    List<Datos_energia> datos_energia = Datos_energiaRep.GraphicsBat(id, precision, datos, fdesde, fhasta);
                    ViewBag.nombre = db.Sensores.Where(s => s.idSensor == id).Select(s => s.nombre).ToList()[0];
                    ViewBag.total = db.Datos_energia.Where(d => d.idSensor == id && d.fecha > fdesde && d.fecha < fhasta).Count();
                    return View("Graphics_energiaBat", datos_energia);
                case 3:
                    List<Datos_co2> datos_co2 = Datos_co2Rep.Graphics(id, precision, datos, fdesde, fhasta);
                    ViewBag.nombre = db.Sensores.Where(s => s.idSensor == id).Select(s => s.nombre).ToList()[0];
                    ViewBag.total = db.Datos_co2.Where(d => d.idSensor == id && d.fecha > fdesde && d.fecha < fhasta).Count();
                    return View(datos_co2);
                case 5:
                    List<Datos_extensometroGraph> datos_extensometro = Datos_extensometroRep.Graphics(id, precision, datos, fdesde, fhasta);
                    if (datos_extensometro.Count > 0)
                    {
                        ViewBag.primero = datos_extensometro[0].fecha;
                        ViewBag.ultimo = datos_extensometro[datos_extensometro.Count - 1].fecha;
                    }
                    ViewBag.sensor = db.Sensores.Find(id);
                    ViewBag.total = db.Datos_extensometro.Where(d => d.idSensor == id && d.fecha > fdesde && d.fecha < fhasta).Count();
                    return View("Graphics_extensometro", datos_extensometro);
                case 6:
                    List<Datos_pm10> datos_pm10 = Datos_pm10Rep.Graphics(id, 1, datos);
                    ViewBag.nombre = db.Sensores.Where(s => s.idSensor == id).Select(s => s.nombre).ToList()[0];
                    return View("Graphics_pm10", datos_pm10);
                case 7:
                    List<Datos_piezometro> datos_piezometro = Datos_piezometroRep.Graphics(id, precision, datos, fdesde, fhasta);
                    List<Datos_piezometro> datosP = Datos_piezometroRep.Graphics(id, precision, datos, fdesde, fhasta);


                    int cont = 0;
                    string datoString = "";

                    if (datosP.Count != 0)
                    {
                        while (cont < datosP.Count)
                        {
                            datoString += "['" + datosP[cont].fecha.ToString("yyyy-MM-dd H:mm:ss")
                            + "', " + datosP[cont].cotaAgua.ToString().Replace(",", ".")
                            + ", " + datosP[cont].metrosSensor.ToString().Replace(",", ".")
                            + ", " + (datosP[cont].presion_pz < 0 ? "'0'" : datosP[cont].presion_pz.ToString().Replace(",", "."))
                            + ", " + datosP[cont].temperatura_pz.ToString().Replace(",", ".")
                            + ", " + datosP[cont].presion_bmp.ToString().Replace(",", ".")
                            + ", " + datosP[cont].temperatura_bmp.ToString().Replace(",", ".")
                            + ", " + (datosP[cont].bUnits == 0 ? "'0'" : datosP[cont].bUnits.ToString().Replace(",", ".")) + "] ,";
                            cont++;
                        }
                        ViewBag.confirmacion = false;
                    }
                    else
                    {
                        ViewBag.confirmacion = true;
                    }



                    if (datos_piezometro.Count > 0)
                    {
                        ViewBag.primero = datos_piezometro[0].fecha;
                        ViewBag.ultimo = datos_piezometro[datos_piezometro.Count - 1].fecha;
                    }

                    ViewBag.sensor = db.Sensores.Find(id);

                    if (ViewBag.sensor.maxLimit != null)
                    {
                        float maxLinea = ViewBag.sensor.maxLimit;
                        string maxL = maxLinea.ToString();
                        ViewBag.maxL = maxL.Replace(",", ".");

                    }
                    else
                    {
                        ViewBag.maxL = 0;
                    }
                    if (ViewBag.sensor.minLimit != null)
                    {
                        float minLinea = ViewBag.sensor.minLimit;
                        string minL = minLinea.ToString();
                        ViewBag.minL = minL.Replace(",", ".");

                    }
                    else
                    {
                        ViewBag.minL = 0;
                    }

                    float cotaTierraReplace = ViewBag.sensor.Sensores_Piezometros.cotaTierra;
                    string cotaT = cotaTierraReplace.ToString();
                    ViewBag.cotaT = cotaT.Replace(",", ".");
                    
                    float cotaSensorReplace = ViewBag.sensor.Sensores_Piezometros.cotaSensor;
                    string cotaS = cotaSensorReplace.ToString();
                    ViewBag.cotaS = cotaS.Replace(",", ".");
                    
                    if (ViewBag.sensor.Sensores_Piezometros.carpeta != null) { 
                        float cotaCarpetaReplace = ViewBag.sensor.Sensores_Piezometros.carpeta;
                        string cotaC = cotaCarpetaReplace.ToString();
                        ViewBag.cotaC = cotaC.Replace(",", ".");
                       
                    }else{
                        ViewBag.cotaC = 0;
                    }

                    ViewBag.total = db.Datos_piezometro.Where(d => d.idSensor == id && d.fecha > fdesde && d.fecha < fhasta).Count();
                    ViewBag.str = datoString;

                    return View("Graphics_piezometro", datos_piezometro);
				default:
					return View();
			}
		}

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Lectura)]
        public FileContentResult Datos_piezometroCSV(int id, string desde, string hasta)
		{
            DateTime fdesde;
            DateTime fhasta;
            Funciones.DesdeHasta(desde, hasta, out fdesde, out fhasta);
            Sensores sensor = db.Sensores.Find(id);
            List<Datos_piezometro> datos_piezometro = Datos_piezometroRep.Graphics(id, true, 1, fdesde, fhasta);
            string csv = "HOJA DE LECTURAS DEL PIEZOMETRO \r\n";
            csv += "Fecha y Hora de creacion:;" + DateTime.Now.ToString("dd-MM-yyyy H:mm:ss") + "\r\n";
            csv += "Nombre de pozo:;" + sensor.nombre + ";;;Factores de Calibracion del Piezometro \r\n";
            csv += "Numero de serie Piezometro:;" + sensor.Sensores_Piezometros.seriePiezo + "\r\n";
            csv += "Cota de Tierra:;" + sensor.Sensores_Piezometros.cotaTierra + ";MSNM;;Factor de Calibracion A;=;" + sensor.Sensores_Piezometros.coefA + "\r\n";
            csv += "Cota de Agua Inicial:;" + sensor.Sensores_Piezometros.cotaAgua + ";MSNM;;Factor de Calibracion B;=;" + sensor.Sensores_Piezometros.coefB + "\r\n";//AGREGADO A CSV
            csv += "Cota del Piezometro:;" + sensor.Sensores_Piezometros.cotaSensor + ";MSNM;;Factor de Calibracion C;=;" + sensor.Sensores_Piezometros.coefC + "\r\n";//AGREGADO A CSV
            csv += "Cota de Geomembrana:;" + sensor.Sensores_Piezometros.carpeta + ";MSNM;;Factor de Correccion de Temperatura K;=;" + sensor.Sensores_Piezometros.tempK + "\r\n"; //AGREGADO A CSV
            csv += "Profundidad del Pozo:;" + sensor.Sensores_Piezometros.profundidadPozo + ";mts;;Factor de Calibracion Inicial;=;" + sensor.Sensores_Piezometros.metrosSensor + "\r\n"; //AGREGADO A CSV
            csv += "Temperatura Inicial del Piezometro:;" + sensor.Sensores_Piezometros.tempI + ";C;;\r\n";
            csv += "Presion Barometrica Inicial:;" + sensor.Sensores_Piezometros.baroI + ";mbar;\r\n\r\n"; //AGREGADO A CSV
            csv += "Fecha y Hora de medicion;Cota de Agua[MSNM];Columna de Agua[mts];Presion Piezometro[kPa];Temperatura Piezometro[C];B Units;Temperatura Ambiente[C];Presion Atmosferica[mB];Promedio Diario Cota de Agua[MSNM]\r\n";
			DateTime controlDia = datos_piezometro[0].fecha;
            float sumaCota = 0;
            int contadordatos = 0;
            foreach(var dato in datos_piezometro)
            {
                if (controlDia.Day != dato.fecha.Day)
                {
                    csv += ";" + sumaCota / contadordatos + "\r\n";
                    sumaCota = contadordatos = 0;
                }
                else
                {
                    csv += "\r\n";
                }
                csv += dato.fecha.ToString("yyyy-MM-dd H:mm:ss") + ";" + dato.cotaAgua + ";" + dato.metrosSensor + ";" + dato.presion_pz + ";" + dato.temperatura_pz + ";" + dato.bUnits + ";" + dato.temperatura_bmp + ";" + dato.presion_bmp;
				sumaCota += dato.cotaAgua;
                contadordatos++;
                controlDia = dato.fecha;
            }
            csv += ";" + sumaCota / contadordatos + "\r\n";
            return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", db.Sensores.Find(id).nombre + " " + DateTime.Now.ToString("yyyy_MM_dd H_mm_ss") + ".csv");
		}

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Lectura)]
        public ActionResult GraphicsChar(int id, int idTipo, bool precision, int datos, string desde, string hasta)
		{
			DateTime fdesde;
			DateTime fhasta;
			Funciones.DesdeHasta(desde, hasta, out fdesde, out fhasta);
			ViewBag.id = id;
			ViewBag.idTipo = idTipo;
			ViewBag.precision = precision;
			ViewBag.datos = datos;
			ViewBag.desde = fdesde;
			ViewBag.hasta = fhasta;
			List<Datos_energia> datos_energia = Datos_energiaRep.GraphicsChar(id, precision, datos, fdesde, fhasta);
			ViewBag.nombre = db.Sensores.Where(s => s.idSensor == id).Select(s => s.nombre).ToList()[0];
			ViewBag.total = db.Datos_energia.Where(d => d.idSensor == id && d.fecha > fdesde && d.fecha < fhasta).Count();
			return View("Graphics_energiaChar", datos_energia);
		}

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Lectura)]
        public ActionResult GraphicsInv(int id, int idTipo, bool precision, int datos, string desde, string hasta)
		{
			DateTime fdesde;
			DateTime fhasta;
			Funciones.DesdeHasta(desde, hasta, out fdesde, out fhasta);
			ViewBag.id = id;
			ViewBag.idTipo = idTipo;
			ViewBag.precision = precision;
			ViewBag.datos = datos;
			ViewBag.desde = fdesde;
			ViewBag.hasta = fhasta;
			List<Datos_energia> datos_energia = Datos_energiaRep.GraphicsInv(id, precision, datos, fdesde, fhasta);
			ViewBag.nombre = db.Sensores.Where(s => s.idSensor == id).Select(s => s.nombre).ToList()[0];
			ViewBag.total = db.Datos_energia.Where(d => d.idSensor == id && d.fecha > fdesde && d.fecha < fhasta).Count();
			return View("Graphics_energiaInv", datos_energia);
		}

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Lectura)]
        public ActionResult GraphicsConsumo(int id, int idTipo, bool precision, int datos, string desde, string hasta)
		{
			DateTime fdesde;
			DateTime fhasta;
			Funciones.DesdeHasta(desde, hasta, out fdesde, out fhasta);
			ViewBag.id = id;
			ViewBag.idTipo = idTipo;
			ViewBag.precision = precision;
			ViewBag.datos = datos;
			ViewBag.desde = fdesde;
			ViewBag.hasta = fhasta;
			List<Datos_energia> datos_energia = Datos_energiaRep.GraphicsConsumo(id, precision, datos, fdesde, fhasta);
			ViewBag.nombre = db.Sensores.Where(s => s.idSensor == id).Select(s => s.nombre).ToList()[0];
			ViewBag.total = db.Datos_energia.Where(d => d.idSensor == id && d.fecha > fdesde && d.fecha < fhasta).Count();
			return View("Graphics_energiaConsumo", datos_energia);
		}

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Lectura)]
        public ActionResult Monitoreo(int id)
		{
			ViewBag.id = id;
			Response.AddHeader("Refresh", "60");
			Datos_energia energia = (from h in db.Datos_energia where h.idDato == (from j in db.Datos_energia select j.idDato).Max() select h).ToList()[0];
			return View("Estado_energia", energia);
		}
	}
}