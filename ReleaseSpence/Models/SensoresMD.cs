using System.ComponentModel.DataAnnotations;

namespace ReleaseSpence.Models
{
	[MetadataType(typeof(SensoresMD))]
	public partial class Sensores
	{
        public Sensores(SensoresViewModel sensorVM)
        {
            this.idSensor = sensorVM.idSensor;
            this.idTipo = sensorVM.idTipo;
            this.idFigura = sensorVM.idFigura;
            this.idPuntoMonitoreo = sensorVM.idPuntoMonitoreo;
            this.nombre = sensorVM.nombre;
            this.minLimit = sensorVM.minLimit;
            this.maxLimit = sensorVM.maxLimit;
            this.Sensores_Piezometros = new Sensores_Piezometros();
            this.Sensores_Piezometros.seriePiezo = sensorVM.Sensores_Piezometros.seriePiezo;
            this.Sensores_Piezometros.profundidadPozo = sensorVM.Sensores_Piezometros.profundidadPozo;
            this.Sensores_Piezometros.cotaTierra = sensorVM.Sensores_Piezometros.cotaTierra;
            this.Sensores_Piezometros.cotaAgua = sensorVM.Sensores_Piezometros.cotaAgua;
            this.Sensores_Piezometros.distT_A = sensorVM.Sensores_Piezometros.distT_A;
            this.Sensores_Piezometros.cotaSensor = sensorVM.Sensores_Piezometros.cotaSensor;
            this.Sensores_Piezometros.metrosSensor = sensorVM.Sensores_Piezometros.metrosSensor;
            this.Sensores_Piezometros.carpeta = sensorVM.Sensores_Piezometros.carpeta;
            if (!string.IsNullOrEmpty(sensorVM.Sensores_Piezometros.coefA)) this.Sensores_Piezometros.coefA = float.Parse(sensorVM.Sensores_Piezometros.coefA, System.Globalization.NumberStyles.Float);
            this.Sensores_Piezometros.coefB = sensorVM.Sensores_Piezometros.coefB;
            this.Sensores_Piezometros.coefC = sensorVM.Sensores_Piezometros.coefC;
            if (!string.IsNullOrEmpty(sensorVM.Sensores_Piezometros.tempK)) this.Sensores_Piezometros.tempK = float.Parse(sensorVM.Sensores_Piezometros.tempK, System.Globalization.NumberStyles.Float);
            this.Sensores_Piezometros.tempI = sensorVM.Sensores_Piezometros.tempI;
            this.Sensores_Piezometros.baroI = sensorVM.Sensores_Piezometros.baroI;
            this.Sensores_Piezometros.freqRead = sensorVM.Sensores_Piezometros.freqRead;
            this.Sensores_Piezometros.freqSend = sensorVM.Sensores_Piezometros.freqSend;
        }
    }

	public class SensoresMD
	{
		[Display(Name = "Id Sensor")]
		public int idSensor { get; set; }

		[Display(Name = "Tipo de Sensor")]
        public int idTipo { get; set; }

        [Display(Name = "Figura / Ícono en Plano")]
        public int idFigura { get; set; }

        [Display(Name = "Punto de Monitoreo")]
        public int idPuntoMonitoreo { get; set; }

        [Display(Name = "Nombre del Sensor")]
		[Required]
		[StringLength(128, MinimumLength = 1, ErrorMessage = "El nombre es demasiado largo")]
		[Alfanumerico]
		public string nombre { get; set; }

		[Display(Name = "Umbral de alerta Superior [MSNM]")]
        [RegularExpression(Validacion.numero, ErrorMessage = Validacion.numeroError)]
        public float? maxLimit { get; set; }

		[Display(Name = "Umbral de alerta Inferior [MSNM]")]
        [RegularExpression(Validacion.numero, ErrorMessage = Validacion.numeroError)]
        public float? minLimit { get; set; }
	}

    public class SensoresViewModel : Sensores
    {
        public SensoresViewModel() { }
        public SensoresViewModel(Sensores sensor)
        {
            this.idSensor = sensor.idSensor;
            this.idTipo = sensor.idTipo;
            this.idFigura = sensor.idFigura;
            this.idPuntoMonitoreo = sensor.idPuntoMonitoreo;
            this.nombre = sensor.nombre;
            this.minLimit = sensor.minLimit;
            this.maxLimit = sensor.maxLimit;
            this.Sensores_Piezometros = new Sensores_PiezometrosViewModel();
            this.Sensores_Piezometros.seriePiezo = sensor.Sensores_Piezometros.seriePiezo;
            this.Sensores_Piezometros.profundidadPozo = sensor.Sensores_Piezometros.profundidadPozo;
            this.Sensores_Piezometros.cotaTierra = sensor.Sensores_Piezometros.cotaTierra;
            this.Sensores_Piezometros.cotaAgua = sensor.Sensores_Piezometros.cotaAgua;
            this.Sensores_Piezometros.distT_A = sensor.Sensores_Piezometros.distT_A;
            this.Sensores_Piezometros.cotaSensor = sensor.Sensores_Piezometros.cotaSensor;
            this.Sensores_Piezometros.metrosSensor = sensor.Sensores_Piezometros.metrosSensor;
            this.Sensores_Piezometros.carpeta = sensor.Sensores_Piezometros.carpeta;
            this.Sensores_Piezometros.coefA = sensor.Sensores_Piezometros.coefA.ToString();
            this.Sensores_Piezometros.coefB = sensor.Sensores_Piezometros.coefB;
            this.Sensores_Piezometros.coefC = sensor.Sensores_Piezometros.coefC;
            this.Sensores_Piezometros.tempK = sensor.Sensores_Piezometros.tempK.ToString();
            this.Sensores_Piezometros.tempI = sensor.Sensores_Piezometros.tempI;
            this.Sensores_Piezometros.baroI = sensor.Sensores_Piezometros.baroI;
            this.Sensores_Piezometros.freqRead = sensor.Sensores_Piezometros.freqRead;
            this.Sensores_Piezometros.freqSend = sensor.Sensores_Piezometros.freqSend;
        }
        public new Sensores_PiezometrosViewModel Sensores_Piezometros { get; set; }
    }
}