using System;
using System.Data;
using System.Data.SqlClient;

namespace ReleaseSpence.Models
{
	public class Sensores_PiezometrosRep
	{
		private static MonitoreoIntegradoEntities db = new MonitoreoIntegradoEntities();

		public static int Create(Sensores sensores)
		{
            int respuesta = -1;
			SqlConnection con = db.Database.Connection as SqlConnection;
			SqlCommand cmd = new SqlCommand("SensoresPiezometros_Create", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@idTipo", sensores.idTipo);
            cmd.Parameters.AddWithValue("@idFigura", sensores.idFigura);
            cmd.Parameters.AddWithValue("@idPuntoMonitoreo", sensores.idPuntoMonitoreo);
            cmd.Parameters.AddWithValue("@nombre", sensores.nombre);
			cmd.Parameters.AddWithValue("@maxLimit", sensores.maxLimit == null ? (object)DBNull.Value : sensores.maxLimit);
            cmd.Parameters.AddWithValue("@minLimit", sensores.minLimit == null ? (object)DBNull.Value : sensores.minLimit);
            cmd.Parameters.AddWithValue("@seriePiezo", sensores.Sensores_Piezometros.seriePiezo == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.seriePiezo);
            cmd.Parameters.AddWithValue("@profundidadPozo", sensores.Sensores_Piezometros.profundidadPozo == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.profundidadPozo);
            cmd.Parameters.AddWithValue("@cotaTierra", sensores.Sensores_Piezometros.cotaTierra == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.cotaTierra);
            cmd.Parameters.AddWithValue("@distT_A", sensores.Sensores_Piezometros.distT_A == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.distT_A);
            cmd.Parameters.AddWithValue("@cotaAgua", sensores.Sensores_Piezometros.cotaAgua == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.cotaAgua);
            cmd.Parameters.AddWithValue("@cotaSensor", sensores.Sensores_Piezometros.cotaSensor == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.cotaSensor);
            cmd.Parameters.AddWithValue("@metrosSensor", sensores.Sensores_Piezometros.metrosSensor == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.metrosSensor);
            cmd.Parameters.AddWithValue("@carpeta", sensores.Sensores_Piezometros.carpeta == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.carpeta);
            cmd.Parameters.AddWithValue("@coefA", sensores.Sensores_Piezometros.coefA == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.coefA);
            cmd.Parameters.AddWithValue("@coefB", sensores.Sensores_Piezometros.coefB == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.coefB);
            cmd.Parameters.AddWithValue("@coefC", sensores.Sensores_Piezometros.coefC == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.coefC);
            cmd.Parameters.AddWithValue("@tempK", sensores.Sensores_Piezometros.tempK == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.tempK);
            cmd.Parameters.AddWithValue("@tempI", sensores.Sensores_Piezometros.tempI == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.tempI);
            cmd.Parameters.AddWithValue("@baroI", sensores.Sensores_Piezometros.baroI == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.baroI);
            cmd.Parameters.AddWithValue("@freqRead", sensores.Sensores_Piezometros.freqRead == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.freqRead);
            cmd.Parameters.AddWithValue("@freqSend", sensores.Sensores_Piezometros.freqSend == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.freqSend);
            

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
            SqlCommand cmd = new SqlCommand("SensoresPiezometros_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idSensor", sensores.idSensor);
            cmd.Parameters.AddWithValue("@idFigura", sensores.idFigura);
            cmd.Parameters.AddWithValue("@nombre", sensores.nombre);
            cmd.Parameters.AddWithValue("@maxLimit", sensores.maxLimit == null ? (object)DBNull.Value : sensores.maxLimit);
            cmd.Parameters.AddWithValue("@minLimit", sensores.minLimit == null ? (object)DBNull.Value : sensores.minLimit);
            cmd.Parameters.AddWithValue("@seriePiezo", sensores.Sensores_Piezometros.seriePiezo == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.seriePiezo);
            cmd.Parameters.AddWithValue("@profundidadPozo", sensores.Sensores_Piezometros.profundidadPozo == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.profundidadPozo);
            cmd.Parameters.AddWithValue("@cotaTierra", sensores.Sensores_Piezometros.cotaTierra == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.cotaTierra);
            cmd.Parameters.AddWithValue("@distT_A", sensores.Sensores_Piezometros.distT_A == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.distT_A);
            cmd.Parameters.AddWithValue("@cotaAgua", sensores.Sensores_Piezometros.cotaAgua == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.cotaAgua);
            cmd.Parameters.AddWithValue("@cotaSensor", sensores.Sensores_Piezometros.cotaSensor == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.cotaSensor);
            cmd.Parameters.AddWithValue("@metrosSensor", sensores.Sensores_Piezometros.metrosSensor == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.metrosSensor);      
            cmd.Parameters.AddWithValue("@carpeta", sensores.Sensores_Piezometros.carpeta == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.carpeta);
            cmd.Parameters.AddWithValue("@coefA", sensores.Sensores_Piezometros.coefA == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.coefA);
            cmd.Parameters.AddWithValue("@coefB", sensores.Sensores_Piezometros.coefB == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.coefB);
            cmd.Parameters.AddWithValue("@coefC", sensores.Sensores_Piezometros.coefC == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.coefC);
            cmd.Parameters.AddWithValue("@tempK", sensores.Sensores_Piezometros.tempK == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.tempK);
            cmd.Parameters.AddWithValue("@tempI", sensores.Sensores_Piezometros.tempI == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.tempI);
            cmd.Parameters.AddWithValue("@baroI", sensores.Sensores_Piezometros.baroI == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.baroI);
            cmd.Parameters.AddWithValue("@freqRead", sensores.Sensores_Piezometros.freqRead == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.freqRead);
            cmd.Parameters.AddWithValue("@freqSend", sensores.Sensores_Piezometros.freqSend == null ? (object)DBNull.Value : sensores.Sensores_Piezometros.freqSend);
            
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch
            {
                con.Close();
            }
        }
    }
}