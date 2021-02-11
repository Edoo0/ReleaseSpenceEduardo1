using System;
using System.Data;
using System.Data.SqlClient;

namespace ReleaseSpence.Models
{
	public class SensoresRep
	{
		private static MonitoreoIntegradoEntities db = new MonitoreoIntegradoEntities();

		public static int Create(Sensores sensores)
		{
			int respuesta = -1;
			SqlConnection con = db.Database.Connection as SqlConnection;
			SqlCommand cmd = new SqlCommand("Sensores_Create", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@idTipo", sensores.idTipo);
            cmd.Parameters.AddWithValue("@idFigura", sensores.idFigura);
            cmd.Parameters.AddWithValue("@idPuntoMonitoreo", sensores.idPuntoMonitoreo);
            cmd.Parameters.AddWithValue("@nombre", sensores.nombre);
			cmd.Parameters.AddWithValue("@maxLimit", sensores.maxLimit == null ? (object)DBNull.Value : sensores.maxLimit);
			cmd.Parameters.AddWithValue("@minLimit", sensores.minLimit == null ? (object)DBNull.Value : sensores.minLimit);

            con.Open();
            try
            {
                respuesta = (int)cmd.ExecuteScalar();
            }
            catch { }
            con.Close();
            return respuesta;
        }

		public static void Update(Sensores sensores)
		{
			SqlConnection con = db.Database.Connection as SqlConnection;
			SqlCommand cmd = new SqlCommand("Sensores_Update", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@idSensor", sensores.idSensor);
            cmd.Parameters.AddWithValue("@idFigura", sensores.idFigura);
            cmd.Parameters.AddWithValue("@idPuntoMonitoreo", sensores.idPuntoMonitoreo);
            cmd.Parameters.AddWithValue("@nombre", sensores.nombre);
			cmd.Parameters.AddWithValue("@maxLimit", sensores.maxLimit == null ? (object)DBNull.Value : sensores.maxLimit);
			cmd.Parameters.AddWithValue("@minLimit", sensores.minLimit == null ? (object)DBNull.Value : sensores.minLimit);
			con.Open();
			cmd.ExecuteNonQuery();
			con.Close();
		}
	}
}