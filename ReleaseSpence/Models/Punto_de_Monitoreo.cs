//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ReleaseSpence.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Punto_de_Monitoreo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Punto_de_Monitoreo()
        {
            this.Sensores = new HashSet<Sensores>();
        }
    
        public int idPuntoMonitoreo { get; set; }
        public int idFigura { get; set; }
        public Nullable<float> carpeta { get; set; }
        public Nullable<float> cotaTierra { get; set; }
        public string nombre { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sensores> Sensores { get; set; }
        public virtual Figuras Figuras { get; set; }
        public virtual MarcadoresMpz MarcadoresMpz { get; set; }
    }
}
