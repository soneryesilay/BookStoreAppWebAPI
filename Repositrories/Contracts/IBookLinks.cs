﻿using Entities.DataTransferObject;
using Entities.LinkModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositrories.Contracts
{
	public interface IBookLinks
	{
		LinkResponse TryGenerateLinks(IEnumerable<BookDto> booksDto,
			string fields, HttpContext httpContext);
	}
}
