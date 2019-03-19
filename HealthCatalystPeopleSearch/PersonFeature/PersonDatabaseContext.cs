using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VanderStack.HealthCatalystPeopleSearch.PersonFeature
{
    public class PersonDatabaseContext
		: DbContext
	{
		public PersonDatabaseContext(DbContextOptions<PersonDatabaseContext> options)
		: base(options)
		{ }

		public DbSet<PersonModel> PersonSet { get; set; }
	}
}
