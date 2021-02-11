using System.ComponentModel.DataAnnotations;

namespace ReleaseSpence.Models
{
    public class ChangePassVM
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña actual")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar la nueva contraseña")]
        [Compare("NewPassword", ErrorMessage = "La nueva contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPassVM
	{
		[Required]
		public int idUsuario { get; set; }

		[Display(Name = "Nombre de Usuario")]
		public string userName { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Nueva contraseña")]
		public string NewPassword { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirmar la nueva contraseña")]
		[Compare("NewPassword", ErrorMessage = "La nueva contraseña y la contraseña de confirmación no coinciden.")]
		public string ConfirmPassword { get; set; }
	}

	public class LoginViewModel
	{
		[Required]
		[Display(Name = "Nombre de Usuario")]
		public string userName { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Contraseña")]
		public string Password { get; set; }

		[Display(Name = "¿Recordar cuenta?")]
		public bool RememberMe { get; set; }
	}

	[MetadataType(typeof(IdentityUserMD))]
	public partial class Identity_Users
	{
		public int[] roles { get; set; }
	}
	public class IdentityUserMD
	{
		[Required]
		[EmailAddress]
		[Display(Name = "Correo electrónico")]
		public string email { get; set; }

		[Required]
		[Display(Name = "Nombre de Usuario")]
		public string userName { get; set; }

		[Required]
		[Display(Name = "Nombre y Apellido")]
		public string fullName { get; set; }
	}

	public class UserRegister
	{
		[Required]
		[EmailAddress]
		[Display(Name = "Correo electrónico")]
		public string email { get; set; }

		[Required]
		[Display(Name = "Nombre de Usuario")]
		public string userName { get; set; }

		[Required]
		[Display(Name = "Nombre y Apellido")]
		public string fullName { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Contraseña")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirmar contraseña")]
		[Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
		public string ConfirmPassword { get; set; }

		public int[] roles { get; set; }
	}
}