//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ReleaseSpence.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Datos_o2
    {
        public int idDato { get; set; }
        public int idSensor { get; set; }
        public System.DateTime fecha { get; set; }
        public float dato { get; set; }
    
        public virtual Sensores Sensores { get; set; }
    }
}
