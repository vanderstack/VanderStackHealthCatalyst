using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace VanderStack.HealthCatalystPeopleSearch.PersonFeature
{
	/// <summary>
	/// A controller for handling network requests related to People
	/// </summary>
	[Route("api/[controller]")]
	[Produces("application/json")]
    public class PersonController : Controller
    {
		/// <summary>
		/// Creates a new instance of the Person Controller requiring all dependencies
		/// in the constructor
		/// </summary>
		/// <param name="databaseContext">The database context for working with people</param>
		/// <param name="appSettings">The settings for how the application is configured</param>
		public PersonController(
			PersonDatabaseContext databaseContext
			, IOptionsSnapshot<AppSettings> appSettings
		)
		{
			DatabaseContext = databaseContext;
			AppSettings = appSettings;
		}

		/// <summary>
		/// The database context for working with people
		/// </summary>
		protected PersonDatabaseContext DatabaseContext { get; }

		/// <summary>
		/// The settings for how the application is configured
		/// </summary>
		protected IOptionsSnapshot<AppSettings> AppSettings { get; }

		/// <summary>
		/// Creates a new Person
		/// </summary>
		/// <param name="model">The new person to create</param>
		/// <returns>
		/// Validation Errors or Success.
		/// </returns>
		[HttpPost()]
		public async Task<IActionResult> Post([FromBody] PersonModel model)
		{
			// todo: in a more mature system this could easily
			// be handled by a validation middleware. In addition
			// we would want client side validation in an ideal world,
			// but server side is a requirement as we cannot trust
			// any data sent from the outside won't be malicious.
			if (!ModelState.IsValid)
			{
				return BadRequest(new PersonValidationResult
				{
					ValidationErrors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage))
				});
			}

			await DatabaseContext.PersonSet.AddAsync(model);
			await DatabaseContext.SaveChangesAsync();
			return Ok();
		}

		/// <summary>
		/// Performs a search for a person meeting the provided personName
		/// </summary>
		/// <param name="personName">The name of the person to search for</param>
		/// <returns>
		/// A set of people who contain the provided personName token
		/// </returns>
		[HttpGet("search/{personName}")]
		public async Task<IActionResult> Search(string personName)
		{
			// return an empty set rather than the full set when no filter criteria is provided
			if (string.IsNullOrEmpty(personName)) { return Ok(new PersonSearchResult { PersonSet = Enumerable.Empty<PersonModel>() }); }

			// remove whitespace and ignore capitalization
			personName = personName.Trim().ToLower();

			var maxNumberOfResults = AppSettings.Value.PersonSearchMaxResults;

			var searchResultSet =
				(
					await
					DatabaseContext
					.PersonSet
					.Include(person =>
						person.InterestSet
					)
					.AsNoTracking()
					.Where(candidatePerson =>
						(candidatePerson.FirstName + " " + candidatePerson.LastName)
						// these options do not translate to SQL
						//$"{candidatePerson.FirstName} {candidatePerson.LastName}"
						// candidatePerson.FullName
						.ToLower()
						.Contains(personName)
					)
					// todo: the resultset has been limited to a maximum size to
					// avoid consuming too many resources. This limit may need to 
					// be reviewed by the business. Pagination should be implemented
					// to allow records beyond the limit, but is beyond the scope of
					// this prototype
					.Take(maxNumberOfResults)
					// resolve the set, do further processing in the
					// webservers memory, which is easier to scale horizontally
					// than our datastore, or break out into a microservice.
					.ToListAsync()
				)
				// put in some alphabetic ordering.
				// This was not part of the spec but takes no work
				// and improves the value of the output significantly.
				.OrderBy(person =>
					person.FirstName
				)
				.ThenBy(person =>
					person.LastName
				)
			;

			return Ok(new PersonSearchResult { PersonSet = searchResultSet });
		}
	}
}
