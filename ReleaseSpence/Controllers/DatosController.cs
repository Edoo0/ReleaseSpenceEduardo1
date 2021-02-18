using System;
using System.Web.Mvc;
using ReleaseSpence.Models;
using System.IO;
using System.Web;
using System.Threading.Tasks;

namespace ReleaseSpence.Controllers
{
    [Authorize]
    public class DatosController : ControladorBase
    {
        private MonitoreoIntegradoEntities db = new MonitoreoIntegradoEntities();

		public ActionResult Cargar()
		{
			ViewBag.idTipo = new SelectList(db.TipoSensores, "idTipo", "nombre");
			return View();
		}


        
        public void Reparar(int idSensor)
        {
            var datosRotos = Datos_piezometroRep.getDatosRotos(idSensor);
            Datos_piezometroRep.borrarDatosMalos(idSensor);
            var datosBuenos = Datos_piezometroRep.getDatosBuenos(idSensor);
            float accBUnit = 0;
            //float accTempBmp = 0;
            
            
            foreach (var datoBueno in datosBuenos)
            {
                accBUnit += datoBueno.bUnits;
               // accTempBmp += (float)datoBueno.temperatura_bmp;
               
                
            }
            var bUnitPromedio = accBUnit / datosBuenos.Count;
            //var tempBmpPromedio = accTempBmp / datosBuenos.Count;
            
            

            foreach (var datoRoto in datosRotos)
            {
                datoRoto.bUnits = bUnitPromedio;
                //datoRoto.temperatura_bmp = tempBmpPromedio;
                Datos_piezometroInsert(datoRoto);
            }
        }

        [HttpPost]
		[Authorize(Roles = RolesSistema.Administrador + "," + RolesSistema.Escritura)]
		[ValidateAntiForgeryToken]
		public ActionResult Cargar([Bind(Include = "archivos,idTipo")] HttpPostedFileBase[] archivos, int idTipo)
		{
			switch (idTipo)
			{
				case 2:
					{
						int contadordatos = 0;
						foreach (HttpPostedFileBase archivo in archivos)
						{
							if (archivo != null && archivo.ContentLength > 0)
							{
								BinaryReader bread = new BinaryReader(archivo.InputStream);
								byte[] archivobytes = bread.ReadBytes((int)archivo.InputStream.Length);
								string archivotext = System.Text.Encoding.UTF8.GetString(archivobytes);
								string[] lineas = archivotext.Split(new string[] { "\r\n" }, StringSplitOptions.None);
								foreach (string linea in lineas)
								{
									if (linea.Length > 0)
									{
										string[] variables = linea.Split(';');
										Datos_energia dato = new Datos_energia();
										dato.idSensor = int.Parse(variables[0]);
										dato.fecha = DateTime.Parse(variables[1]);
										dato.panelV = float.Parse(variables[2].Replace('.', ','));
										dato.panelP = float.Parse(variables[3].Replace('.', ','));
										dato.charC = float.Parse(variables[4].Replace('.', ','));
										dato.charT = float.Parse(variables[5].Replace('.', ','));
										dato.batV = float.Parse(variables[6].Replace('.', ','));
										dato.batC = float.Parse(variables[7].Replace('.', ','));
										dato.batSOC = float.Parse(variables[8].Replace('.', ','));
										dato.batCE = float.Parse(variables[9].Replace('.', ','));
										dato.batTTG = int.Parse(variables[10]);
										dato.inv_inC = float.Parse(variables[11].Replace('.', ','));
										dato.inv_outC = float.Parse(variables[12].Replace('.', ','));
										dato.panelC = (dato.panelV == 0 ? 0 : dato.panelP / dato.panelV);
										dato.charP = dato.charC * dato.batV;
										dato.charE = (dato.panelP == 0 ? 0 : (float)(dato.charP * 100.0) / dato.panelP);
										dato.batP = dato.batV * dato.batC;
										dato.inv_inP = dato.batV * dato.inv_inC;
										dato.inv_outP = (float)220.0 * dato.inv_outC;
										dato.invE = (dato.inv_inP == 0 ? 0 : (float)(dato.inv_outP * 100.0) / dato.inv_inP);
										Datos_energiaRep.Create(dato);
										contadordatos++;
									}
								}
							}
						}
						ViewBag.contdatos = contadordatos;
					}
					break;
                //En el cargar el 7 es el piezometro.
				case 7:
					{
						int contadordatos = 0;
						foreach (HttpPostedFileBase archivo in archivos)
						{
							if (archivo != null && archivo.ContentLength > 0)
							{                                                      
                                string archivotext = archivoATexto(archivo);
                                string[] lineas = archivotext.Split(new string[] { "\r\n" }, StringSplitOptions.None);
								foreach (string linea in lineas)
								{
									if (linea.Length > 0)
									{
										string[] variables = linea.Split(';');
										Datos_piezometro dato = new Datos_piezometro();
                                        dato.idSensor = int.Parse(variables[0]);
                                        dato.fecha = DateTime.Parse(variables[1]);
                                        dato.bUnits = float.Parse(variables[2].Replace('.', ','));
										dato.temperatura_pz = float.Parse(variables[3].Replace('.', ','));
										dato.temperatura_bmp = float.Parse(variables[4].Replace('.', ','));
										dato.presion_bmp = float.Parse(variables[5].Replace('.', ','));
										Datos_piezometroInsert(dato);
										contadordatos++;
									}
								}
							}
						}
						ViewBag.contdatos = contadordatos;
						break;
					}
			}
			ViewBag.idTipo = new SelectList(db.TipoSensores, "idTipo", "nombre");
			return View();
		}

