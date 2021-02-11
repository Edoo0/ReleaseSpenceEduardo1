namespace ReleaseSpence
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Sensores_Piezometros
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idSensor { get; set; }

        [StringLength(64)]
        public string seriePiezo { get; set; }

        public float? profundidadPozo { get; set; }

        public float? cotaTierra { get; set; }

        public float? distT_A { get; set; }

        public float? cotaAgua { get; set; }

        public float? cotaSensor { get; set; }

        public float? metrosSensor { get; set; }

        public float? carpeta { get; set; }

        public float? coefA { get; set; }

        public float? coefB { get; set; }

        public float? coefC { get; set; }

        public float? tempK { get; set; }

        public float? tempI { get; set; }

        public float? baroI { get; set; }

        public int? freqRead { get; set; }

        public int? freqSend { get; set; }
    }
}
