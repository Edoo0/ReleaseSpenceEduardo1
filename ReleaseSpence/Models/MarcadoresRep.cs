using System.Data;
using System.Data.SqlClient;

namespace ReleaseSpence.Models
{
	public class MarcadoresRep
	{
		private static MonitoreoIntegradoEntities db = new MonitoreoIntegradoEntities();

		public static void Create(Marcadores marcador)
		{
			SqlConnection con = db.Database.Connection as SqlConnection;
			SqlCommand cmd = new SqlCommand("Marcadores_Create", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@idSensor", marcador.idSensor);
			cmd.Parameters.AddWithValue("@idImagen", marcador.idImagen);
			cmd.Parameters.AddWithValue("@x", marcador.x);
			cmd.Parameters.AddWithValue("@y", marcador.y);
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

		public static void Delete(Marcadores marcador)
		{
			SqlConnection con = db.Database.Connection as SqlConnection;
			SqlCommand cmd = new SqlCommand("Marcadores_Delete", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@idSensor", marcador.idSensor);
			con.Open();
			cmd.ExecuteNonQuery();
			con.Close();
		}
	}
}