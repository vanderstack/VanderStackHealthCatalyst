using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VanderStack.HealthCatalystPeopleSearch.PersonFeature
{
	/// <summary>
	/// A model representing a person
	/// </summary>
	public class PersonModel
	{
		/// <summary>
		/// Creates a new instance of the <see cref="PersonModel"/> class
		/// </summary>
		public PersonModel()
		{
			InterestSet = new HashSet<InterestModel>();
		}

		/// <summary>
		/// The Id of this person
		/// </summary>
		[Key]
		public Guid Id { get; set; }

		/// <summary>
		/// The First Name of this person
		/// </summary>
		[Required]
		public string FirstName { get; set; }

		/// <summary>
		/// The Last Name of this person
		/// </summary>
		[Required]
		public string LastName { get; set; }

		/// <summary>
		/// The Address of the current person
		/// </summary>
		/// 
		/// <remarks>
		/// This could have been facilitated through Line1, Line2, etc fields
		/// but because the requirements are often unique to each firm I decided
		/// to start off keeping this simple.
		/// 
		/// todo: need business input on if address is optional
		/// </remarks>
		public string Address { get; set; }

		/// <summary>
		/// The age of this person
		/// </summary>
		/// 
		/// <remarks>
		/// todo: need business input on valid age ranges
		/// </remarks>
		[Range(13, 130)]
		public int Age { get; set; }

		/// <summary>
		/// A photo of this person
		/// </summary>
		/// 
		/// <remarks>
		/// todo: need business input on if photo is optional
		/// 
		/// todo: edge cases have not been addressed such as the
		/// photo url starting with http on an https connection
		/// or having a relative route rather than absolute.
		/// </remarks>
		public string PhotoUrl { get; set; }

		/// <summary>
		/// A set of interests this person has
		/// </summary>
		public ICollection<InterestModel> InterestSet { get; set; }
	}
}
