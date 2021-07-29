using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TanzuTacos.WebApi.Models;

namespace TanzuTacos.WebApi.Controllers
{
	[Route("specials")]
	[ApiController]
	public class SpecialsController : Controller
	{
		[HttpGet]
		public async Task<ActionResult<List<TacoSpecial>>> GetSpecialsAsync()
		{
			return await Task.FromResult(GetSpecialsList());
		}

		private List<TacoSpecial> GetSpecialsList()
		{
			return new List<TacoSpecial>
			{
				new TacoSpecial
				{
					Id = 1,
					Name ="Fish Tacos",
					BasePrice = 10M,
					Description ="Fish on a taco, mate!",
					ImageUrl ="https://images.theconversation.com/files/299378/original/file-20191030-154707-1n7h8ay.jpg?ixlib=rb-1.1.0&q=30&auto=format&w=600&h=400&fit=crop&dpr=2"
				},
				new TacoSpecial
				{
					Id = 2,
					Name ="All the Meat Tacos",
					BasePrice = 20M,
					Description ="An Ed pleasing amount of meat on a taco!",
					ImageUrl ="https://cdn-a.william-reed.com/var/wrbm_gb_food_pharma/storage/images/publications/food-beverage-nutrition/foodnavigator.com/article/2019/09/06/cutting-meat-consumption-may-cause-serious-harm-academics-warn/10121111-1-eng-GB/Cutting-meat-consumption-may-cause-serious-harm-academics-warn_wrbm_large.jpg"
				}
			};
		}
	}
}
