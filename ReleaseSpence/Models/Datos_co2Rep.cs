using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ReleaseSpence.Models
{
	public class Datos_co2Rep
	{
		private static MonitoreoIntegradoEntities db = new MonitoreoIntegradoEntities();

		public static void Create(Datos_co2 dato_co2)
		{
			SqlConnection con = db.Database.Connection as SqlConnection;
			SqlCommand cmd = new SqlCommand("Datos_co2_Create", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@idSensor", dato_co2.idSensor);
			cmd.Parameters.AddWithValue("@fecha", dato_co2.fecha);
			cmd.Parameters.AddWithValue("@dato", dato_co2.dato);
			con.Open();
			cmd.ExecuteNonQuery();
			con.Close();
		}

		public static List<Datos_co2> Graphics(int idSensor, bool precision, int cantidad_datos, DateTime desde, DateTime hasta)
		{
			List<Datos_co2> datos = new List<Datos_co2>();
			SqlConnection con = db.Database.Connection as SqlConnection;
			SqlCommand cmd = new SqlCommand("Datos_co2_Graphics", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@idSensor", idSensor);
			cmd.Parameters.AddWithValue("@precision", precision);
			cmd.Parameters.AddWithValue("@datos", cantidad_datos);
			cmd.Parameters.AddWithValue("@desde", desde);
			cmd.Parameters.AddWithValue("@hasta", hasta);
			con.Open();
			SqlDataReader lector = cmd.ExecuteReader();
			while(lector.Read())
			{
				Datos_co2 dato = new Datos_co2();
				dato.fecha = (DateTime)lector["fecha"];
				dato.dato = (float)lector["dato"];
				datos.Add(dato);
			}
			con.Close();
			return datos;
		}
	}
}