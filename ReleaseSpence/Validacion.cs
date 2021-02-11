using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Web;

namespace ReleaseSpence
{
    public static class Validacion
    {
        public const string notacionCientifica = @"^([-]?[\d]{1}(,[\d]+){1}[Ee]{1}[+-]?[\d]+)$";
        public const string notacionCientificaError = "Solo se permite notacion cientifica. 1,234E-5";
        public const string numero = @"^([-]?[\d]+(,[\d]+)?)?$";
        public const string numeroError = "Solo se permite numeros";
        public const string numeroOcientifica = @"^(([-]?[\d]{1}(,[\d]+){1}[Ee]{1}[+-]?[\d]+)||([-]?[\d]+(,[\d]+)?)?)$";
        public const string numeroOcientificaError = "Solo se permite numeros o notacion cientifica";
    }

    public class regexTestAttribute : ValidationAttribute
    {
        private Regex _regex = new Regex(Validacion.numeroOcientifica);

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (_regex.IsMatch(value.ToString())) return ValidationResult.Success;
                else return new ValidationResult("Solo se permite letras, numeros y puntuaciones(- _ .)");
            }
            else return ValidationResult.Success;
        }
    }

    public class AlfanumericoAttribute : ValidationAttribute
    {
        private Regex _regex = new Regex(@"^[\w\s.-_]+$");

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (_regex.IsMatch(value.ToString())) return ValidationResult.Success;
                else return new ValidationResult("Solo se permite letras, numeros y puntuaciones(- _ .)");
            }
            else return ValidationResult.Success;
        }
    }

    public class jpgAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			HttpPostedFileBase file = value as HttpPostedFileBase;
			if (file == null) return ValidationResult.Success;
			try
			{
				using (var img = Image.FromStream(file.InputStream))
				{
					if(img.RawFormat.Equals(ImageFormat.Jpeg)) return ValidationResult.Success;
					else return new ValidationResult("La imagen debe ser formato jpg");
				}
			}
			catch
			{
				return new ValidationResult("La imagen debe ser formato jpg");
			}
		}
	}
}