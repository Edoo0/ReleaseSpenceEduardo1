using System.ComponentModel.DataAnnotations;

namespace ReleaseSpence.Models
{
    [MetadataType(typeof(PuntoMonitoreoMD))]
    public partial class Punto_de_Monitoreo
    {
        public Punto_de_Monitoreo(PuntoMonitoreoViewModel PuntoMonitoreoVM)
        {
            this.idPuntoMonitoreo = PuntoMonitoreoVM.idPuntoMonitoreo;
            this.idFigura = PuntoMonitoreoVM.idFigura;
            this.nombre = PuntoMonitoreoVM.nombre;
            this.carpeta = PuntoMonitoreoVM.carpeta;
            this.cotaTierra = PuntoMonitoreoVM.cotaTierra; 
        }
    }

    public class PuntoMonitoreoMD
    {
        [Display(Name = "ID")]
        public int idPuntoMonitoreo { get; set; }

        [Display(Name = "Figura / Ícono en Plano")]
        public int idFigura { get; set; }

        [Display(Name = "Nombre del Punto de Monitoreo")]
        [Required]
        [StringLength(128, MinimumLength = 1, ErrorMessage = "El nombre es demasiado largo")]
        [Alfanumerico]
        [RegularExpression(@"^[^<>.,?;:'()!~%\-_@#/*""\s]+$", ErrorMessage = "No se permiten los caracteres especiales @^[^<>.,?;:'()!~%-_@#/*]+$")]
        public string nombre { get; set; }

        [Display(Name = "Cota de Geomembrana [MSNM]")]
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        [Required(ErrorMessage = "Campo requerido.")]
        [RegularExpression(Validacion.numero, ErrorMessage = Validacion.numeroError)]
        public float? carpeta { get; set; }

        [Display(Name = "Cota de Tierra [MSNM]")]
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        [Required(ErrorMessage = "Campo requerido.")]
        [RegularExpression(Validacion.numero, ErrorMessage = Validacion.numeroError)]
        public float? cotaTierra { get; set; }
    }

    public class PuntoMonitoreoViewModel : Punto_de_Monitoreo
    {
        public PuntoMonitoreoViewModel() { }

        public PuntoMonitoreoViewModel(Punto_de_Monitoreo puntoMonitoreo)
        {            
            this.idPuntoMonitoreo = puntoMonitoreo.idPuntoMonitoreo;
            this.idFigura = puntoMonitoreo.idFigura;
            this.nombre = puntoMonitoreo.nombre;
            this.carpeta = puntoMonitoreo.carpeta;
            this.cotaTierra = puntoMonitoreo.cotaTierra;
        }
    }    
}