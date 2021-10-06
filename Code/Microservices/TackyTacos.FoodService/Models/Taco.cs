namespace TackyTacos.FoodService.Models
{
	public class Taco
	{
		public int Id { get; set; }

		public int OrderId { get; set; }

		public TacoSpecial Special { get; set; }

		public int SpecialId { get; set; }

		public List<TacoExtra> Extras { get; set; }


		public decimal GetTotalPrice()
		{
			return Special.BasePrice + Extras.Sum(e => e.Extra.Price);
		}

		public string GetFormattedTotalPrice()
		{
			return GetTotalPrice().ToString("0.00");
		}
	}
}
