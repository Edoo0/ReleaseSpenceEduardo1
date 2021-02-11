using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ReleaseSpence.Models
{
	public class Georeferenciar
	{
		public int idImagen { get; set; }
		public float mx { get; set; }
		public float nx { get; set; }
		public float my { get; set; }
		public float ny { get; set; }
	}
}