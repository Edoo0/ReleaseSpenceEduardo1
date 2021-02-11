using System;

namespace ReleaseSpence
{
    public class Funciones
    {
        public static double coseno(short grados)
        {
            return Math.Cos(grados * Math.PI / 180);
        }
        public static double seno(short grados)
        {
            return Math.Sin(grados * Math.PI / 180);
        }
        public static double coseno(int grados)
        {
            return Math.Cos(grados * Math.PI / 180);
        }
        public static double seno(int grados)
        {
            return Math.Sin(grados * Math.PI / 180);
        }

        public static void DesdeHasta(string desde, string hasta, out DateTime fdesde, out DateTime fhasta)
        {
            fhasta = DateTime.Now;
            if (hasta == "1d")
                fdesde = fhasta.AddDays(-1);
            else if (hasta == "1s")
                fdesde = fhasta.AddDays(-7);
            else if (hasta == "1m")
                fdesde = fhasta.AddMonths(-1);
            else if (hasta == "3m")
                fdesde = fhasta.AddMonths(-3);
            else if (hasta == "1a")
                fdesde = fhasta.AddYears(-1);
            else
            {
                fhasta = DateTime.ParseExact(hasta, "ddMMyyyyHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                fdesde = DateTime.ParseExact(desde, "ddMMyyyyHHmmss", System.Globalization.CultureInfo.InvariantCulture);
            }
        }
    }
}