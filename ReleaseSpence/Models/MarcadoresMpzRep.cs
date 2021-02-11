using System.Data;
using System.Data.SqlClient;

namespace ReleaseSpence.Models
{
    public class MarcadoresMpzRep
    {
        private static MonitoreoIntegradoEntities db = new MonitoreoIntegradoEntities();

        public static void Create(MarcadoresMpz marcadorMpz)
        {
            SqlConnection con = db.Database.Connection as SqlConnection;
            SqlCommand cmd = new SqlCommand("MarcadoresMpz_Create", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idPuntoMonitoreo", marcadorMpz.idPuntoMonitoreo);
            cmd.Parameters.AddWithValue("@idImagen", marcadorMpz.idImagen);
            cmd.Parameters.AddWithValue("@x", marcadorMpz.x);
            cmd.Parameters.AddWithValue("@y", marcadorMpz.y);
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

        public static void Delete(MarcadoresMpz marcadorMpz)
        {
            SqlConnection con = db.Database.Connection as SqlConnection;
            SqlCommand cmd = new SqlCommand("MarcadoresMpz_Delete", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idPuntoMonitoreo", marcadorMpz.idPuntoMonitoreo);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}