		[HttpPost]
        [AllowAnonymous]
        public void Datos_energiaCreate([Bind(Include = "idSensor,panelV,panelP,charC,charT,batV,batC,batSOC,batCE,batTTG,inv_inC,inv_outC")] Datos_energia dato)
        {
            dato.fecha = DateTime.Now;
            dato.panelC = (dato.panelV == 0 ? 0 : dato.panelP / dato.panelV);
            dato.charP = dato.charC * dato.batV;
            dato.charE = (dato.panelP == 0 ? 0 : (float)(dato.charP * 100.0) / dato.panelP);
            dato.batP = dato.batV * dato.batC;
            dato.inv_inP = dato.batV * dato.inv_inC;
            dato.inv_outP = (float)220.0 * dato.inv_outC;
            dato.invE = (dato.inv_inP == 0 ? 0 : (float)(dato.inv_outP * 100.0) / dato.inv_inP);
            if (ModelState.IsValid)
            {
                Datos_energiaRep.Create(dato);
            }
        }

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<string> Datos_piezometroCreate(Datos_piezometro dato)
        //{
        //    //user = gbox@gmstechs.com
        //    //pass = gmstechs
        //    //dato.fecha = DateTime.Now;
        //    //var errores = ModelState.Values.SelectMany(v => v.Errors);
        //    //ModelState[nameof(Datos_piezometro.fecha)].Errors.Clear();
        //    var user = await UserManager.FindAsync(Request.Form["user"], Request.Form["pass"]);
        //    if (user != null && await UserManager.IsInRoleAsync(user.Id, RolesDelSistema.Logger))
        //    {
        //        Datos_piezometroInsert(dato);
        //        return "acgms ok";
        //    }
        //    return "acgms fail";
        //}

