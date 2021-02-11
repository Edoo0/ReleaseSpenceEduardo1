using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ReleaseSpence.Models
{
	public class Datos_energiaRep
	{
		private static MonitoreoIntegradoEntities db = new MonitoreoIntegradoEntities();

		public static void Create(Datos_energia dato_energia)
		{
			SqlConnection con = db.Database.Connection as SqlConnection;
			SqlCommand cmd = new SqlCommand("Datos_energia_Create", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@idSensor", dato_energia.idSensor);
			cmd.Parameters.AddWithValue("@fecha", dato_energia.fecha);
			cmd.Parameters.AddWithValue("@panelV", dato_energia.panelV);
			cmd.Parameters.AddWithValue("@panelC", dato_energia.panelC);
			cmd.Parameters.AddWithValue("@panelP", dato_energia.panelP);
			cmd.Parameters.AddWithValue("@charC", dato_energia.charC);
			cmd.Parameters.AddWithValue("@charP", dato_energia.charP);
			cmd.Parameters.AddWithValue("@charE", dato_energia.charE);
			cmd.Parameters.AddWithValue("@charT", dato_energia.charT);
			cmd.Parameters.AddWithValue("@batV", dato_energia.batV);
			cmd.Parameters.AddWithValue("@batC", dato_energia.batC);
			cmd.Parameters.AddWithValue("@batP", dato_energia.batP);
			cmd.Parameters.AddWithValue("@batSOC", dato_energia.batSOC);
			cmd.Parameters.AddWithValue("@batCE", dato_energia.batCE);
			cmd.Parameters.AddWithValue("@batTTG", dato_energia.batTTG);
			cmd.Parameters.AddWithValue("@inv_inC", dato_energia.inv_inC);
			cmd.Parameters.AddWithValue("@inv_inP", dato_energia.inv_inP);
			cmd.Parameters.AddWithValue("@inv_outC", dato_energia.inv_outC);
			cmd.Parameters.AddWithValue("@inv_outP", dato_energia.inv_outP);
			cmd.Parameters.AddWithValue("@invE", dato_energia.invE);
			if (!(con.State == ConnectionState.Open)) con.Open();
			try
			{
				cmd.ExecuteNonQuery();
				con.Close();
			}
			catch
			{
				con.Close();
			}
		}

		//grafico de baterias voltaje y corriente
		//grafico de mppt potencia de entrada y de salida
		//grafico inversor potencia de entrada y salida
		//grafico de potencia solar vs baterias entregada para consumo
		public static List<Datos_energia> GraphicsBat(int idSensor, bool precision, int cantidad_datos, DateTime desde, DateTime hasta)
		{
			List<Datos_energia> datos = new List<Datos_energia>();
			SqlConnection con = db.Database.Connection as SqlConnection;
			SqlCommand cmd = new SqlCommand("Datos_energia_GraphicsBat", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@idSensor", idSensor);
			cmd.Parameters.AddWithValue("@precision", precision);
			cmd.Parameters.AddWithValue("@datos", cantidad_datos);
			cmd.Parameters.AddWithValue("@desde", desde);
			cmd.Parameters.AddWithValue("@hasta", hasta);
			con.Open();
			SqlDataReader lector = cmd.ExecuteReader();
			while (lector.Read())
			{
				Datos_energia dato = new Datos_energia();
				dato.fecha = (DateTime)lector["fecha"];
				dato.batV = (float)lector["batV"];
				dato.batC = (float)lector["batC"];
				dato.charT = (float)lector["charT"];
				datos.Add(dato);
			}
			con.Close();
			return datos;
		}

		public static List<Datos_energia> GraphicsChar(int idSensor, bool precision, int cantidad_datos, DateTime desde, DateTime hasta)
		{
			List<Datos_energia> datos = new List<Datos_energia>();
			SqlConnection con = db.Database.Connection as SqlConnection;
			SqlCommand cmd = new SqlCommand("Datos_energia_GraphicsChar", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@idSensor", idSensor);
			cmd.Parameters.AddWithValue("@precision", precision);
			cmd.Parameters.AddWithValue("@datos", cantidad_datos);
			cmd.Parameters.AddWithValue("@desde", desde);
			cmd.Parameters.AddWithValue("@hasta", hasta);
			con.Open();
			SqlDataReader lector = cmd.ExecuteReader();
			while (lector.Read())
			{
				Datos_energia dato = new Datos_energia();
				dato.fecha = (DateTime)lector["fecha"];
				dato.panelP = (float)lector["panelP"];
				dato.charP = (float)lector["charP"];
				datos.Add(dato);
			}
			con.Close();
			return datos;
		}

		public static List<Datos_energia> GraphicsInv(int idSensor, bool precision, int cantidad_datos, DateTime desde, DateTime hasta)
		{
			List<Datos_energia> datos = new List<Datos_energia>();
			SqlConnection con = db.Database.Connection as SqlConnection;
			SqlCommand cmd = new SqlCommand("Datos_energia_GraphicsInv", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@idSensor", idSensor);
			cmd.Parameters.AddWithValue("@precision", precision);
			cmd.Parameters.AddWithValue("@datos", cantidad_datos);
			cmd.Parameters.AddWithValue("@desde", desde);
			cmd.Parameters.AddWithValue("@hasta", hasta);
			con.Open();
			SqlDataReader lector = cmd.ExecuteReader();
			while (lector.Read())
			{
				Datos_energia dato = new Datos_energia();
				dato.fecha = (DateTime)lector["fecha"];
				dato.inv_inP = (float)lector["inv_inP"];
				dato.inv_outP = (float)lector["inv_outP"];
				datos.Add(dato);
			}
			con.Close();
			return datos;
		}

		public static List<Datos_energia> GraphicsConsumo(int idSensor, bool precision, int cantidad_datos, DateTime desde, DateTime hasta)
		{
			List<Datos_energia> datos = new List<Datos_energia>();
			SqlConnection con = db.Database.Connection as SqlConnection;
			SqlCommand cmd = new SqlCommand("Datos_energia_GraphicsConsumo", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@idSensor", idSensor);
			cmd.Parameters.AddWithValue("@precision", precision);
			cmd.Parameters.AddWithValue("@datos", cantidad_datos);
			cmd.Parameters.AddWithValue("@desde", desde);
			cmd.Parameters.AddWithValue("@hasta", hasta);
			con.Open();
			SqlDataReader lector = cmd.ExecuteReader();
			while (lector.Read())
			{
				Datos_energia dato = new Datos_energia();
				dato.fecha = (DateTime)lector["fecha"];
				dato.charP = (float)lector["charP"];
				dato.inv_inP = (float)lector["inv_inP"];
				datos.Add(dato);
			}
			con.Close();
			return datos;
		}
	}
}