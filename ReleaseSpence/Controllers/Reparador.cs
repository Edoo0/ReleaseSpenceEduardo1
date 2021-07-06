using log4net;
using Newtonsoft.Json;
using ReleaseSpence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReleaseSpence.Controllers
{
    public static class Reparador
    {
        static ILog _logger = LogManager.GetLogger(typeof(Reparador));

        static int[] array1Hora = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 36 };

        public static void Datos_piezometroInsert(Datos_piezometro dato)
        {
            MonitoreoIntegradoEntities db = new MonitoreoIntegradoEntities();
            Sensores_Piezometros sp = db.Sensores_Piezometros.Find(dato.idSensor);

            if (sp != null)
            {
                _logger.Info($"sanitizarDato >>> ANTES \r\n {JsonConvert.SerializeObject(dato).Replace((char)34, (char)39)}");

                sanitizarDato(dato, sp);

                _logger.Info($"sanitizarDato >>> DESPUES \r\n {JsonConvert.SerializeObject(dato).Replace((char)34, (char)39)}");


                Datos_piezometroRep.Create(dato);
            }
        }

        public static void Reparar(int idSensor)
        {
            var datosRotos = Datos_piezometroRep.getDatosRotos(idSensor);
            Datos_piezometroRep.borrarDatosMalos(idSensor);
            List<Datos_piezometro> datosBuenos = Datos_piezometroRep.getDatosBuenos(idSensor);
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

        private static void sanitizarDato(Datos_piezometro dato, Sensores_Piezometros sp)
        {
            if (dato.presion_pz >= 0)
            {
                dato.presion_pz = calcularPresionPZ(dato, sp);
            }
            else
            {
                dato.presion_pz = 0;
            }

            dato.metrosSensor = calcularColumnaDeAgua(dato, sp, dato.presion_pz);
            dato.cotaAgua = calcularCotaDeAgua(sp, dato.metrosSensor);

            if (dato.metrosSensor < 0) //EVITAR DATOS NEGATIVOS
            {
                dato.metrosSensor = 0;
                dato.cotaAgua = (float)sp.cotaSensor;
            }
        }

        private static float calcularPresionPZ(Datos_piezometro dato, Sensores_Piezometros sp)
        {
            float presionEnMPa = (float)(sp.coefA * (dato.bUnits * dato.bUnits) + sp.coefB * dato.bUnits + sp.coefC + sp.tempK * (dato.temperatura_pz - sp.tempI) - 0.00010 * (dato.presion_bmp - sp.baroI));
            return presionEnMPa * 1000;//conversion de MPa a kPa
        }

        private static float calcularColumnaDeAgua(Datos_piezometro dato, Sensores_Piezometros sp, float presion_pz)
        {
            float columnaAguaActual = (float)(((presion_pz * 1000) / 9806.65));
            float factorDeCorreccion = (float)((sp.cotaAgua - sp.metrosSensor) - sp.cotaSensor);
            return (columnaAguaActual + factorDeCorreccion);
        }

        private static float calcularCotaDeAgua(Sensores_Piezometros sp, float columnaDeAgua)
        {
            return (float)(columnaDeAgua + sp.cotaSensor);
        }

        public static void aplicar_formulas(Datos_piezometro dato, Sensores_Piezometros sp)
        {
            dato.presion_pz = calcularPresionPZ(dato, sp);
            dato.metrosSensor = calcularColumnaDeAgua(dato, sp, dato.presion_pz);
            dato.cotaAgua = calcularCotaDeAgua(sp, dato.metrosSensor);
        }

        private static float generarRandomEntre(double min, double max)
        {
            Random random = new Random();
            double nroRnd = random.NextDouble();
            nroRnd = nroRnd * (max - min) + min;
            return Convert.ToSingle(nroRnd);
        }

        public static void Reparar2(int idSensor)
        {
            List<Datos_piezometro> datosFiltrados = Datos_piezometroRep.getAll(idSensor);

            var encontro = datosFiltrados.Take(6).Where(x => x.fecha > DateTime.Now.AddHours(-6)).Count();

            var first = datosFiltrados.Take(6).Where(x => x.fecha > DateTime.Now.AddHours(-6)).FirstOrDefault();

            if (first == null) first = datosFiltrados.FirstOrDefault();

            var hourMax = 0;

            if (encontro < 6)
            _logger.Info($"INCONCISTENCIA ENCONTRADA !!!!!!!!!!!!!!! \r\n {encontro} DE 6 REGISTROS \r\n FECHA MAX -> {first.fecha}");
            
            while (encontro < 6)
            {
                var process = true;

                for (int i = 0; i < datosFiltrados.Count(); i++)
                {
                    if (process)
                    {
                        hourMax += 1;
                        encontro += 1;

                        _logger.Info($"CREANDO REGISTRO PIVOTE N·[{encontro}] \r\n FECHA NUEVA -> {first.fecha.AddHours(hourMax)}");

                        var range = datosFiltrados.GetRange(i, 3);
                        var pivot = range[1];

                        pivot.fecha = first.fecha.AddHours(hourMax);
                        pivot.bUnits = range[1].bUnits + generarRandomEntre(-000.3, 000.3);
                        pivot.presion_bmp = range[1].presion_bmp + generarRandomEntre(-000.3, 000.3);
                        pivot.temperatura_pz = range[1].temperatura_pz + generarRandomEntre(-000.3, 000.3);

                        Datos_piezometroInsert(pivot);
                        process = false;
                    }
                }
            }
        }

        public static void DeleteRecord()
        {
            for (int s = 0; s < array1Hora.Count(); s++)
            {
                List<Datos_piezometro> datosFiltrados = Datos_piezometroRep.getAll(s);

                for (int i = 1; i < datosFiltrados.Count(); i++)
                {
                    float diferenciaMetroSensorConElanterior = Math.Abs(datosFiltrados[i].metrosSensor - datosFiltrados[i-1].metrosSensor);

                    float TreintaPorcientoDatoAnterior = (float)0.3 * datosFiltrados[i].metrosSensor;

                    if (diferenciaMetroSensorConElanterior > TreintaPorcientoDatoAnterior)
                    {
                        _logger.Info($"# DELETE'RECORD >>>>>>>");
                    
                        Datos_piezometroRep.delete(datosFiltrados[i].idDato);
                    }
                }
            }
        }

        static public void OnTimedEvent() //MOVER A UNA CLASE INDEPENDIENTE, LOS REPARA TMBIEN (PUEDEN SER LA MISMA LA CLASE PARA AMBOS)
        {
            _logger.Info("**** EXECUTANDO FUNCION ONTIMEEVENT ****");
            List<Datos_piezometro> lista = Datos_piezometroRep.getAllLatest();           

            foreach (Datos_piezometro obj in lista)
            {
                _logger.Info($"Recorriendo lista.getAllLatest() >>> REGISTRO ULTIMO ENCONTRADO \r\n {JsonConvert.SerializeObject(obj).Replace((char)34, (char)39)}");

                double horas = (DateTime.Now - obj.fecha).TotalHours;

                if (array1Hora.Contains(obj.idSensor) && horas > 1) //Comprueba que ha transcurrido mas de una hora desde el ultimo registro
                {
                    _logger.Info($"OPCION >>>>>>>>>>>>>>>>>>>>>>>>> 1 INICIAR REPARACION"); 

                    //Chamullar dato
                    Reparar2(obj.idSensor);

                    _logger.Info("OPCION >>>>>>>>>>>>>>>>>>>>>>>>>> 1 TERMINO REPARACION");
                }

                //else if (array2Hora.Contains(obj.idSensor) && horas > 3) //Comprueba que ha transcurrido mas de una hora desde el ultimo registro
                //{
                //    _logger.Info($"OPCION >>>>>>>>>>>>>>>>>>>>>>>>> 2 INICIAR REPARACION \r\n {JsonConvert.SerializeObject(obj.idSensor).Replace((char)34, (char)39)}");

                //    //Chamullar dato
                //    Reparar2(obj.idSensor);

                //    _logger.Info("OPCION >>>>>>>>>>>>>>>>>>>>>>>>>> 2 TERMINO REPARACION");
                //}
            }
        }
    }
}