        [HttpPost]
        [AllowAnonymous]
        public async Task<string> Datos_piezometroBuffer()
        {
            var user = await UserManager.FindAsync(Request.Form["user"] == null ? "" : Request.Form["user"], Request.Form["pass"] == null ? "" : Request.Form["pass"]);
            if (user != null && await UserManager.IsInRoleAsync(user.Id, RolesSistema.Logger))
            {
                try
                {
                    string buffer = Request.Form["bufferdata"];
                    string[] lineas = buffer.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);//id;fecha;bunits;temp
                    foreach (string linea in lineas)
                    {
                        string[] variables = linea.Split(';');
                        Datos_piezometro dato = new Datos_piezometro();
                        dato.idSensor = int.Parse(variables[0]);
                        dato.fecha = DateTime.Parse(variables[1]);
                        dato.bUnits = float.Parse(variables[2].Replace('.', ','));
                        dato.temperatura_pz = float.Parse(variables[3].Replace('.', ','));
                        dato.temperatura_bmp = float.Parse(variables[4].Replace('.', ','));
                        dato.presion_bmp = float.Parse(variables[5].Replace('.', ','));
                        Datos_piezometroInsert(dato);
                    }
                    return "acgms ok";
                }
                catch
                {
                    return "acgms fail";
                }
            }
            return "acgms fail";
        }

        private float calcularPresionPZ(Datos_piezometro dato, Sensores_Piezometros sp)
        {
            float presionEnMPa = (float)(sp.coefA * (dato.bUnits * dato.bUnits) + sp.coefB * dato.bUnits + sp.coefC + sp.tempK * (dato.temperatura_pz - sp.tempI) - 0.00010 * (dato.presion_bmp - sp.baroI));
            return presionEnMPa * 1000;//conversion de MPa a kPa
        }

        private float calcularColumnaDeAgua(Datos_piezometro dato, Sensores_Piezometros sp, float presion_pz)
        {
            float columnaAguaActual = (float)(((presion_pz * 1000) / 9806.65));
            float factorDeCorreccion = (float)((sp.cotaAgua - sp.metrosSensor) - sp.cotaSensor);
            return (columnaAguaActual + factorDeCorreccion);
        }

        private float calcularCotaDeAgua(Sensores_Piezometros sp, float columnaDeAgua)
        {
            return (float)(columnaDeAgua + sp.cotaSensor);
        }

        private void aplicar_formulas(Datos_piezometro dato, Sensores_Piezometros sp)
        {
            dato.presion_pz = calcularPresionPZ(dato, sp);
            dato.metrosSensor = calcularColumnaDeAgua(dato, sp, dato.presion_pz);
            dato.cotaAgua = calcularCotaDeAgua(sp, dato.metrosSensor);
        }

        private string archivoATexto(HttpPostedFileBase archivo)
        {
            BinaryReader bread = new BinaryReader(archivo.InputStream);
            byte[] archivobytes = bread.ReadBytes((int)archivo.InputStream.Length);
            return System.Text.Encoding.UTF8.GetString(archivobytes);
        }
        private Boolean esDatoValido(Datos_piezometro dato, Sensores_Piezometros sp) {
            return !((dato.bUnits == 0) || (dato.temperatura_pz <= 0) || (dato.metrosSensor < 0) || (dato.metrosSensor > sp.cotaTierra));
        }

        

        private void Datos_piezometroInsertXD(Datos_piezometro dato)
        {
            Sensores_Piezometros sp = db.Sensores_Piezometros.Find(dato.idSensor);
            if (sp != null)
            {
                aplicar_formulas(dato, sp);
                if (esDatoValido(dato,sp))
                {
                    Console.WriteLine("es valido :)");
                }
                else
                {
                    Console.WriteLine("la re puta madre :c");
                }
                float accBunit = 0;
                float accTempPz = 0;
                var cantidad = 0;
                var ultimosDatos = Datos_piezometroRep.getLatest(6, dato.idSensor, dato.fecha);
                

                foreach (var i in ultimosDatos)
                {
                    if (esDatoValido(i,sp)) {
                        accBunit += i.bUnits;
                        accTempPz += i.temperatura_pz;
                        cantidad++;
                    }
                    
                }
                float promedioBunit = accBunit / cantidad;
                float promedioTempPz = accTempPz / cantidad;
                if (dato.bUnits == 0)
                {
                    dato.bUnits = promedioBunit;
                }
                if (dato.temperatura_pz <= 0)
                {
                    dato.temperatura_pz = promedioTempPz;
                }

                
                if (dato.metrosSensor < 0) //EVITAR DATOS NEGATIVOS
                {
                    dato.metrosSensor = 0;
                    dato.cotaAgua = (float)sp.cotaSensor;
                }
                if (dato.cotaAgua >= sp.cotaTierra)
                {
                    dato.bUnits = promedioBunit;
                    aplicar_formulas(dato, sp);

                }

                if (TryValidateModel(dato))
                {
                    Datos_piezometroRep.Create(dato);
                }
            }
        }

