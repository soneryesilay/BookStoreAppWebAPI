using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObject
{
	public record BookDtoForUpdate : BookDtoForManipulation
	{
		[Required]
        public int ID { get; set; }						
    }


	//public int ID { get; init; }     
	//public string Title { get; init; }
	// public decimal Price { get; init; }
	//data transfer objeleri readonly olmalıdır değeri değişmemelidir	(init = readolny tanımlandığı yerde değeri verilir)
}
