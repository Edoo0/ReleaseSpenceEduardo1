using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ReleaseSpence.Models
{
	public class Datos_piezometroRep
	{
        static ILog _logger = LogManager.GetLogger(typeof(Datos_piezometroRep));

        private static MonitoreoIntegradoEntities db = new MonitoreoIntegradoEntities();

		public static void Create(Datos_piezometro dato_piezometro)
		{
			SqlConnection con = db.Database.Connection as SqlConnection;
			SqlCommand cmd = new SqlCommand("Datos_piezometro_Create", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@idSensor", dato_piezometro.idSensor);
            cmd.Parameters.AddWithValue("@fecha", dato_piezometro.fecha);
            cmd.Parameters.AddWithValue("@cotaAgua", dato_piezometro.cotaAgua);
            cmd.Parameters.AddWithValue("@metrosSensor", dato_piezometro.metrosSensor);
            cmd.Parameters.AddWithValue("@temperatura_pz", dato_piezometro.temperatura_pz);
            cmd.Parameters.AddWithValue("@bUnits", dato_piezometro.bUnits);
            cmd.Parameters.AddWithValue("@presion_pz", dato_piezometro.presion_pz);
            cmd.Parameters.AddWithValue("@temperatura_bmp", dato_piezometro.temperatura_bmp);
            cmd.Parameters.AddWithValue("@presion_bmp", dato_piezometro.presion_bmp);
            try
            {

                if (con.State == ConnectionState.Open) con.Close();

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                _logger.Info($"Datos_piezometroRep >>> [CREATE SUCCESS] \r\n {JsonConvert.SerializeObject(dato_piezometro).Replace((char)34, (char)39)}");
            }
            catch(Exception e)
            {
                _logger.Error($"Datos_piezometroRep >>> [CREATE ERROR] \r\n {e?.Message} \r\n {e?.InnerException?.Message}");

                if (con.State == ConnectionState.Open) con.Close();
            }
        }

        //public static List<Datos_piezometro> Multipiezometro(int idSensor, bool precision, int cantidad_datos, DateTime desde, DateTime hasta)
        //{
        //    List<Datos_piezometro> datos = new List<Datos_piezometro>();
        //    SqlConnection con = db.Database.Connection as SqlConnection;
        //    if (!precision)
        //    {
        //        SqlCommand cmd = new SqlCommand("Datos_piezometro_Graphics", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@idSensor", idSensor);
        //        cmd.Parameters.AddWithValue("@precision", precision);
        //        cmd.Parameters.AddWithValue("@datos", cantidad_datos);
        //        cmd.Parameters.AddWithValue("@desde", desde);
        //        cmd.Parameters.AddWithValue("@hasta", hasta);
        //        if (!(con.State == ConnectionState.Open)) con.Open();
        //        SqlDataReader lector = cmd.ExecuteReader();
        //        while (lector.Read())
        //        {
        //            Datos_piezometro dato = new Datos_piezometro();
        //            dato.fecha = (DateTime)lector["fecha"];
        //            dato.cotaAgua = (float)lector["cotaAgua"];
        //            dato.metrosSensor = (float)lector["metrosSensor"];
        //            dato.temperatura_pz = (float)lector["temperatura_pz"];
        //            dato.presion_pz = (float)lector["presion_pz"];
        //            dato.temperatura_bmp = (lector["temperatura_bmp"].GetType() == typeof(DBNull) ? 0 : (float)(lector["temperatura_bmp"]));
        //            dato.presion_bmp = (lector["presion_bmp"].GetType() == typeof(DBNull) ? 0 : (float)(lector["presion_bmp"]));
        //            datos.Add(dato);
        //        }
        //        con.Close();
        //        return datos;
        //    }
        //    else
        //    {
        //        SqlCommand cmd = new SqlCommand("Datos_piezometro_Range", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@idSensor", idSensor);
        //        cmd.Parameters.AddWithValue("@desde", desde);
        //        cmd.Parameters.AddWithValue("@hasta", hasta);
        //        if (!(con.State == ConnectionState.Open)) con.Open();
        //        SqlDataReader lector = cmd.ExecuteReader();
        //        while (lector.Read())
        //        {
        //            Datos_piezometro dato = new Datos_piezometro();
        //            dato.fecha = (DateTime)lector["fecha"];
        //            dato.cotaAgua = (float)lector["cotaAgua"];
        //            dato.metrosSensor = (float)lector["metrosSensor"];
        //            dato.bUnits = (float)lector["bUnits"];
        //            dato.temperatura_pz = (float)lector["temperatura_pz"];
        //            dato.presion_pz = (float)lector["presion_pz"];
        //            dato.temperatura_bmp = (lector["temperatura_bmp"].GetType() == typeof(DBNull) ? 0 : (float)(lector["temperatura_bmp"]));
        //            dato.presion_bmp = (lector["presion_bmp"].GetType() == typeof(DBNull) ? 0 : (float)(lector["presion_bmp"]));
        //            datos.Add(dato);
        //        }
        //        con.Close();
        //        return datos;
        //    }
        //}

        public static List<Datos_piezometro> getLatest(int cantidad, int idSensor, DateTime fecha)
        {
            /*List<Datos_piezometro> datos = new List<Datos_piezometro>();
            SqlConnection con = db.Database.Connection as SqlConnection;
            var items = db.Datos_piezometro.OrderByDescending(u => u.fecha).Take(cantidad);
            return items.ToList();*/

            List<Datos_piezometro> datos = new List<Datos_piezometro>();
            SqlConnection con = db.Database.Connection as SqlConnection;
            
            string query="";
            query += "SELECT TOP @cantidad * from Datos_piezometro ";
            query += "WHERE idSensor = @idSensor AND fecha < @fecha ";
            query += "ORDER BY fecha DESC";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@cantidad", cantidad);
            cmd.Parameters.AddWithValue("@idSensor", idSensor);
            cmd.Parameters.AddWithValue("@fecha", fecha);

            if (con.State == ConnectionState.Open) con.Close();
            con.Open();
            SqlDataReader lector = cmd.ExecuteReader();
            while (lector.Read())
            {
                Datos_piezometro dato = new Datos_piezometro();
                dato.fecha = (DateTime)lector["fecha"];
                dato.cotaAgua = (float)lector["cotaAgua"];
                dato.metrosSensor = (float)lector["metrosSensor"];
                dato.temperatura_pz = (float)lector["temperatura_pz"];
                dato.bUnits = (float)lector["bUnits"];
                dato.presion_pz = (float)lector["presion_pz"];
                dato.temperatura_bmp = (lector["temperatura_bmp"].GetType() == typeof(DBNull) ? 0 : (float)(lector["temperatura_bmp"]));
                dato.presion_bmp = (lector["presion_bmp"].GetType() == typeof(DBNull) ? 0 : (float)(lector["presion_bmp"]));
                datos.Add(dato);
            }
            if (con.State == ConnectionState.Open) con.Close();
            return datos;
        }

        public static List<Datos_piezometro> getAll(int idSensor)
        {

            List<Datos_piezometro> datos = new List<Datos_piezometro>();
            SqlConnection con = db.Database.Connection as SqlConnection;

            string query = "";
            query += "SELECT top 10 * from Datos_piezometro ";
            query += "WHERE idSensor = @idSensor ";
            query += "ORDER BY fecha desc";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@idSensor", idSensor);

            if (con.State == ConnectionState.Open) con.Close();
            con.Open();
            SqlDataReader lector = cmd.ExecuteReader();
            while (lector.Read())
            {
                Datos_piezometro dato = new Datos_piezometro();
                dato.idDato = (int)lector["idDato"];
                dato.idSensor = (int)lector["idSensor"];
                dato.fecha = (DateTime)lector["fecha"];
                dato.cotaAgua = (float)lector["cotaAgua"];
                dato.metrosSensor = (float)lector["metrosSensor"];
                dato.temperatura_pz = (float)lector["temperatura_pz"];
                dato.bUnits = (float)lector["bUnits"];
                dato.presion_pz = (float)lector["presion_pz"];
                dato.temperatura_bmp = (lector["temperatura_bmp"].GetType() == typeof(DBNull) ? 0 : (float)(lector["temperatura_bmp"]));
                dato.presion_bmp = (lector["presion_bmp"].GetType() == typeof(DBNull) ? 0 : (float)(lector["presion_bmp"]));
                datos.Add(dato);
            }
            if (con.State == ConnectionState.Open) con.Close();
            return datos;
        }


        public static List<Datos_piezometro> getDatosRotos(int idSensor)
        {

            List<Datos_piezometro> datos = new List<Datos_piezometro>();
            SqlConnection con = db.Database.Connection as SqlConnection;

            string query = "";

            query += "SELECT * from Datos_piezometro dp ";
            query += "JOIN Sensores s ON dp.idSensor = s.idSensor ";
            query += "JOIN Sensores_Piezometros sp ON s.idSensor = sp.idSensor ";
            query += "WHERE dp.idSensor = @idSensor AND (";
            query += "dp.cotaAgua > sp.cotaTierra OR ";
            query += "dp.metrosSensor < 0 OR ";
            query += "dp.presion_pz < 0 )";
            

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@idSensor", idSensor);

            if (!(con.State == ConnectionState.Open)) con.Open();
            SqlDataReader lector = cmd.ExecuteReader();
            while (lector.Read())
            {
                int iddatito = (int)lector["idDato"];
                if (iddatito == 75658)
                {
                    Console.WriteLine("estoy aqui :)");

                }
                Datos_piezometro dato = new Datos_piezometro();
                dato.idDato = (int)lector["idDato"];
                dato.idSensor = (int)lector["idSensor"];
                dato.fecha = (DateTime)lector["fecha"];
                dato.cotaAgua = (float)lector["cotaAgua"];
                dato.metrosSensor = (float)lector["metrosSensor"];
                dato.temperatura_pz = (float)lector["temperatura_pz"];
                dato.bUnits = (float)lector["bUnits"];
                dato.presion_pz = (float)lector["presion_pz"];
                dato.temperatura_bmp = (lector["temperatura_bmp"].GetType() == typeof(DBNull) ? 0 : (float)(lector["temperatura_bmp"]));
                dato.presion_bmp = (lector["presion_bmp"].GetType() == typeof(DBNull) ? 0 : (float)(lector["presion_bmp"]));
                datos.Add(dato);
            }
            con.Close();
            return datos;
        }

        public static int borrarDatosMalos(int idSensor)
        {

            List<Datos_piezometro> datos = new List<Datos_piezometro>();
            SqlConnection con = db.Database.Connection as SqlConnection;

            string query = "";

            query += "DELETE dp FROM Datos_piezometro dp ";
            query += "JOIN Sensores s ON dp.idSensor = s.idSensor ";
            query += "JOIN Sensores_Piezometros sp ON s.idSensor = sp.idSensor ";
            query += "WHERE dp.idSensor = @idSensor AND (";
            query += "dp.cotaAgua > sp.cotaTierra OR ";
            query += "dp.metrosSensor < 0 OR ";
            query += "dp.presion_pz < 0)";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@idSensor", idSensor);
            int filasAfectadas = 0;
            try
            {
                con.Open();
                filasAfectadas = cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception e)
            {
                con.Close();
            }
            return filasAfectadas;
        }

        public static int delete(int idDato)
        {

            List<Datos_piezometro> datos = new List<Datos_piezometro>();
            SqlConnection con = db.Database.Connection as SqlConnection;

            string query = "";

            query += "DELETE dp FROM Datos_piezometro dp ";
            query += "WHERE dp.idDato = @idDato";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@idDato", idDato);
            int filasAfectadas = 0;
            try
            {
                con.Open();
                filasAfectadas = cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception e)
            {
                con.Close();
            }
            return filasAfectadas;
        }

        public static List<Datos_piezometro> getDatosBuenos(int idSensor)
        {

            List<Datos_piezometro> datos = new List<Datos_piezometro>();
            SqlConnection con = db.Database.Connection as SqlConnection;

            string query = "";

            query += "SELECT * from Datos_piezometro dp ";
            query += "JOIN Sensores s ON dp.idSensor = s.idSensor ";
            query += "JOIN Sensores_Piezometros sp ON s.idSensor = sp.idSensor ";
            query += "WHERE dp.idSensor = @idSensor AND (";
            query += "dp.cotaAgua < sp.cotaTierra AND ";
            query += "dp.metrosSensor >= 0 AND ";
            query += "dp.presion_pz >= 0)";
            

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@idSensor", idSensor);

            if (!(con.State == ConnectionState.Open)) con.Open();
            SqlDataReader lector = cmd.ExecuteReader();
            while (lector.Read())
            {
                Datos_piezometro dato = new Datos_piezometro();
                dato.fecha = (DateTime)lector["fecha"];
                dato.cotaAgua = (float)lector["cotaAgua"];
                dato.metrosSensor = (float)lector["metrosSensor"];
                dato.temperatura_pz = (float)lector["temperatura_pz"];
                dato.bUnits = (float)lector["bUnits"];
                dato.presion_pz = (float)lector["presion_pz"];
                dato.temperatura_bmp = (lector["temperatura_bmp"].GetType() == typeof(DBNull) ? 0 : (float)(lector["temperatura_bmp"]));
                dato.presion_bmp = (lector["presion_bmp"].GetType() == typeof(DBNull) ? 0 : (float)(lector["presion_bmp"]));
                datos.Add(dato);
            }
            con.Close();
            return datos;

        }

        public static void updateMetroSensor(int idDato, float metrosSensor)
        {
            List<Datos_piezometro> datos = new List<Datos_piezometro>();
            SqlConnection con = db.Database.Connection as SqlConnection;

            string query = "";
            query += "UPDATE Datos_piezometro SET ";
            query += "metrosSensor = @metrosSensor ";
            query += "WHERE idDato = @idDato ";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@metrosSensor", metrosSensor);
            cmd.Parameters.AddWithValue("@idDato", idDato);

            if (!(con.State == ConnectionState.Open)) con.Open();

            cmd.ExecuteNonQuery();

            con.Close();

        }

        public static void updateCotaAgua(int idDato, float cotaAgua)
        {
            List<Datos_piezometro> datos = new List<Datos_piezometro>();
            SqlConnection con = db.Database.Connection as SqlConnection;

            string query = "";
            query += "UPDATE Datos_piezometro SET ";
            query += "cotaAgua = @cotaAgua ";
            query += "WHERE idDato = @idDato ";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@cotaAgua", cotaAgua);
            cmd.Parameters.AddWithValue("@idDato", idDato);

            if (!(con.State == ConnectionState.Open)) con.Open();

            cmd.ExecuteNonQuery();

            con.Close();

        }

        public static List<Datos_piezometro> Graphics(int idSensor, bool precision, int cantidad_datos, DateTime desde, DateTime hasta)
        {
            List<Datos_piezometro> datos = new List<Datos_piezometro>();
            SqlConnection con = db.Database.Connection as SqlConnection;
            if (!precision)
            {
                SqlCommand cmd = new SqlCommand("Datos_piezometro_Graphics", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idSensor", idSensor);
                cmd.Parameters.AddWithValue("@precision", precision);
                cmd.Parameters.AddWithValue("@datos", cantidad_datos);
                cmd.Parameters.AddWithValue("@desde", desde);
                cmd.Parameters.AddWithValue("@hasta", hasta);
                if (!(con.State == ConnectionState.Open)) con.Open();
                SqlDataReader lector = cmd.ExecuteReader();
                while (lector.Read())
                {
                    Datos_piezometro dato = new Datos_piezometro();
                    dato.fecha = (DateTime)lector["fecha"];
                    dato.cotaAgua = (float)lector["cotaAgua"];
                    dato.metrosSensor = (float)lector["metrosSensor"];
                    dato.temperatura_pz = (float)lector["temperatura_pz"];
                    dato.presion_pz = (float)lector["presion_pz"];
                    dato.temperatura_bmp = (lector["temperatura_bmp"].GetType() == typeof(DBNull) ? 0 : (float)(lector["temperatura_bmp"]));
                    dato.presion_bmp = (lector["presion_bmp"].GetType() == typeof(DBNull) ? 0 : (float)(lector["presion_bmp"]));
                    datos.Add(dato);
                }
                con.Close();
                return datos;
            }
            else
            {
                SqlCommand cmd = new SqlCommand("Datos_piezometro_Range", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idSensor", idSensor);
                cmd.Parameters.AddWithValue("@desde", desde);
                cmd.Parameters.AddWithValue("@hasta", hasta);
                if (!(con.State == ConnectionState.Open)) con.Open();
                SqlDataReader lector = cmd.ExecuteReader();
                while (lector.Read())
                {
                    Datos_piezometro dato = new Datos_piezometro();
                    dato.fecha = (DateTime)lector["fecha"];
                    dato.cotaAgua = (float)lector["cotaAgua"];
                    dato.metrosSensor = (float)lector["metrosSensor"];
                    dato.bUnits = (float)lector["bUnits"];
                    dato.temperatura_pz = (float)lector["temperatura_pz"];
                    dato.presion_pz = (float)lector["presion_pz"];
                    dato.temperatura_bmp = (lector["temperatura_bmp"].GetType() == typeof(DBNull) ? 0 : (float)(lector["temperatura_bmp"]));
                    dato.presion_bmp = (lector["presion_bmp"].GetType() == typeof(DBNull) ? 0 : (float)(lector["presion_bmp"]));
                    datos.Add(dato);
                }
                con.Close();
                return datos;
            }
		}

		public static List<Datos_sensor_alarm> Alertas(int idImagen, int idTipo, DateTime desde, DateTime hasta)
		{
			List<Datos_sensor_alarm> datos = new List<Datos_sensor_alarm>();
			SqlConnection con = db.Database.Connection as SqlConnection;
			SqlCommand cmd = new SqlCommand("Datos_piezometro_Alertas", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@idImagen", idImagen);
			cmd.Parameters.AddWithValue("@idTipo", idTipo);
			cmd.Parameters.AddWithValue("@desde", desde);
			cmd.Parameters.AddWithValue("@hasta", hasta);
			if (!(con.State == ConnectionState.Open)) con.Open();
			SqlDataReader lector = cmd.ExecuteReader();
			while (lector.Read())
			{
				Datos_sensor_alarm dato = new Datos_sensor_alarm();
				dato.idSensor = (int)lector["idSensor"];
				dato.nombre = (string)lector["nombre"];
				dato.cantidad = (int)lector["cantidad"];
				datos.Add(dato);
			}
			con.Close();
			return datos;
		}

		public static List<Datos_sensor_alarmList> AlertasDet(int idSensor, DateTime desde, DateTime hasta)
		{
			List<Datos_sensor_alarmList> datos = new List<Datos_sensor_alarmList>();
			SqlConnection con = db.Database.Connection as SqlConnection;
			SqlCommand cmd = new SqlCommand("Datos_piezometro_AlertasDet", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@idSensor", idSensor);
			cmd.Parameters.AddWithValue("@desde", desde);
			cmd.Parameters.AddWithValue("@hasta", hasta);
			if (!(con.State == ConnectionState.Open)) con.Open();
			SqlDataReader lector = cmd.ExecuteReader();
			while (lector.Read())
			{
				Datos_sensor_alarmList dato = new Datos_sensor_alarmList();
				dato.idSensor = (int)lector["idSensor"];
				dato.dato = (float)lector["dato"];
				dato.fecha = (DateTime)lector["fecha"];
				datos.Add(dato);
			}
			con.Close();
			return datos;
        }

        public static int Corregir(CorregirPiezo correccion)
        {
            int respuesta = -1;
            SqlConnection con = db.Database.Connection as SqlConnection;
            SqlCommand cmd = new SqlCommand("Datos_piezometro_Corregir", con);
            cmd.CommandType = CommandType.StoredProcedure;
            float coefA = 0, tempK = 0;  
            if (!string.IsNullOrEmpty(correccion.coefA)) coefA = float.Parse(correccion.coefA, System.Globalization.NumberStyles.Float);
            if (!string.IsNullOrEmpty(correccion.tempK)) tempK = float.Parse(correccion.tempK, System.Globalization.NumberStyles.Float);
            cmd.Parameters.AddWithValue("@idSensor", correccion.idSensor);
            cmd.Parameters.AddWithValue("@desde", correccion.desde);
            cmd.Parameters.AddWithValue("@hasta", correccion.hasta);
            cmd.Parameters.AddWithValue("@cotaSensor", correccion.cotaSensor);
            cmd.Parameters.AddWithValue("@coefA", coefA);
            cmd.Parameters.AddWithValue("@coefB", correccion.coefB);
            cmd.Parameters.AddWithValue("@coefC", correccion.coefC);
            cmd.Parameters.AddWithValue("@tempK", tempK);
            cmd.Parameters.AddWithValue("@tempI", correccion.tempI);
            cmd.Parameters.AddWithValue("@baroI", correccion.baroI == null ? (object)DBNull.Value : correccion.baroI);
            cmd.Parameters.AddWithValue("@metrosSensor", correccion.metrosSensor);

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

        public static List<Datos_piezometro> getAllLatest()
        {
            /*List<Datos_piezometro> datos = new List<Datos_piezometro>();
            SqlConnection con = db.Database.Connection as SqlConnection;
            var items = db.Datos_piezometro.OrderByDescending(u => u.fecha).Take(cantidad);
            return items.ToList();*/

            List<Datos_piezometro> datos = new List<Datos_piezometro>();
            SqlConnection con = db.Database.Connection as SqlConnection;

            string query = "";
             query+= "SELECT * from Datos_piezometro t inner join (";
             query+= "select idsensor, max(fecha) as MaxDate from Datos_piezometro group by idSensor) tm ON t.idSensor = tm.idSensor AND t.fecha = tm.MaxDate ORDER BY t.idSensor";

            SqlCommand cmd = new SqlCommand(query, con);


            if (con.State == ConnectionState.Open) con.Close();
            con.Open();
            SqlDataReader lector = cmd.ExecuteReader();
            while (lector.Read())
            {
                Datos_piezometro dato = new Datos_piezometro();
                dato.idSensor = (int)lector["idSensor"];
                dato.fecha = (DateTime)lector["fecha"];
                dato.cotaAgua = (float)lector["cotaAgua"];
                dato.metrosSensor = (float)lector["metrosSensor"];
                dato.temperatura_pz = (float)lector["temperatura_pz"];
                dato.bUnits = (float)lector["bUnits"];
                dato.presion_pz = (float)lector["presion_pz"];
                dato.temperatura_bmp = (lector["temperatura_bmp"].GetType() == typeof(DBNull) ? 0 : (float)(lector["temperatura_bmp"]));
                dato.presion_bmp = (lector["presion_bmp"].GetType() == typeof(DBNull) ? 0 : (float)(lector["presion_bmp"]));
                datos.Add(dato);
            }
            if (con.State == ConnectionState.Open) con.Close();
            return datos;
        }
    }
}