        private void Datos_piezometroInsert(Datos_piezometro dato)
        {
            Sensores_Piezometros sp = db.Sensores_Piezometros.Find(dato.idSensor);
            if (sp != null)
            {
                if (dato.presion_pz >= 0)
                {
                    dato.presion_pz = 1000;//conversion de MPa a kPa
                }
                else
                {

                    dato.presion_pz = 0;
                    //float min = 0;
                    //float max = (float)0.5;
                    //dato.presion_pz = (float) (new Random().NextDouble() * (max - min) + min);
                }

                dato.metrosSensor = (float)(((dato.presion_pz * 1000) / 9806.65));
                dato.metrosSensor += (float)((sp.cotaAgua - sp.metrosSensor) - sp.cotaSensor); // Factor de Corrección
                dato.cotaAgua = (float)(sp.cotaSensor + dato.metrosSensor);
                if (dato.metrosSensor < 0) //EVITAR DATOS NEGATIVOS
                {
                    dato.metrosSensor = 0;
                    dato.cotaAgua = (float)sp.cotaSensor;
                }
                
                if (TryValidateModel(dato))
                {
                    Datos_piezometroRep.Create(dato);
                }
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<string> Datos_extensometroBuffer()
        {
            var user = await UserManager.FindAsync(Request.Form["user"] == null ? "" : Request.Form["user"], Request.Form["pass"] == null ? "" : Request.Form["pass"]);
            if (user != null && await UserManager.IsInRoleAsync(user.Id, RolesSistema.Logger))
            {
                try
                {
                    string buffer = Request.Form["bufferdata"];
                    string[] lineas = buffer.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);//id;fecha;dato
                    foreach (string linea in lineas)
                    {
                        string[] variables = linea.Split(';');
                        Datos_extensometro dato = new Datos_extensometro();
                        dato.idSensor = int.Parse(variables[0]);
                        dato.fecha = DateTime.Parse(variables[1]);
                        dato.dato = float.Parse(variables[2].Replace('.', ','));
                        Datos_extensometroInsert(dato);
                    }
                    return "acgms ok";
                }
                catch
                {
                    return "acgms fail";
                }
            }
            return "acgms fail";
        }

        private void Datos_extensometroInsert(Datos_extensometro dato)
        {
            Sensores sp = db.Sensores.Find(dato.idSensor);
            if (sp != null)
            {
                if (TryValidateModel(dato))
                {
                    Datos_extensometroRep.Create(dato);
                }
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<string> Datos_piezometroSync(int? id)
        {
            var user = await UserManager.FindAsync(Request.Form["user"] == null ? "" : Request.Form["user"], Request.Form["pass"] == null ? "" : Request.Form["pass"]);
            if (user != null && await UserManager.IsInRoleAsync(user.Id, RolesSistema.Logger))
            {
                string returndata = "acgms ok\r\n";
                Sensores_Piezometros sensor = db.Sensores_Piezometros.Find(id);
                returndata += "st" + sensor.Sensores.syncToken + "\r\n";
                if (sensor.Sensores.syncToken == Request.Form["syncToken"]) return returndata;
                else
                {
                    returndata += "ca" + sensor.coefA + "\r\n";
                    returndata += "cb" + sensor.coefB + "\r\n";
                    returndata += "cc" + sensor.coefC + "\r\n";
                    returndata += "tk" + sensor.tempK + "\r\n";
                    returndata += "ti" + sensor.tempI + "\r\n";
                    returndata += "bi" + sensor.baroI + "\r\n";
                    returndata += "fr" + sensor.freqRead + "\r\n";
                    returndata += "fs" + sensor.freqSend + "\r\n";
                    returndata += "cs" + sensor.cotaSensor + "\r\n";
                    returndata += "ci" + sensor.cotaAgua + "\r\n"; 
                    returndata += "fd" + sensor.metrosSensor + "\r\n";
                    returndata += "ct" + sensor.cotaTierra + "\r\n"; 
                    return returndata;
                }
            }
            return "acgms fail";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize(Roles = RolesSistema.Administrador)]
        public void generar(int id)//idsensor
        {
            Datos_extensometro dato = new Datos_extensometro();
            dato.idSensor = id;
            dato.fecha = DateTime.Now.AddMonths(-12);
            Random rnd = new Random();
            int minimo = 5;
            int maximo = 1000;
            int max_variacion = 10;
            float referencial = 5;// (float)(rnd.Next((minimo * 10), (maximo * 10)) / 10.0);
            float valor = referencial;
            float variacion = 0;
            while (dato.fecha < DateTime.Now)
            {
                dato.fecha = dato.fecha.AddHours(4);
                //-------------------------------------------
                dato.dato = (float)Math.Round(valor, 2);
                int elrandom = rnd.Next(1, 100);
                variacion = (float)((Math.Pow(1.0+elrandom/1500.0, (elrandom)) - 1.0)/100);
                if (valor + variacion > maximo) valor = valor;
                else if (valor + variacion > maximo || valor + variacion < minimo) valor -= variacion;
                else valor += variacion;
                //var errores = ModelState.Values.SelectMany(v => v.Errors);
                //-----------------------------------------------
                Datos_extensometroRep.Create(dato);
            }

            //Datos_piezometro dato = new Datos_piezometro();
            //dato.idSensor = id;
            //dato.fecha = DateTime.Now.AddMonths(-24);
            //Random rnd = new Random();
            //int minimo = 7000;
            //int maximo = 8900;
            //int max_variacion = 500;
            //float referencial = (float)(rnd.Next((minimo * 10), (maximo * 10)) / 10.0);
            //float valor = referencial;
            //float variacion = 0;
            //Sensores_Piezometros sp = db.Sensores_Piezometros.Find(id);
            //while (dato.fecha < DateTime.Now)
            //{
            //    dato.fecha = dato.fecha.AddHours(4);
            //    //-------------------------------------------
            //    dato.bUnits = valor;
            //    dato.presion_bmp = (float)935.44;
            //    dato.temperatura_bmp = (float)25.38;
            //    dato.temperatura_pz = (float)(rnd.Next(1000, 2800) / 100.0);
            //    dato.presion_pz = (float)(sp.coefA * (dato.bUnits * dato.bUnits) + sp.coefB * dato.bUnits + sp.coefC + sp.tempK * (dato.temperatura_pz - sp.tempI) - 0.0001 * (dato.presion_bmp - sp.baroI));
            //    dato.presion_pz *= 1000;//conversion de MPa a kPa
            //    dato.metrosSensor = (float)((dato.presion_pz * 1000) / 9806.65);
            //    dato.cotaAgua = (float)(sp.cotaSensor + dato.metrosSensor);
            //    variacion = (float)(rnd.Next((-max_variacion * 10), (max_variacion * 10)) / 10.0);
            //    if (valor + variacion > maximo || valor + variacion < minimo) valor -= variacion;
            //    else valor += variacion;
            //    //var errores = ModelState.Values.SelectMany(v => v.Errors);
            //    //-----------------------------------------------
            //    Datos_piezometroRep.Create(dato);
            //}
        }
    }
}