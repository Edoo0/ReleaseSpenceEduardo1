namespace ReleaseSpence
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Marcadores
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idSensor { get; set; }

        public int idImagen { get; set; }

        public short x { get; set; }

        public short y { get; set; }

        public virtual Imagenes Imagenes { get; set; }
    }
}
