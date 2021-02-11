using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ReleaseSpence.Models
{
	public class Datos_extensometroRep
	{
		private static MonitoreoIntegradoEntities db = new MonitoreoIntegradoEntities();

		public static void Create(Datos_extensometro dato_extensometro)
		{
			SqlConnection con = db.Database.Connection as SqlConnection;
			SqlCommand cmd = new SqlCommand("Datos_extensometro_Create", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@idSensor", dato_extensometro.idSensor);
			cmd.Parameters.AddWithValue("@fecha", dato_extensometro.fecha);
			cmd.Parameters.AddWithValue("@dato", dato_extensometro.dato);
			con.Open();
			cmd.ExecuteNonQuery();
			con.Close();
		}

		public static List<Datos_extensometroGraph> Graphics(int idSensor, bool precision, int cantidad_datos, DateTime desde, DateTime hasta)
		{
			List<Datos_extensometroGraph> datos = new List<Datos_extensometroGraph>();
			SqlConnection con = db.Database.Connection as SqlConnection;
			SqlCommand cmd = new SqlCommand("Datos_extensometro_Graphics", con);
			cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idSensor", idSensor);
            cmd.Parameters.AddWithValue("@precision", precision);
            cmd.Parameters.AddWithValue("@datos", cantidad_datos);
            cmd.Parameters.AddWithValue("@desde", desde);
            cmd.Parameters.AddWithValue("@hasta", hasta);
            if (con.State != ConnectionState.Open) con.Open();
			SqlDataReader lector = cmd.ExecuteReader();
			while(lector.Read())
			{
				Datos_extensometroGraph dato = new Datos_extensometroGraph();
				dato.fecha = (DateTime)lector["fecha"];
				dato.dato = (float)lector["dato"];
                dato.velocidad = (float)lector["velocidad"];
                dato.aceleracion = (float)lector["aceleracion"];
                datos.Add(dato);
			}
			con.Close();
			return datos;
		}

        public static List<Datos_sensor_alarm> Alertas(int idImagen, int idTipo, DateTime desde, DateTime hasta)
        {
            List<Datos_sensor_alarm> datos = new List<Datos_sensor_alarm>();
            SqlConnection con = db.Database.Connection as SqlConnection;
            SqlCommand cmd = new SqlCommand("Datos_extensometro_Alertas", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idImagen", idImagen);
            cmd.Parameters.AddWithValue("@idTipo", idTipo);
            cmd.Parameters.AddWithValue("@desde", desde);
            cmd.Parameters.AddWithValue("@hasta", hasta);
            if (!(con.State == ConnectionState.Open)) con.Open();
            SqlDataReader lector = cmd.ExecuteReader();
            while (lector.Read())
            {
                Datos_sensor_alarm dato = new Datos_sensor_alarm();
                dato.idSensor = (int)lector["idSensor"];
                dato.nombre = (string)lector["nombre"];
                dato.cantidad = (int)lector["cantidad"];
                datos.Add(dato);
            }
            con.Close();
            return datos;
        }

        public static List<Datos_sensor_alarmList> AlertasDet(int idSensor, DateTime desde, DateTime hasta)
        {
            List<Datos_sensor_alarmList> datos = new List<Datos_sensor_alarmList>();
            SqlConnection con = db.Database.Connection as SqlConnection;
            SqlCommand cmd = new SqlCommand("Datos_extensometro_AlertasDet", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idSensor", idSensor);
            cmd.Parameters.AddWithValue("@desde", desde);
            cmd.Parameters.AddWithValue("@hasta", hasta);
            if (!(con.State == ConnectionState.Open)) con.Open();
            SqlDataReader lector = cmd.ExecuteReader();
            while (lector.Read())
            {
                Datos_sensor_alarmList dato = new Datos_sensor_alarmList();
                dato.idSensor = (int)lector["idSensor"];
                dato.dato = (float)lector["dato"];
                dato.fecha = (DateTime)lector["fecha"];
                datos.Add(dato);
            }
            con.Close();
            return datos;
        }
    }
}