using System;
using System.ComponentModel.DataAnnotations;

namespace ReleaseSpence.Models
{
    public class Datos_sensor_alarmList
    {
        public int idSensor { get; set; }

        public float dato { get; set; }

        public DateTime fecha { get; set; }
    }
}