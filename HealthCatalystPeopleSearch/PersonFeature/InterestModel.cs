using System;
using System.ComponentModel.DataAnnotations;

namespace VanderStack.HealthCatalystPeopleSearch.PersonFeature
{
	/// <summary>
	/// The model of an interest for an <see cref="PersonModel"/>
	/// </summary>
	public class InterestModel
	{
		/// <summary>
		/// The Id of the interest
		/// </summary>
		[Key]
		public Guid Id { get; set; }

		/// <summary>
		/// The Id of the <see cref="PersonModel"/> who has the interest
		/// </summary>
		public Guid PersonId { get; set; }

		/// <summary>
		/// A summary of the interest
		/// </summary>
		public string Summary { get; set; }
	}
}
