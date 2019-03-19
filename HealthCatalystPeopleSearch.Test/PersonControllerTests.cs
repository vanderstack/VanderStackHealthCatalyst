using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using VanderStack.HealthCatalystPeopleSearch.PersonFeature;
using Xunit;

namespace VanderStack.HealthCatalystPeopleSearch.Tests
{
	/// <summary>
	/// A set of tests for the <see cref="PersonController"/>
	/// </summary>
	public class PersonControllerTests
    {
		/// <summary>
		/// Tests that the search endpoint can return results
		/// </summary>
		[Fact]
		public async Task SearchReturnsResults()
		{
			// Arrange
			var databaseContext =
				new PersonDatabaseContext(
					new DbContextOptionsBuilder<PersonDatabaseContext>()
						.UseInMemoryDatabase(Guid.NewGuid().ToString())
						.Options
				)
			;

			var testFirstName = "foo";

			var testPerson =
				new PersonModel()
				{
					Age = 14
					, FirstName = testFirstName
					, LastName = "bar"
					, Id = Guid.NewGuid()
				}
			;

			databaseContext.PersonSet.Add(testPerson);
			databaseContext.SaveChanges();

			var personSearchOptionsMock = new Mock<IOptionsSnapshot<PersonSearchOptions>>();

			personSearchOptionsMock
				.Setup(personSearchOptions =>
					personSearchOptions.Value
				)
				.Returns(new PersonSearchOptions
				{
					MaxNumberOfResults = 100
				})
			;

			var controller =
				new PersonController(
					databaseContext: databaseContext
					, personSearchOptions: personSearchOptionsMock.Object
				)
			;

			// Act
			var result = await controller.Search(personName: testFirstName);

			// Assert
			var viewResult = Assert.IsType<OkObjectResult>(result);
			var model =
				Assert
				.IsAssignableFrom<PersonSearchResult>(
					viewResult
					.Value
				)
			;

			Assert.NotEmpty(model.PersonSet);
		}

		/// <summary>
		/// Tests that the search endpoint does not return any results
		/// when no input search string is provided
		/// </summary>
		[Fact]
		public async Task EmptySearchStringReturnsNoResults()
		{
			// Arrange
			var databaseContext =
				new PersonDatabaseContext(
					new DbContextOptionsBuilder<PersonDatabaseContext>()
						.UseInMemoryDatabase(Guid.NewGuid().ToString())
						.Options
				)
			;

			var testFirstName = "foo";

			var testPerson =
				new PersonModel()
				{
					Age = 14
					, FirstName = testFirstName
					, LastName = "bar"
					, Id = Guid.NewGuid()
				}
			;

			databaseContext.PersonSet.Add(testPerson);
			databaseContext.SaveChanges();

			var personSearchOptionsMock = new Mock<IOptionsSnapshot<PersonSearchOptions>>();

			personSearchOptionsMock
				.Setup(personSearchOptions =>
					personSearchOptions.Value
				)
				.Returns(new PersonSearchOptions
				{
					MaxNumberOfResults = 100
				})
			;

			var controller =
				new PersonController(
					databaseContext: databaseContext
					, personSearchOptions: personSearchOptionsMock.Object
				)
			;

			// Act
			var result = await controller.Search(personName: string.Empty);

			// Assert
			var viewResult = Assert.IsType<OkObjectResult>(result);
			var model =
				Assert
				.IsAssignableFrom<PersonSearchResult>(
					viewResult
					.Value
				)
			;

			Assert.Empty(model.PersonSet);
		}

		/// <summary>
		/// Tests that the search endpoint will return multiple results which all
		/// match the provided search criteria.
		/// </summary>
		[Fact]
		public async Task PersonSearchReturnsMultipleResults()
		{
			// Arrange
			var databaseContext =
				new PersonDatabaseContext(
					new DbContextOptionsBuilder<PersonDatabaseContext>()
						.UseInMemoryDatabase(Guid.NewGuid().ToString())
						.Options
				)
			;

			var testFirstName = "foo";

			var testPerson1 =
				new PersonModel()
				{
					Age = 14
					, FirstName = testFirstName
					, LastName = "bar"
					, Id = Guid.NewGuid()
				}
			;

			var testPerson2 =
				new PersonModel()
				{
					Age = 16
					, FirstName = testFirstName
					, LastName = "baz"
					, Id = Guid.NewGuid()
				}
			;

			databaseContext.PersonSet.Add(testPerson1);
			databaseContext.PersonSet.Add(testPerson2);
			databaseContext.SaveChanges();

			var personSearchOptionsMock = new Mock<IOptionsSnapshot<PersonSearchOptions>>();

			personSearchOptionsMock
				.Setup(personSearchOptions =>
					personSearchOptions.Value
				)
				.Returns(new PersonSearchOptions
				{
					MaxNumberOfResults = 100
				})
			;

			var controller =
				new PersonController(
					databaseContext: databaseContext
					, personSearchOptions: personSearchOptionsMock.Object
				)
			;

			// Act
			var result = await controller.Search(personName: testFirstName);

			// Assert
			var viewResult = Assert.IsType<OkObjectResult>(result);
			var model =
				Assert
				.IsAssignableFrom<PersonSearchResult>(
					viewResult
					.Value
				)
			;

			Assert.NotEmpty(model.PersonSet);
			Assert.NotEqual(1, model.PersonSet.Count());
		}

