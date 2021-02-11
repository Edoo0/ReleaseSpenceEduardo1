using System.Data;
using System.Data.SqlClient;

namespace ReleaseSpence.Models
{
    public class Identity_UsersRep
    {
        private static MonitoreoIntegradoEntities db = new MonitoreoIntegradoEntities();

        public static void Update(Identity_Users usuario)
        {
            SqlConnection con = db.Database.Connection as SqlConnection;
            SqlCommand cmd = new SqlCommand("Identity_Users_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idUsuario", usuario.idUsuario);
            cmd.Parameters.AddWithValue("@email", usuario.email);
            cmd.Parameters.AddWithValue("@userName", usuario.userName);
            cmd.Parameters.AddWithValue("@fullName", usuario.fullName);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static void CreateRol(int idRol, int idUsuario)
        {
            SqlConnection con = db.Database.Connection as SqlConnection;
            SqlCommand cmd = new SqlCommand("Identity_UserRoles_Create", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idRol", idRol);
            cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static void DeleteRol(int idRol, int idUsuario)
        {
            SqlConnection con = db.Database.Connection as SqlConnection;
            SqlCommand cmd = new SqlCommand("Identity_UserRoles_Delete", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idRol", idRol);
            cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}