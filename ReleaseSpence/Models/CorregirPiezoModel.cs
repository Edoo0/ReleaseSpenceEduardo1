using System;
using System.ComponentModel.DataAnnotations;

namespace ReleaseSpence.Models
{
    public class CorregirPiezo
    {
        [Required]
        public int idSensor { get; set; }

        [Display(Name = "Nombre del Pozo")]
        public string nombre { get; set; }

        [Display(Name = "Número de Serie del Piezómetro")]
        public string seriePiezo { get; set; }

        [Display(Name = "Desde")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd'-'MM'-'yyyy' 'HH':'mm':'ss}", ApplyFormatInEditMode = true)]
        public DateTime desde { get; set; }

        [Display(Name = "Hasta")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd'-'MM'-'yyyy' 'HH':'mm':'ss}", ApplyFormatInEditMode = true)]
        public DateTime hasta { get; set; }

        [Display(Name = "Cota de Tierra [MSNM]")]
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        [RegularExpression(Validacion.numero, ErrorMessage = Validacion.numeroError)]
        public float? cotaTierra { get; set; }

        [Display(Name = "Medición del Pozómetro [mts]")]
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        [RegularExpression(Validacion.numero, ErrorMessage = Validacion.numeroError)]
        public float? distT_A { get; set; }

        [Display(Name = "Cota de Agua Inicial [MSNM]")]
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        [RegularExpression(Validacion.numero, ErrorMessage = Validacion.numeroError)]
        public float? cotaAgua { get; set; }

        [Display(Name = "Cota del Piezómetro [MSNM]")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        [RegularExpression(Validacion.numero, ErrorMessage = Validacion.numeroError)]
        public float cotaSensor { get; set; }

        [Display(Name = "Cota de Carpeta [MSNM]")]
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        [RegularExpression(Validacion.numero, ErrorMessage = Validacion.numeroError)]
        public float carpeta { get; set; }

        [Display(Name = "Factor Calibración Inicial")]
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        [RegularExpression(Validacion.numero, ErrorMessage = Validacion.numeroError)]
        public float? metrosSensor { get; set; }

        [Display(Name = "Coeficiente A")]
        [Required]
        [RegularExpression(Validacion.notacionCientifica, ErrorMessage = Validacion.notacionCientificaError)]
        [DisplayFormat(DataFormatString = "{0:F20}")]
        public string coefA { get; set; }

        [Display(Name = "Coeficiente B")]
        [Required]
        [RegularExpression(Validacion.numero, ErrorMessage = Validacion.numeroError)]
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        public float coefB { get; set; }

        [Display(Name = "Coeficiente C")]
        [Required]
        [RegularExpression(Validacion.numero, ErrorMessage = Validacion.numeroError)]
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        public float coefC { get; set; }

        [Display(Name = "Factor Corrección de Temperatura K [Tk]")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        [RegularExpression(Validacion.numeroOcientifica, ErrorMessage = Validacion.numeroOcientificaError)]
        public string tempK { get; set; }

        [Display(Name = "Temperatura Inicial del Piezómetro [°C]")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        [RegularExpression(Validacion.numero, ErrorMessage = Validacion.numeroError)]
        public float tempI { get; set; }

        [Display(Name = "Presion Barométrica Inicial [mbar]")]
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        [RegularExpression(Validacion.numero, ErrorMessage = Validacion.numeroError)]
        public float? baroI { get; set; }
        

        public CorregirPiezo() { }
        public CorregirPiezo(Sensores sensor)
        {
            this.idSensor = sensor.idSensor;
            this.nombre = sensor.nombre;
            this.seriePiezo = sensor.Sensores_Piezometros.seriePiezo;
            this.cotaTierra = sensor.Sensores_Piezometros.cotaTierra;
            this.cotaAgua = sensor.Sensores_Piezometros.cotaAgua;
            this.distT_A = sensor.Sensores_Piezometros.distT_A;
            this.cotaSensor = (float)sensor.Sensores_Piezometros.cotaSensor;
            this.metrosSensor = sensor.Sensores_Piezometros.metrosSensor;
            this.coefA = sensor.Sensores_Piezometros.coefA.ToString();
            this.coefB = (float)sensor.Sensores_Piezometros.coefB;
            this.coefC = (float)sensor.Sensores_Piezometros.coefC;
            this.tempK = sensor.Sensores_Piezometros.tempK.ToString();
            this.tempI = (float)sensor.Sensores_Piezometros.tempI;
            this.baroI = sensor.Sensores_Piezometros.baroI;
            
        }
    }
}