namespace ReleaseSpence
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Imagenes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Imagenes()
        {
            Imagen_TipoSensor = new HashSet<Imagen_TipoSensor>();
            Marcadores = new HashSet<Marcadores>();
        }

        [Key]
        public int idImagen { get; set; }

        [Required]
        [StringLength(64)]
        public string nombre { get; set; }

        public float? mx { get; set; }

        public float? nx { get; set; }

        public float? my { get; set; }

        public float? ny { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Imagen_TipoSensor> Imagen_TipoSensor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Marcadores> Marcadores { get; set; }
    }
}