		/// <summary>
		/// Tests that configuring the <see cref="PersonSearchOptions"/> will
		/// limit the number of People returned in the search.
		/// </summary>
		[Fact]
		public async Task PersonSearchOptionsLimitResults()
		{
			// Arrange
			var databaseContext =
				new PersonDatabaseContext(
					new DbContextOptionsBuilder<PersonDatabaseContext>()
						.UseInMemoryDatabase(Guid.NewGuid().ToString())
						.Options
				)
			;

			var testFirstName = "foo";

			var testPerson1 =
				new PersonModel()
				{
					Age = 14
					, FirstName = testFirstName
					, LastName = "bar"
					, Id = Guid.NewGuid()
				}
			;

			var testPerson2 =
				new PersonModel()
				{
					Age = 16
					, FirstName = testFirstName
					, LastName = "baz"
					, Id = Guid.NewGuid()
				}
			;

			databaseContext.PersonSet.Add(testPerson1);
			databaseContext.PersonSet.Add(testPerson2);
			databaseContext.SaveChanges();

			var personSearchOptionsMock = new Mock<IOptionsSnapshot<PersonSearchOptions>>();

			var testNumberOfResults = 1;

			personSearchOptionsMock
				.Setup(personSearchOptions =>
					personSearchOptions.Value
				)
				.Returns(new PersonSearchOptions
				{
					MaxNumberOfResults = testNumberOfResults
				})
			;

			var controller =
				new PersonController(
					databaseContext: databaseContext
					, personSearchOptions: personSearchOptionsMock.Object
				)
			;

			// Act
			var result = await controller.Search(personName: testFirstName);

			// Assert
			var viewResult = Assert.IsType<OkObjectResult>(result);
			var model =
				Assert
				.IsAssignableFrom<PersonSearchResult>(
					viewResult
					.Value
				)
			;

			Assert.Equal(testNumberOfResults, model.PersonSet.Count());
		}

		/// <summary>
		/// Tests that whitespace surrounding a search string is ignored.
		/// </summary>
		[Fact]
		public async Task WhitespaceAroundSearchStringIgnored()
		{
			// Arrange
			var databaseContext =
				new PersonDatabaseContext(
					new DbContextOptionsBuilder<PersonDatabaseContext>()
						.UseInMemoryDatabase(Guid.NewGuid().ToString())
						.Options
				)
			;

			var testFirstName = "foo";

			var testPerson1 =
				new PersonModel()
				{
					Age = 14
					, FirstName = testFirstName
					, LastName = "bar"
					, Id = Guid.NewGuid()
				}
			;

			databaseContext.PersonSet.Add(testPerson1);
			databaseContext.SaveChanges();

			var personSearchOptionsMock = new Mock<IOptionsSnapshot<PersonSearchOptions>>();

			personSearchOptionsMock
				.Setup(personSearchOptions =>
					personSearchOptions.Value
				)
				.Returns(new PersonSearchOptions
				{
					MaxNumberOfResults = 100
				})
			;

			var controller =
				new PersonController(
					databaseContext: databaseContext
					, personSearchOptions: personSearchOptionsMock.Object
				)
			;

			// Act
			var result = await controller.Search(personName: $" {testFirstName} ");

			// Assert
			var viewResult = Assert.IsType<OkObjectResult>(result);
			var model =
				Assert
				.IsAssignableFrom<PersonSearchResult>(
					viewResult
					.Value
				)
			;

			Assert.NotEmpty(model.PersonSet);
		}

