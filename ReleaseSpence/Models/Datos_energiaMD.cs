using System;
using System.ComponentModel.DataAnnotations;

namespace ReleaseSpence.Models
{
	[MetadataType(typeof(Datos_energiaMD))]
    public partial class Datos_energia
    {
    }

	public class Datos_energiaMD
    {
		public int idDato { get; set; }

		[Display(Name = "Fecha")]
		[DisplayFormat(DataFormatString = "{0:dd'-'MM'-'yyyy' 'HH':'mm':'ss}", ApplyFormatInEditMode = true)]
		public DateTime fecha { get; set; }

		[Display(Name = "Eficiencia cargador")]
		[DisplayFormat(DataFormatString = "{0:#.##}")]
		public float charE { get; set; } // %

		[Display(Name = "Voltaje de baterias")]
		public float batV { get; set; } // V

		[Display(Name = "Potencia de baterias")]
		[DisplayFormat(DataFormatString = "{0:#.##}")]
		public float batP { get; set; } // W

		[Display(Name = "Potencia de paneles")]
		[DisplayFormat(DataFormatString = "{0:#.##}")]
		public float panelP { get; set; }

		[Display(Name = "Corriente de baterias")]
		public float batC { get; set; } // A

		[Display(Name = "Energia consumida de baterias")]
		public float batCE { get; set; } //Ah

		[Display(Name = "Estado de Carga baterias")]
		public float batSOC { get; set; } // %

		[Display(Name = "Tiempo de energia restante")]
		public int batTTG { get; set; } // m

		[Display(Name = "Eficiencia inversor")]
		[DisplayFormat(DataFormatString = "{0:#.##}")]
		public float invE { get; set; } // %
    }
}