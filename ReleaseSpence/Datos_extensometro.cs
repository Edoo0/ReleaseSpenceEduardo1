namespace ReleaseSpence
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Datos_extensometro
    {
        [Key]
        public int idDato { get; set; }

        public int idSensor { get; set; }

        public DateTime fecha { get; set; }

        public float dato { get; set; }

        public virtual Sensores Sensores { get; set; }
    }
}
