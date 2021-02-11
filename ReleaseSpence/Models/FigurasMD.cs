using System;
using System.ComponentModel.DataAnnotations;

namespace ReleaseSpence.Models
{
    [MetadataType(typeof(FigurasMD))]
    public partial class Figuras
    {
    }

    public class FigurasMD
    {
        public int idFigura { get; set; }
        [Display(Name = "Tipo")]

        [Required]
        public short tipo { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        [StringLength(128, MinimumLength = 1, ErrorMessage = "El nombre es demasiado largo")]
        [Alfanumerico]
        public string nombre { get; set; }

        [Display(Name = "Tamaño")]
        [Required]
        [Range(10, 1000)]
        public short size { get; set; }

        [Display(Name = "Color")]
        [Required]
        public string color { get; set; }

        [Display(Name = "Grosor Borde")]
        [Range(0, 500)]
        public Nullable<short> borde { get; set; }

        [Display(Name = "Color de Borde")]
        public string colorBorde { get; set; }

        [Display(Name = "Rotacion (grados)")]
        [Range(0, 360)]
        public Nullable<short> rotacion { get; set; }
    }
}