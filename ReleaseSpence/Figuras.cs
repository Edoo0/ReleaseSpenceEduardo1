namespace ReleaseSpence
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Figuras
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Figuras()
        {
            Sensores = new HashSet<Sensores>();
        }

        [Key]
        public int idFigura { get; set; }

        public short tipo { get; set; }

        [StringLength(128)]
        public string nombre { get; set; }

        public short size { get; set; }

        [StringLength(6)]
        public string color { get; set; }

        public short? borde { get; set; }

        [StringLength(6)]
        public string colorBorde { get; set; }

        public short? rotacion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sensores> Sensores { get; set; }
    }
}
