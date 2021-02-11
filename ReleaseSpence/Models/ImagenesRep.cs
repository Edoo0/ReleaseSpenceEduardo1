using System;
using System.Data;
using System.Data.SqlClient;

namespace ReleaseSpence.Models
{
	public class ImagenesRep
	{
		private static MonitoreoIntegradoEntities db = new MonitoreoIntegradoEntities();

		public static int Create(Imagenes imagenes)
		{
			int respuesta = -1;
			SqlConnection con = db.Database.Connection as SqlConnection;
			SqlCommand cmd = new SqlCommand("Imagenes_Create", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@nombre", imagenes.nombre);
            try
            {
                con.Open();
                respuesta = (int)cmd.ExecuteScalar();
                con.Close();
            }
            catch
            {
                con.Close();
            }
			return respuesta;
		}

		public static void Update(Imagenes imagenes)
		{
			SqlConnection con = db.Database.Connection as SqlConnection;
			SqlCommand cmd = new SqlCommand("Imagenes_Update", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@idImagen", imagenes.idImagen);
			cmd.Parameters.AddWithValue("@nombre", imagenes.nombre);
			con.Open();
			cmd.ExecuteNonQuery();
			con.Close();
		}

		public static void Georeferenciar(Georeferenciar imagenes)
		{
			SqlConnection con = db.Database.Connection as SqlConnection;
			SqlCommand cmd = new SqlCommand("Imagenes_Georeferenciar", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@idImagen", imagenes.idImagen);
			cmd.Parameters.AddWithValue("@mx", imagenes.mx);
			cmd.Parameters.AddWithValue("@nx", imagenes.nx);
			cmd.Parameters.AddWithValue("@my", imagenes.my);
			cmd.Parameters.AddWithValue("@ny", imagenes.ny);
			con.Open();
			cmd.ExecuteNonQuery();
			con.Close();
		}
	}
}