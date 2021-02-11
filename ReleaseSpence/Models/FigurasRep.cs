using System;
using System.Data;
using System.Data.SqlClient;

namespace ReleaseSpence.Models
{
	public class FigurasRep
	{
		private static MonitoreoIntegradoEntities db = new MonitoreoIntegradoEntities();

		public static int Create(Figuras figuras)
		{
			int respuesta;
			SqlConnection con = db.Database.Connection as SqlConnection;
			SqlCommand cmd = new SqlCommand("Figuras_Create", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@tipo", figuras.tipo);
			cmd.Parameters.AddWithValue("@nombre", figuras.nombre);
			cmd.Parameters.AddWithValue("@size", figuras.size);
			cmd.Parameters.AddWithValue("@color", figuras.color.Replace("#", ""));
			cmd.Parameters.AddWithValue("@borde", (object)figuras.borde ?? DBNull.Value);
			cmd.Parameters.AddWithValue("@colorBorde", String.IsNullOrEmpty(figuras.colorBorde) ? (object)DBNull.Value : figuras.colorBorde.Replace("#", ""));
			cmd.Parameters.AddWithValue("@rotacion", (object)figuras.rotacion ?? DBNull.Value);
			con.Open();
			respuesta = (int)cmd.ExecuteScalar();
			con.Close();
			return respuesta;
		}

		public static void Update(Figuras figuras)
		{
			SqlConnection con = db.Database.Connection as SqlConnection;
			SqlCommand cmd = new SqlCommand("Figuras_Update", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@idFigura", figuras.idFigura);
			cmd.Parameters.AddWithValue("@tipo", figuras.tipo);
			cmd.Parameters.AddWithValue("@nombre", figuras.nombre);
			cmd.Parameters.AddWithValue("@size", figuras.size);
			cmd.Parameters.AddWithValue("@color", figuras.color.Replace("#", ""));
			cmd.Parameters.AddWithValue("@borde", (object)figuras.borde ?? DBNull.Value);
			cmd.Parameters.AddWithValue("@colorBorde", String.IsNullOrEmpty(figuras.colorBorde) ? (object)DBNull.Value : figuras.colorBorde.Replace("#", ""));
			cmd.Parameters.AddWithValue("@rotacion", (object)figuras.rotacion ?? DBNull.Value);
			con.Open();
			cmd.ExecuteNonQuery();
			con.Close();
		}
	}
}