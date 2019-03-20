namespace VanderStack.HealthCatalystPeopleSearch
{
	public class AppSettings
	{
		public bool UseInMemoryDatabase { get; set; }
		public string InMemoryDatabaseName { get; set; }
		public bool UseSqlDatabase { get; set; }
		public bool SeedSqlDatabaseOnStart { get; set; }
		public string SqlDatabaseConnectionString { get; set; }
		public int PersonSearchMaxResults { get; set; }
	}
}
