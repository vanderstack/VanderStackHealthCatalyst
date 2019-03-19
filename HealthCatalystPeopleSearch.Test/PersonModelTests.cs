using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VanderStack.HealthCatalystPeopleSearch.PersonFeature;
using Xunit;

namespace VanderStack.HealthCatalystPeopleSearch.Tests
{
	/// <summary>
	/// Tests for the <see cref="PersonModel"/>
	/// </summary>
	public class PersonModelTests
	{
		/// <summary>
		/// Tests that the Last Name is Required.
		/// </summary>
		[Fact]
		public void PersonLastNameRequired()
		{
			// Arrange
			// Create a test person without a Last Name
			var testPerson =
				new PersonModel()
				{
					Age = 14
					, FirstName = "foo"
					, LastName = ""
					, Id = Guid.NewGuid()
				}
			;

			var validationResults = new List<ValidationResult>();
			var validationContext = new ValidationContext(testPerson, null, null);

			// Act
			// Validate the Test Person
			Validator.TryValidateObject(testPerson, validationContext, validationResults, true);
			(testPerson as IValidatableObject)?.Validate(validationContext);

			// Assert
			// Verify we have validation errors
			Assert.NotEmpty(validationResults);
			Assert.Single(validationResults);
		}

		/// <summary>
		/// Tests that the First Name is Required.
		/// </summary>
		[Fact]
		public void PersonFirstNameRequired()
		{
			// Arrange
			// Create a test person without a First Name
			var testPerson =
				new PersonModel()
				{
					Age = 14
					, FirstName = ""
					, LastName = "bar"
					, Id = Guid.NewGuid()
				}
			;

			var validationResults = new List<ValidationResult>();
			var validationContext = new ValidationContext(testPerson, null, null);

			// Act
			// Validate the Test Person
			Validator.TryValidateObject(testPerson, validationContext, validationResults, true);
			(testPerson as IValidatableObject)?.Validate(validationContext);

			// Assert
			// Verify we have validation errors
			Assert.NotEmpty(validationResults);
			Assert.Single(validationResults);
		}

		/// <summary>
		/// Tests that the minimum age is 13.
		/// </summary>
		[Fact]
		public void PersonAgeCannotBeYoungerThan13()
		{
			// Arrange
			// Create a test person under the age of 13.
			var testPerson =
				new PersonModel()
				{
					Age = 12
					, FirstName = "foo"
					, LastName = "bar"
					, Id = Guid.NewGuid()
				}
			;

			var validationResults = new List<ValidationResult>();
			var validationContext = new ValidationContext(testPerson, null, null);

			// Act
			// Validate the Test Person
			Validator.TryValidateObject(testPerson, validationContext, validationResults, true);
			(testPerson as IValidatableObject)?.Validate(validationContext);

			// Assert
			// Verify we have validation errors
			Assert.NotEmpty(validationResults);
			Assert.Single(validationResults);
		}

		/// <summary>
		/// Tests that the maximum age is 130
		/// </summary>
		[Fact]
		public void PersonAgeCannotBeOlderThan131()
		{
			// Arrange
			// Create a test person over the Age of 130
			var testPerson =
				new PersonModel()
				{
					Age = 131
					, FirstName = "foo"
					, LastName = "bar"
					, Id = Guid.NewGuid()
				}
			;

			var validationResults = new List<ValidationResult>();
			var validationContext = new ValidationContext(testPerson, null, null);

			// Act
			// Validate the Test Person
			Validator.TryValidateObject(testPerson, validationContext, validationResults, true);
			(testPerson as IValidatableObject)?.Validate(validationContext);

			// Assert
			// Verify we have validation errors
			Assert.NotEmpty(validationResults);
			Assert.Single(validationResults);
		}
	}
}