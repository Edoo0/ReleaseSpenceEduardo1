using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ReleaseSpence.Models
{
    [MetadataType(typeof(ImagenesMD))]
    public partial class Imagenes
    {
        public HttpPostedFileBase archivo { get; set; }
        public int[] idTipos { get; set; }
    }

    public class ImagenesMD
    {
        public int idImagen { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        [StringLength(64, MinimumLength = 1, ErrorMessage = "El nombre es demasiado largo")]
        [Alfanumerico]
        public string nombre { get; set; }

        [Display(Name = "Imagen")]
        [jpg]
        public HttpPostedFileBase archivo { get; set; }

		[DisplayFormat(DataFormatString = "{0:#.##}")]
		public float? mx { get; set; }

		[DisplayFormat(DataFormatString = "{0:#.##}")]
		public float? nx { get; set; }

		[DisplayFormat(DataFormatString = "{0:#.##}")]
		public float? my { get; set; }

		[DisplayFormat(DataFormatString = "{0:#.##}")]
		public float? ny { get; set; }
    }

    public class ImagenesCreate : Imagenes
    {
        [Required]
        public new HttpPostedFileBase archivo { get; set; }
    }
}