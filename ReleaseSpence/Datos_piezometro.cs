namespace ReleaseSpence
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Datos_piezometro
    {
        [Key]
        public int idDato { get; set; }

        public int idSensor { get; set; }

        public DateTime fecha { get; set; }

        public float cotaAgua { get; set; }

        public float metrosSensor { get; set; }

        public float temperatura_pz { get; set; }

        public float bUnits { get; set; }

        public float presion_pz { get; set; }

        public float? presion_bmp { get; set; }

        public float? temperatura_bmp { get; set; }
    }
}