		/// <summary>
		/// Tests that the case will be ignored when evaluating a search string.
		/// </summary>
		[Fact]
		public async Task SearchStringIsNotCaseSensitive()
		{
			// Arrange
			var databaseContext =
				new PersonDatabaseContext(
					new DbContextOptionsBuilder<PersonDatabaseContext>()
						.UseInMemoryDatabase(Guid.NewGuid().ToString())
						.Options
				)
			;

			var testFirstName = "foo";

			var testPerson1 =
				new PersonModel()
				{
					Age = 14
					, FirstName = testFirstName.ToLower()
					, LastName = "bar"
					, Id = Guid.NewGuid()
				}
			;

			databaseContext.PersonSet.Add(testPerson1);
			databaseContext.SaveChanges();

			var personSearchOptionsMock = new Mock<IOptionsSnapshot<PersonSearchOptions>>();

			personSearchOptionsMock
				.Setup(personSearchOptions =>
					personSearchOptions.Value
				)
				.Returns(new PersonSearchOptions
				{
					MaxNumberOfResults = 100
				})
			;

			var controller =
				new PersonController(
					databaseContext: databaseContext
					, personSearchOptions: personSearchOptionsMock.Object
				)
			;

			// Act
			var result = await controller.Search(personName: testFirstName.ToUpper());

			// Assert
			var viewResult = Assert.IsType<OkObjectResult>(result);
			var model =
				Assert
				.IsAssignableFrom<PersonSearchResult>(
					viewResult
					.Value
				)
			;

			Assert.NotEmpty(model.PersonSet);
		}

		/// <summary>
		/// Tests that partial matches return results, specifically where
		/// the search criteria is the entire First name and the first
		/// character of the Last Name
		/// </summary>
		[Fact]
		public async Task FirstNameLastNameFirstCharacterReturnsResults()
		{
			// Arrange
			var databaseContext =
				new PersonDatabaseContext(
					new DbContextOptionsBuilder<PersonDatabaseContext>()
						.UseInMemoryDatabase(Guid.NewGuid().ToString())
						.Options
				)
			;

			var testFirstName = "foo";
			var testLastName = "bar";

			var testPerson1 =
				new PersonModel()
				{
					Age = 14
					, FirstName = testFirstName
					, LastName = testLastName
					, Id = Guid.NewGuid()
				}
			;

			databaseContext.PersonSet.Add(testPerson1);
			databaseContext.SaveChanges();

			var personSearchOptionsMock = new Mock<IOptionsSnapshot<PersonSearchOptions>>();

			personSearchOptionsMock
				.Setup(personSearchOptions =>
					personSearchOptions.Value
				)
				.Returns(new PersonSearchOptions
				{
					MaxNumberOfResults = 100
				})
			;

			var controller =
				new PersonController(
					databaseContext: databaseContext
					, personSearchOptions: personSearchOptionsMock.Object
				)
			;

			// Act
			var result = await controller.Search(personName: $"{testFirstName} {testLastName.First()}");

			// Assert
			var viewResult = Assert.IsType<OkObjectResult>(result);
			var model =
				Assert
				.IsAssignableFrom<PersonSearchResult>(
					viewResult
					.Value
				)
			;

			Assert.NotEmpty(model.PersonSet);
		}

		/// <summary>
		/// Tests that partial matches return results, specifically where
		/// the search criteria is the last character of the first name 
		/// and the entire Last Name
		/// </summary>
		[Fact]
		public async Task FirstNameLastCharacterLastNameReturnsResults()
		{
			// Arrange
			var databaseContext =
				new PersonDatabaseContext(
					new DbContextOptionsBuilder<PersonDatabaseContext>()
						.UseInMemoryDatabase(Guid.NewGuid().ToString())
						.Options
				)
			;

			var testFirstName = "foo";
			var testLastName = "bar";

			var testPerson1 =
				new PersonModel()
				{
					Age = 14
					, FirstName = testFirstName
					, LastName = testLastName
					, Id = Guid.NewGuid()
				}
			;

			databaseContext.PersonSet.Add(testPerson1);
			databaseContext.SaveChanges();

			var personSearchOptionsMock = new Mock<IOptionsSnapshot<PersonSearchOptions>>();

			personSearchOptionsMock
				.Setup(personSearchOptions =>
					personSearchOptions.Value
				)
				.Returns(new PersonSearchOptions
				{
					MaxNumberOfResults = 100
				})
			;

			var controller =
				new PersonController(
					databaseContext: databaseContext
					, personSearchOptions: personSearchOptionsMock.Object
				)
			;

			// Act
			var result = await controller.Search(personName: $"{testFirstName.Last()} {testLastName}");

			// Assert
			var viewResult = Assert.IsType<OkObjectResult>(result);
			var model =
				Assert
				.IsAssignableFrom<PersonSearchResult>(
					viewResult
					.Value
				)
			;

			Assert.NotEmpty(model.PersonSet);
		}

