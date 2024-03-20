using System.Runtime.CompilerServices;
using WebAPI.Utilities.Formater;

namespace WebAPI.Extensions
{
	public static class IMvcBuilderExtensions
	{
		public static IMvcBuilder AddCustomCvsFormatter(this IMvcBuilder builder) =>
			builder.AddMvcOptions(config =>
				config.OutputFormatters
			.Add(new CsvOutputFormatter()));

			
	}					
}
