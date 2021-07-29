using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TanzuTacos.WebApi.Models;

namespace TanzuTacos.WebApi.Controllers
{
	[Route("extras")]
	[ApiController]
	public class ExtrasController : Controller
	{

		[HttpGet]
		public async Task<ActionResult<List<Extra>>> GetExtrasAsync()
		{
			return await Task.FromResult(GetExtrasList());
		}

		private List<Extra> GetExtrasList()
		{
			return new List<Extra>
			{
				new Extra
				{
					Id = 1,
					Name ="lettuce",
					Price = .5M,

				},
				new Extra
				{
					Id = 2,
					Name ="tomato",
					Price = 1M,
				},
				new Extra
				{
					Id = 3,
					Name ="cheese",
					Price = 2.5M,
				},
				new Extra
				{
					Id = 4,
					Name ="salsa",
					Price = 2M,
				},
				new Extra
				{
					Id = 5,
					Name ="avocado",
					Price = 4M,
				}

			};
		}
	}
}