using ReleaseSpence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReleaseSpence.Controllers
{
    public class Datos_extensometroController : ControladorBase
    {
        private MonitoreoIntegradoEntities db = new MonitoreoIntegradoEntities();

        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Lectura)]
        public FileContentResult ExportCSV(int id, string desde, string hasta)
        {
            DateTime fdesde;
            DateTime fhasta;
            Funciones.DesdeHasta(desde, hasta, out fdesde, out fhasta);
            Sensores sensor = db.Sensores.Find(id);
            List<Datos_extensometroGraph> datos_extensometro = Datos_extensometroRep.Graphics(id, true, 1, fdesde, fhasta);
            string csv = "Fecha de creacion:;" + DateTime.Now.ToString("dd-MM-yyyy H:mm:ss") + "\r\n";
            csv += "Nombre del sensor:;" + sensor.nombre + "\r\n";
            csv += "Fecha;Extension[mm]\r\n";
            foreach (var dato in datos_extensometro)
            {
                csv += dato.fecha.ToString("yyyy-MM-dd H:mm:ss") + ";" + dato.dato + "\r\n";
            }
            return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", db.Sensores.Find(id).nombre + " " + DateTime.Now.ToString("yyyy_MM_dd H_mm_ss") + ".csv");
        }
    }
}
