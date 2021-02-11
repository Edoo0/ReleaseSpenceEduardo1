namespace ReleaseSpence
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Punto_de_Monitoreo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Punto_de_Monitoreo()
        {
            Sensores = new HashSet<Sensores>();
        }

        [Key]
        public int idPuntoMonitoreo { get; set; }

        public float? carpeta { get; set; }

        public float? cotaTierra { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sensores> Sensores { get; set; }
    }
}
