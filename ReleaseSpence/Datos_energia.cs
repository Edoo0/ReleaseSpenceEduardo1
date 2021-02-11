namespace ReleaseSpence
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Datos_energia
    {
        [Key]
        public int idDato { get; set; }

        public int idSensor { get; set; }

        public DateTime fecha { get; set; }

        public float? panelV { get; set; }

        public float? panelC { get; set; }

        public float? panelP { get; set; }

        public float? charC { get; set; }

        public float? charP { get; set; }

        public float? charE { get; set; }

        public float? charT { get; set; }

        public float? batV { get; set; }

        public float? batC { get; set; }

        public float? batP { get; set; }

        public float? batSOC { get; set; }

        public float? batCE { get; set; }

        public int? batTTG { get; set; }

        public float? inv_inC { get; set; }

        public float? inv_inP { get; set; }

        public float? inv_outC { get; set; }

        public float? inv_outP { get; set; }

        public float? invE { get; set; }

        public virtual Sensores Sensores { get; set; }
    }
}
