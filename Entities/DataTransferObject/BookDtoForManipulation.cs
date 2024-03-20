using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObject
{
	public abstract record class BookDtoForManipulation
	{
        [Required(ErrorMessage ="Title is a required field.")]
        [MinLength(2, ErrorMessage = "Title mustconsist of at least 2 characters.")]
        [MaxLength(50, ErrorMessage = "Title mustconsist of at maximum 2 characters.")]
        public string Title { get; init; }

        [Required(ErrorMessage = "Price is a required field.")]
        [Range(10,1000)]

        public decimal Price { get; init; }
    }
}
