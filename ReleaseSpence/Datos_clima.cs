namespace ReleaseSpence
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Datos_clima
    {
        [Key]
        public int idDato { get; set; }

        public int idSensor { get; set; }

        public DateTime fecha { get; set; }

        public float? tempInt { get; set; }

        public float? tempExt { get; set; }

        public float? dewpoint { get; set; }

        public byte? humedadInt { get; set; }

        public byte? humedadExt { get; set; }

        public float? vientoVel { get; set; }

        public short? vientoGrados { get; set; }

        [StringLength(4)]
        public string vientoDir { get; set; }

        public float? sensacionTerm { get; set; }

        public float? lluviaH { get; set; }

        public float? lluviaD { get; set; }

        public float? lluviaT { get; set; }

        public float? presion { get; set; }

        [StringLength(16)]
        public string tendencia { get; set; }

        [StringLength(16)]
        public string pronostico { get; set; }

        public virtual Sensores Sensores { get; set; }
    }
}
