using System.Collections.Generic;

namespace VanderStack.HealthCatalystPeopleSearch.PersonFeature
{
	/// <summary>
	/// Encapsulates the results of a person validation.
	/// </summary>
	public class PersonValidationResult
	{
		/// <summary>
		/// The set of validation errors for the person.
		/// </summary>
		public IEnumerable<string> ValidationErrors { get; set; }
	}
}
