using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ReleaseSpence.Models
{
	public class Datos_pm10Rep
	{
		private static MonitoreoIntegradoEntities db = new MonitoreoIntegradoEntities();

		public static void Create(Datos_pm10 dato_pm10)
		{
			SqlConnection con = db.Database.Connection as SqlConnection;
			SqlCommand cmd = new SqlCommand("Datos_pm10_Create", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@idSensor", dato_pm10.idSensor);
			cmd.Parameters.AddWithValue("@fecha", dato_pm10.fecha);
			cmd.Parameters.AddWithValue("@dato", dato_pm10.dato);
			con.Open();
			cmd.ExecuteNonQuery();
			con.Close();
		}

		public static List<Datos_pm10> Graphics(int idSensor, int multipler, int cantidad_datos)
		{
			List<Datos_pm10> datos = new List<Datos_pm10>();
			SqlConnection con = db.Database.Connection as SqlConnection;
			SqlCommand cmd = new SqlCommand("Datos_pm10_Graphics", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@idSensor", idSensor);
			cmd.Parameters.AddWithValue("@multipler", multipler);
			cmd.Parameters.AddWithValue("@datos", cantidad_datos);
			con.Open();
			SqlDataReader lector = cmd.ExecuteReader();
			while(lector.Read())
			{
				Datos_pm10 dato = new Datos_pm10();
				dato.fecha = (DateTime)lector["fecha"];
				dato.dato = (float)lector["dato"];
				datos.Add(dato);
			}
			con.Close();
			return datos;
		}
	}
}