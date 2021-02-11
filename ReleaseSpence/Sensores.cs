namespace ReleaseSpence
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Sensores
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sensores()
        {
            Datos_clima = new HashSet<Datos_clima>();
            Datos_co2 = new HashSet<Datos_co2>();
            Datos_energia = new HashSet<Datos_energia>();
            Datos_extensometro = new HashSet<Datos_extensometro>();
            Datos_o2 = new HashSet<Datos_o2>();
            Datos_pm10 = new HashSet<Datos_pm10>();
        }

        [Key]
        public int idSensor { get; set; }

        public int idTipo { get; set; }

        public int idFigura { get; set; }

        public int idPuntoMonitoreo { get; set; }

        [Required]
        [StringLength(128)]
        public string nombre { get; set; }

        public float? maxLimit { get; set; }

        public float? minLimit { get; set; }

        [StringLength(8)]
        public string syncToken { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Datos_clima> Datos_clima { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Datos_co2> Datos_co2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Datos_energia> Datos_energia { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Datos_extensometro> Datos_extensometro { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Datos_o2> Datos_o2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Datos_pm10> Datos_pm10 { get; set; }

        public virtual Figuras Figuras { get; set; }

        public virtual Punto_de_Monitoreo Punto_de_Monitoreo { get; set; }

        public virtual TipoSensores TipoSensores { get; set; }
    }
}