		/// <summary>
		/// Tests that when the model is valid the post endpoint returns an Ok result.
		/// </summary>
		[Fact]
		public async Task PostReturnsOkForValidModel()
		{
			// Arrange
			var databaseContext =
				new PersonDatabaseContext(
					new DbContextOptionsBuilder<PersonDatabaseContext>()
						.UseInMemoryDatabase(Guid.NewGuid().ToString())
						.Options
				)
			;

			var testPerson =
				new PersonModel()
				{
					Age = 14
					, FirstName = "foo"
					, LastName = "bar"
					, Id = Guid.NewGuid()
				}
			;

			var personSearchOptionsMock = new Mock<IOptionsSnapshot<PersonSearchOptions>>();

			personSearchOptionsMock
				.Setup(personSearchOptions =>
					personSearchOptions.Value
				)
				.Returns(new PersonSearchOptions
				{
					MaxNumberOfResults = 100
				})
			;

			var controller =
				new PersonController(
					databaseContext: databaseContext
					, personSearchOptions: personSearchOptionsMock.Object
				)
			;

			// Act
			var result = await controller.Post(model: testPerson);

			// Assert
			var viewResult = Assert.IsType<OkResult>(result);
		}

		/// <summary>
		/// Tests that when the model is valid the post endpoint adds a new record.
		/// </summary>
		[Fact]
		public async Task PostAddsNewRecordForValidModel()
		{
			// Arrange
			var databaseContext =
				new PersonDatabaseContext(
					new DbContextOptionsBuilder<PersonDatabaseContext>()
						.UseInMemoryDatabase(Guid.NewGuid().ToString())
						.Options
				)
			;

			var testPerson =
				new PersonModel()
				{
					Age = 14
					, FirstName = "foo"
					, LastName = "bar"
					, Id = Guid.NewGuid()
				}
			;

			var personSearchOptionsMock = new Mock<IOptionsSnapshot<PersonSearchOptions>>();

			personSearchOptionsMock
				.Setup(personSearchOptions =>
					personSearchOptions.Value
				)
				.Returns(new PersonSearchOptions
				{
					MaxNumberOfResults = 100
				})
			;

			var controller =
				new PersonController(
					databaseContext: databaseContext
					, personSearchOptions: personSearchOptionsMock.Object
				)
			;

			// Act
			var result = await controller.Post(model: testPerson);

			// Assert
			var viewResult = Assert.IsType<OkResult>(result);
			Assert.NotEmpty(databaseContext.PersonSet);
		}

		/// <summary>
		/// Verifies that invoking the Post action on the <see cref="PersonController"/>
		/// when the provided model will cause validation errors results in a
		/// <see cref="BadRequestObjectResult"/> containing a <see cref="PersonValidationResult"/>
		/// with a set of validation error messages.
		/// </summary>
		/// <remarks>
		/// Only testing that controller responds correctly to model errors.
		/// does not test model validation conditions as advised here:
		/// https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/testing?view=aspnetcore-2.2#unit-tests-of-controller-logic
		/// </remarks>
		[Fact]
		public async Task PostReturnsValidationErrorForInvalidModel()
		{
			// Arrange
			var databaseContext =
				new PersonDatabaseContext(
					new DbContextOptionsBuilder<PersonDatabaseContext>()
						.UseInMemoryDatabase(Guid.NewGuid().ToString())
						.Options
				)
			;

			var testPerson =
				new PersonModel()
				{
					Age = 131
					, FirstName = "foo"
					, LastName = "bar"
					, Id = Guid.NewGuid()
				}
			;

			var personSearchOptionsMock = new Mock<IOptionsSnapshot<PersonSearchOptions>>();

			personSearchOptionsMock
				.Setup(personSearchOptions =>
					personSearchOptions.Value
				)
				.Returns(new PersonSearchOptions
				{
					MaxNumberOfResults = 100
				})
			;

			var controller =
				new PersonController(
					databaseContext: databaseContext
					, personSearchOptions: personSearchOptionsMock.Object
				)
			;

			controller.ModelState.AddModelError("error", "some error");

			// Act
			var result = await controller.Post(model: testPerson);

			// Assert
			var viewResult = Assert.IsType<BadRequestObjectResult>(result);
			var model =
				Assert
				.IsAssignableFrom<PersonValidationResult>(
					viewResult
					.Value
				)
			;

			Assert.NotEmpty(model.ValidationErrors);
		}
	}
}