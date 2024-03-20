using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObject
{
	public record TokenDto
	{
        public string? RefreshToken { get; init; }
		public string? AccessToken { get; init; }
    }
}
