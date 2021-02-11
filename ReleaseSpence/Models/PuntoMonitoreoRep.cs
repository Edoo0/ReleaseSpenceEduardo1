using System;
using System.Data;
using System.Data.SqlClient;

namespace ReleaseSpence.Models
{
    public class PuntoMonitoreoRep
    {
        private static MonitoreoIntegradoEntities db = new MonitoreoIntegradoEntities();

        public static int Create(Punto_de_Monitoreo puntoMonitoreo)
        {
            int respuesta = -1;
            SqlConnection con = db.Database.Connection as SqlConnection;
            SqlCommand cmd = new SqlCommand("PuntoMonitoreo_Create", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idFigura", puntoMonitoreo.idFigura);
            cmd.Parameters.AddWithValue("@nombre", puntoMonitoreo.nombre);
            cmd.Parameters.AddWithValue("@carpeta", puntoMonitoreo.carpeta);
            cmd.Parameters.AddWithValue("@cotaTierra", puntoMonitoreo.cotaTierra);
            con.Open();
            try
            {
                respuesta = (int)cmd.ExecuteScalar();
            }
            catch { }
            con.Close();
            return respuesta;
        }

        public static void Update(Punto_de_Monitoreo puntoMonitoreo)
        {
            SqlConnection con = db.Database.Connection as SqlConnection;
            SqlCommand cmd = new SqlCommand("PuntoMonitoreo_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idPuntoMonitoreo", puntoMonitoreo.idPuntoMonitoreo);
            cmd.Parameters.AddWithValue("@idFigura", puntoMonitoreo.idFigura);
            cmd.Parameters.AddWithValue("@nombre", puntoMonitoreo.nombre);
            cmd.Parameters.AddWithValue("@carpeta", puntoMonitoreo.carpeta);
            cmd.Parameters.AddWithValue("@cotaTierra", puntoMonitoreo.cotaTierra);
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