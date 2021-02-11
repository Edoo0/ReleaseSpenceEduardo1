using ReleaseSpence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReleaseSpence.Controllers
{
    [Authorize]
    public class MultiPiezometroController : ControladorBase
    {
        private MonitoreoIntegradoEntities db = new MonitoreoIntegradoEntities();

        private void ViewbagMulti(int id)
        {
            List<Sensores> listaSensores = db.Sensores.Where(i => i.idPuntoMonitoreo == id).OrderBy(i => i.Sensores_Piezometros.cotaSensor).ToList();
      
            int pz1 = 0;
            int pz2 = 0;
            int pz3 = 0;
            int pz4 = 0;
            int pz5 = 0;

            int cantidad = listaSensores.Count();

            switch (cantidad)
            {
                case 5:
                    {
                        pz1 = listaSensores[0].idSensor;
                        pz2 = listaSensores[1].idSensor;
                        pz3 = listaSensores[2].idSensor;
                        pz4 = listaSensores[3].idSensor;
                        pz5 = listaSensores[4].idSensor;
                        break;
                    }

                case 4:
                    {
                        pz1 = listaSensores[0].idSensor;
                        pz2 = listaSensores[1].idSensor;
                        pz3 = listaSensores[2].idSensor;
                        pz4 = listaSensores[3].idSensor;
                        break;
                    }

                case 3:
                    {
                        pz1 = listaSensores[0].idSensor;
                        pz2 = listaSensores[1].idSensor;
                        pz3 = listaSensores[2].idSensor;
                        break;
                    }

                case 2:
                    {
                        pz1 = listaSensores[0].idSensor;
                        pz2 = listaSensores[1].idSensor;
                        break;
                    }

                case 1:
                    {
                        pz1 = listaSensores[0].idSensor;
                        break;
                    }
                case 0:
                    {
                        ViewBag.confirmacion = true;
                        break;
                    }
                default:
                    {
                    }
                    break;
            }

            ViewBag.BaseX = ViewBag.carpeta;
            
            for (int i = 0; i < listaSensores.Count; i++)
            {
                if (listaSensores[i].Sensores_Piezometros.cotaSensor < ViewBag.BaseX)
                {
                    ViewBag.BaseX = listaSensores[i].Sensores_Piezometros.cotaSensor;
                }
            }

            if (ViewBag.carpeta < ViewBag.BaseX)
            {
                ViewBag.BaseX = ViewBag.carpeta;
            }
            float Base = ViewBag.BaseX;
            string base1 = Base.ToString().Replace(",",".");
            ViewBag.Base = base1;


            ViewBag.pz1 = pz1;
            ViewBag.pz2 = pz2;
            ViewBag.pz3 = pz3;
            ViewBag.pz4 = pz4;
            ViewBag.pz5 = pz5;

            ViewBag.cantPz = cantidad;
            ViewBag.pzPunto = listaSensores;            
        }
             
        [Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Lectura)]
        public ActionResult Index(int id, int idTipo, bool precision, int datos, string desde, string hasta)
        {            

            Punto_de_Monitoreo puntoMonitoreo = db.Punto_de_Monitoreo.Find(id);
            ViewBag.puntoMonitoreo = puntoMonitoreo;

            ViewBag.id = id;
            ViewBag.carpeta = puntoMonitoreo.carpeta;
            string cotaC = puntoMonitoreo.carpeta.ToString();
            cotaC = cotaC.Replace(",",".");
            ViewBag.carpetaS = cotaC;

            ViewBag.cotaTierra = puntoMonitoreo.cotaTierra;
            string cotaT = puntoMonitoreo.cotaTierra.ToString();
            cotaT = cotaT.Replace(",",".");
            ViewBag.cotaTierraS = cotaT;

            ViewbagMulti(id);

            //Controlador de la página de Multigráficos:
            DateTime fdesde;
            DateTime fhasta;
            Funciones.DesdeHasta(desde, hasta, out fdesde, out fhasta);

            int pz1 = ViewBag.pz1;
            int pz2 = ViewBag.pz2;
            int pz3 = ViewBag.pz3;
            int pz4 = ViewBag.pz4;
            int pz5 = ViewBag.pz5;

            ViewBag.idTipo = 7;
            ViewBag.precision = precision;
            ViewBag.datos = datos;
            ViewBag.desde = fdesde;
            ViewBag.hasta = fhasta;

            ViewBag.cota1 = 0;
            ViewBag.nombreS1 = "";
            ViewBag.total1 = 0;
            ViewBag.str1 = null;

            ViewBag.cota2 = 0;
            ViewBag.nombreS2 = "";
            ViewBag.total2 = 0;
            ViewBag.str2 = null;

            ViewBag.cota3 = 0;
            ViewBag.nombreS3 = "";
            ViewBag.total3 = 0;
            ViewBag.str3 = null;

            ViewBag.cota4 = 0;
            ViewBag.nombreS4 = "";
            ViewBag.total4 = 0;
            ViewBag.str4 = null;

            ViewBag.cota5 = 0;
            ViewBag.nombreS5 = "";
            ViewBag.total5 = 0;
            ViewBag.str5 = null;

            
            //Tomar datos para el string que grafica la cota agua del sensor 1
            string datoString = "";

            if (pz1 != 0)
            {
                int cont = 0;
                List<Datos_piezometro> datosP = Datos_piezometroRep.Graphics(pz1, precision, datos, fdesde, fhasta);

                if (datosP.Count != 0)
                {
                    while (cont < datosP.Count)
                    {
                        datoString += "['" + datosP[cont].fecha.ToString("yyyy-MM-dd H:mm:ss")
                        + "', " + datosP[cont].cotaAgua.ToString().Replace(",", ".")
                        + ", " + datosP[cont].metrosSensor.ToString().Replace(",", ".")
                        + ", " + (datosP[cont].presion_pz <= 0 ? "'no disponible'" : datosP[cont].presion_pz.ToString().Replace(",", "."))
                        + ", " + datosP[cont].temperatura_pz.ToString().Replace(",", ".")
                        + ", " + datosP[cont].presion_bmp.ToString().Replace(",", ".")
                        + ", " + datosP[cont].temperatura_bmp.ToString().Replace(",", ".")
                        + ", " + (datosP[cont].bUnits == 0 ? "'no disponible'" : datosP[cont].bUnits.ToString().Replace(",", ".")) + "] ,";
                        cont++;
                    }
                }

                if (datosP.Count > 0)
                {
                    ViewBag.primero1 = datosP[0].fecha;
                    ViewBag.ultimo1 = datosP[datosP.Count - 1].fecha;
                }

                string cota1S = db.Sensores.Find(pz1).Sensores_Piezometros.cotaSensor.ToString();
                ViewBag.cota1S = cota1S.Replace(",",".");
                ViewBag.cota1 = db.Sensores.Find(pz1).Sensores_Piezometros.cotaSensor;
                ViewBag.nombreS1 = db.Sensores.Find(pz1).nombre;
                ViewBag.total1 = db.Datos_piezometro.Where(d => d.idSensor == pz1 && d.fecha > fdesde && d.fecha < fhasta).Count();
                ViewBag.str1 = datoString;
                ViewBag.idS1 = pz1;
            }


            //Tomar datos para el string que grafica la cota agua del sensor 2
            datoString = "";

            if (pz2 != 0)
            {
                int cont = 0;

                List<Datos_piezometro> datosP = Datos_piezometroRep.Graphics(pz2, precision, datos, fdesde, fhasta);

                if (datosP.Count != 0)
                {
                    while (cont < datosP.Count)
                    {
                        datoString += "['" + datosP[cont].fecha.ToString("yyyy-MM-dd H:mm:ss")
                        + "', " + datosP[cont].cotaAgua.ToString().Replace(",", ".")
                        + ", " + datosP[cont].metrosSensor.ToString().Replace(",", ".")
                        + ", " + (datosP[cont].presion_pz <= 0 ? "'no disponible'" : datosP[cont].presion_pz.ToString().Replace(",", "."))
                        + ", " + datosP[cont].temperatura_pz.ToString().Replace(",", ".")
                        + ", " + datosP[cont].presion_bmp.ToString().Replace(",", ".")
                        + ", " + datosP[cont].temperatura_bmp.ToString().Replace(",", ".")
                        + ", " + (datosP[cont].bUnits == 0 ? "'no disponible'" : datosP[cont].bUnits.ToString().Replace(",", ".")) + "] ,";
                        cont++;
                    }
                }

                if (datosP.Count > 0)
                {
                    ViewBag.primero2 = datosP[0].fecha;
                    ViewBag.ultimo2 = datosP[datosP.Count - 1].fecha;
                }
                string cota2S = db.Sensores.Find(pz2).Sensores_Piezometros.cotaSensor.ToString();
                ViewBag.cota2S = cota2S.Replace(",", ".");
                ViewBag.cota2 = db.Sensores.Find(pz2).Sensores_Piezometros.cotaSensor;
                ViewBag.nombreS2 = db.Sensores.Find(pz2).nombre;
                ViewBag.total2 = db.Datos_piezometro.Where(d => d.idSensor == pz2 && d.fecha > fdesde && d.fecha < fhasta).Count();
                ViewBag.str2 = datoString;
                ViewBag.idS2 = pz2;
            }


            //Tomar datos para el string que grafica la cota agua del sensor 3
            datoString = "";

            if (pz3 != 0)
            {
                int cont = 0;

                List<Datos_piezometro> datosP = Datos_piezometroRep.Graphics(pz3, precision, datos, fdesde, fhasta);

                if (datosP.Count != 0)
                {
                    while (cont < datosP.Count)
                    {
                        datoString += "['" + datosP[cont].fecha.ToString("yyyy-MM-dd H:mm:ss")
                        + "', " + datosP[cont].cotaAgua.ToString().Replace(",", ".")
                        + ", " + datosP[cont].metrosSensor.ToString().Replace(",", ".")
                        + ", " + (datosP[cont].presion_pz <= 0 ? "'no disponible'" : datosP[cont].presion_pz.ToString().Replace(",", "."))
                        + ", " + datosP[cont].temperatura_pz.ToString().Replace(",", ".")
                        + ", " + datosP[cont].presion_bmp.ToString().Replace(",", ".")
                        + ", " + datosP[cont].temperatura_bmp.ToString().Replace(",", ".")
                        + ", " + (datosP[cont].bUnits == 0 ? "'no disponible'" : datosP[cont].bUnits.ToString().Replace(",", ".")) + "] ,";
                        cont++;
                    }
                }

                if (datosP.Count > 0)
                {
                    ViewBag.primero3 = datosP[0].fecha;
                    ViewBag.ultimo3 = datosP[datosP.Count - 1].fecha;
                }
                string cota3S = db.Sensores.Find(pz3).Sensores_Piezometros.cotaSensor.ToString();
                ViewBag.cota3S = cota3S.Replace(",", ".");
                ViewBag.cota3 = db.Sensores.Find(pz3).Sensores_Piezometros.cotaSensor;
                ViewBag.nombreS3 = db.Sensores.Find(pz3).nombre;
                ViewBag.total3 = db.Datos_piezometro.Where(d => d.idSensor == pz3 && d.fecha > fdesde && d.fecha < fhasta).Count();
                ViewBag.str3 = datoString;
                ViewBag.idS3 = pz3;
            }

            //Tomar datos para el string que grafica la cota agua del sensor 4
            datoString = "";

            if (pz4 != 0)
            {
                int cont = 0;

                List<Datos_piezometro> datosP = Datos_piezometroRep.Graphics(pz4, precision, datos, fdesde, fhasta);

                if (datosP.Count != 0)
                {
                    while (cont < datosP.Count)
                    {
                        datoString += "['" + datosP[cont].fecha.ToString("yyyy-MM-dd H:mm:ss")
                        + "', " + datosP[cont].cotaAgua.ToString().Replace(",", ".")
                        + ", " + datosP[cont].metrosSensor.ToString().Replace(",", ".")
                        + ", " + (datosP[cont].presion_pz <= 0 ? "'no disponible'" : datosP[cont].presion_pz.ToString().Replace(",", "."))
                        + ", " + datosP[cont].temperatura_pz.ToString().Replace(",", ".")
                        + ", " + datosP[cont].presion_bmp.ToString().Replace(",", ".")
                        + ", " + datosP[cont].temperatura_bmp.ToString().Replace(",", ".")
                        + ", " + (datosP[cont].bUnits == 0 ? "'no disponible'" : datosP[cont].bUnits.ToString().Replace(",", ".")) + "] ,";
                        cont++;
                    }
                }

                if (datosP.Count > 0)
                {
                    ViewBag.primero4= datosP[0].fecha;
                    ViewBag.ultimo4 = datosP[datosP.Count - 1].fecha;
                }
                string cota4S = db.Sensores.Find(pz4).Sensores_Piezometros.cotaSensor.ToString();
                ViewBag.cota4S = cota4S.Replace(",", ".");
                ViewBag.cota4 = db.Sensores.Find(pz4).Sensores_Piezometros.cotaSensor;
                ViewBag.nombreS4 = db.Sensores.Find(pz4).nombre;
                ViewBag.total4 = db.Datos_piezometro.Where(d => d.idSensor == pz4 && d.fecha > fdesde && d.fecha < fhasta).Count();
                ViewBag.str4 = datoString;
                ViewBag.idS4 = pz4;
            }

            //Tomar datos para el string que grafica la cota agua del sensor 5
            datoString = "";

            if (pz5 != 0)
            {
                int cont = 0;

                List<Datos_piezometro> datosP = Datos_piezometroRep.Graphics(pz5, precision, datos, fdesde, fhasta);

                if (datosP.Count != 0)
                {
                    while (cont < datosP.Count)
                    {
                        datoString += "['" + datosP[cont].fecha.ToString("yyyy-MM-dd H:mm:ss")
                        + "', " + datosP[cont].cotaAgua.ToString().Replace(",", ".")
                        + ", " + datosP[cont].metrosSensor.ToString().Replace(",", ".")
                        + ", " + (datosP[cont].presion_pz <= 0 ? "'no disponible'" : datosP[cont].presion_pz.ToString().Replace(",", "."))
                        + ", " + datosP[cont].temperatura_pz.ToString().Replace(",", ".")
                        + ", " + datosP[cont].presion_bmp.ToString().Replace(",", ".")
                        + ", " + datosP[cont].temperatura_bmp.ToString().Replace(",", ".")
                        + ", " + (datosP[cont].bUnits == 0 ? "'no disponible'" : datosP[cont].bUnits.ToString().Replace(",", ".")) + "] ,";
                        cont++;
                    }
                }


                if (datosP.Count > 0)
                {
                    ViewBag.primero5 = datosP[0].fecha;
                    ViewBag.ultimo5 = datosP[datosP.Count - 1].fecha;
                }
                string cota5S = db.Sensores.Find(pz5).Sensores_Piezometros.cotaSensor.ToString();
                ViewBag.cota5S = cota5S.Replace(",", ".");
                ViewBag.cota5 = db.Sensores.Find(pz5).Sensores_Piezometros.cotaSensor;
                ViewBag.nombreS5 = db.Sensores.Find(pz5).nombre;
                ViewBag.total5 = db.Datos_piezometro.Where(d => d.idSensor == pz5 && d.fecha > fdesde && d.fecha < fhasta).Count();
                ViewBag.str5 = datoString;
                ViewBag.idS5 = pz5;
            }

            ViewBag.primero = ViewBag.primero1;

            if (ViewBag.primero2 != null && ViewBag.primero2 < ViewBag.primero)
            {
                ViewBag.primero = ViewBag.primero2;
            }

            if (ViewBag.primero3 != null && ViewBag.primero3 < ViewBag.primero)
            {
                ViewBag.primero = ViewBag.primero3;
            }

            if (ViewBag.primero4 != null && ViewBag.primero4 < ViewBag.primero)
            {
                ViewBag.primero = ViewBag.primero4;
            }

            if (ViewBag.primero5 != null && ViewBag.primero5 < ViewBag.primero)
            {
                ViewBag.primero = ViewBag.primero5;
            }

            //------------------------------------------------------------------------------------
            ViewBag.ultimo = ViewBag.ultimo1;

            if (ViewBag.ultimo2 > ViewBag.ultimo)
            {
                ViewBag.ultimo = ViewBag.ultimo2;
            }

            if (ViewBag.ultimo3 > ViewBag.ultimo)
            {
                ViewBag.ultimo = ViewBag.ultimo3;
            }

            if (ViewBag.ultimo4 > ViewBag.ultimo)
            {
                ViewBag.ultimo = ViewBag.ultimo4;
            }

            if (ViewBag.ultimo5 > ViewBag.ultimo)
            {
                ViewBag.ultimo = ViewBag.ultimo5;
            }

            if (ViewBag.str1 == "" && ViewBag.str2 == "" && ViewBag.str3 == "" && ViewBag.str4 == "" && ViewBag.str5 == "")
            {
                ViewBag.confirmacion = true;
            }
            else
            {
                ViewBag.confirmacion = false;
            }

            return View();
        }
    }
}