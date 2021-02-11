using System.Data;
using System.Data.SqlClient;

namespace ReleaseSpence.Models
{
    public class Imagen_TipoSensorRep
    {
        private static MonitoreoIntegradoEntities db = new MonitoreoIntegradoEntities();

        public static void Create(Imagen_TipoSensor imagen_TipoSensor)
        {
            SqlConnection con = db.Database.Connection as SqlConnection;
            SqlCommand cmd = new SqlCommand("Imagen_TipoSensor_Create", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idImagen", imagen_TipoSensor.idImagen);
            cmd.Parameters.AddWithValue("@idTipo", imagen_TipoSensor.idTipo);
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

        public static void Delete(Imagen_TipoSensor imagen_TipoSensor)
        {
            SqlConnection con = db.Database.Connection as SqlConnection;
            SqlCommand cmd = new SqlCommand("Imagen_TipoSensor_Delete", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idImagen", imagen_TipoSensor.idImagen);
            cmd.Parameters.AddWithValue("@idTipo", imagen_TipoSensor.idTipo);
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