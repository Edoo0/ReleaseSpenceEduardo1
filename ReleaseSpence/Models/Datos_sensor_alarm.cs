using System;
using System.ComponentModel.DataAnnotations;

namespace ReleaseSpence.Models
{
    public class Datos_sensor_alarm
    {
        public int idSensor { get; set; }

        public string nombre { get; set; }

        public int cantidad { get; set; }
    }
}