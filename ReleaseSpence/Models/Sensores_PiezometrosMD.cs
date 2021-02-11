using System.ComponentModel.DataAnnotations;

namespace ReleaseSpence.Models
{
    [MetadataType(typeof(Sensores_PiezometrosMD))]
    public partial class Sensores_Piezometros
    {
    }

    public class Sensores_PiezometrosMD
    {
        [Display(Name = "Número de Serie del Piezómetro")]
        [StringLength(64, MinimumLength = 1, ErrorMessage = "El numero de serie es demasiado largo")]
        [Alfanumerico]
        public string seriePiezo { get; set; }

        [Display(Name = "Profundidad del Pozo [mts]")]
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        [RegularExpression(Validacion.numero, ErrorMessage = Validacion.numeroError)]
        public float? profundidadPozo { get; set; }

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
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        [RegularExpression(Validacion.numero, ErrorMessage = Validacion.numeroError)]
        public float? cotaSensor { get; set; }

        [Display(Name = "Factor Calibración Inicial")]
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        [RegularExpression(Validacion.numero, ErrorMessage = Validacion.numeroError)]
        public float? metrosSensor { get; set; }

        [Display(Name = "Cota de Geomembrana [MSNM]")]
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        [RegularExpression(Validacion.numero, ErrorMessage = Validacion.numeroError)]
        public float? carpeta { get; set; }        

        [Display(Name = "Coeficiente A")]
        [RegularExpression(Validacion.notacionCientifica, ErrorMessage = Validacion.notacionCientificaError)]
        [DisplayFormat(DataFormatString = "{0:F20}")]
        public float? coefA { get; set; }

        [Display(Name = "Coeficiente B")]
        [RegularExpression(Validacion.numero, ErrorMessage = Validacion.numeroError)]
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        public float? coefB { get; set; }

        [Display(Name = "Coeficiente C")]
        [RegularExpression(Validacion.numero, ErrorMessage = Validacion.numeroError)]
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        public float? coefC { get; set; }

        [Display(Name = "Factor Corrección de Temperatura K [Tk]")]
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        [RegularExpression(Validacion.numeroOcientifica, ErrorMessage = Validacion.numeroOcientificaError)]
        public float? tempK { get; set; }

        [Display(Name = "Temperatura Inicial del Piezómetro [°C]")]
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        [RegularExpression(Validacion.numero, ErrorMessage = Validacion.numeroError)]
        public float? tempI { get; set; }

        [Display(Name = "Presion barométrica Inicial [mbar]")]
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        [RegularExpression(Validacion.numero, ErrorMessage = Validacion.numeroError)]
        public float? baroI { get; set; }

        [Display(Name = "Frecuencia de Lectura [min]")]
        [Range(1, 1440)]
        public int? freqRead { get; set; }

        [Display(Name = "Frecuencia de Envío [hrs]")]
        [Range(1, 24)]
        public int? freqSend { get; set; }
    }

    public class Sensores_PiezometrosViewModel : Sensores_Piezometros
    {
        [Display(Name = "Coeficiente A")]
        [RegularExpression(Validacion.notacionCientifica, ErrorMessage = Validacion.notacionCientificaError)]
        [DisplayFormat(DataFormatString = "{0:F20}")]
        public new string coefA { get; set; }

        [Display(Name = "Factor Corrección de Temperatura K [Tk]")]
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        [RegularExpression(Validacion.numeroOcientifica, ErrorMessage = Validacion.numeroOcientificaError)]
        public new string tempK { get; set; }

    }
}