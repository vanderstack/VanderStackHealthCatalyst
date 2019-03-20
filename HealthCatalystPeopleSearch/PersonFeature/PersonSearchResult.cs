using System.Collections.Generic;

namespace VanderStack.HealthCatalystPeopleSearch.PersonFeature
{
	/// <summary>
	/// Encapsulates the results of a person search.
	/// </summary>
	public class PersonSearchResult
	{
		/// <summary>
		/// The set of people found as the result of the search.
		/// </summary>
		public IEnumerable<PersonModel> PersonSet { get; set; }
	}
}
