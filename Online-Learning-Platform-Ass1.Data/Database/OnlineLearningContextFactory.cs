namespace Online_Learning_Platform_Ass1.Data.Database;

public class OnlineLearningContextFactory : IDesignTimeDbContextFactory<OnlineLearningContext>
{
    public OnlineLearningContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, false)
            .AddJsonFile("appsettings.Development.json", true, false)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException(
                "The connection string 'DefaultConnection' was not found in appsettings.json. " +
                $"Current directory: {Directory.GetCurrentDirectory()}");

        var optionsBuilder = new DbContextOptionsBuilder<OnlineLearningContext>();
        optionsBuilder.UseSqlServer(connectionString);
        return new OnlineLearningContext(optionsBuilder.Options);
    }
}
