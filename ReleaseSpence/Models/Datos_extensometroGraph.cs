using System.ComponentModel.DataAnnotations;

namespace ReleaseSpence.Models
{
    public class Datos_extensometroGraph : Datos_extensometro
    {
        public double velocidad { get; set; } //velocidad en mm/dia
        public double aceleracion { get; set; } //aceleracion en mm/dia²
    }
}