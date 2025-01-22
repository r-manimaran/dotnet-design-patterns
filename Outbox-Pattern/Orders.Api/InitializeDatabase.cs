using Dapper;
using Npgsql;

namespace Orders.Api;

internal sealed class InitializeDatabase (
                NpgsqlDataSource dataSource,
                IConfiguration configuration,
                ILogger<InitializeDatabase> logger)
{

    public async Task Execute(CancellationToken stoppingToken =default)
    {
        try
        {
            logger.LogInformation("Starting the database initialization.");
            await EnsureDatabaseExists();
            await IntializeDB();

            logger.LogInformation("Database initialization completed successfully.");
        }
        catch (Exception ex) 
        {
            logger.LogError(ex, "An Error occured while initializing the database.");
        }
    }

    private async Task EnsureDatabaseExists()
    {
        string connectionString = configuration.GetConnectionString("Database");
        var builder = new NpgsqlConnectionStringBuilder(connectionString);
        string? databaseName = builder.Database;
        builder.Database = "postgres";

        using var connection = new NpgsqlConnection(builder.ToString());
        await connection.OpenAsync();

        bool databaseExists = await connection.ExecuteScalarAsync<bool>(
            "SELECT Exists(Select 1 FROM pg_database where datName = @databaseName)",
            new {databaseName});

        if (!databaseExists)
        {
            logger.LogInformation("Creating database {DatabaseName}",databaseName);
            await connection.ExecuteAsync($"CREATE DATABASE {databaseName}");

        }
    }

    private async Task IntializeDB()
    {
        const string sql =
            """
            -- Create order table if it doesn't exist
            CREATE TABLE IF NOT EXISTS orders(
                id UUID Primary KEY,
                customer_name VARCHAR(255) NOT NULL,
                product_name VARCHAR(255) NOT NULL,
                quantity INTEGER NOT NULL,
                total_price DECIMAL(18,2) NOT NULL,
                order_date TIMESTAMP WITH TIME ZONE NOT NULL
            );

             -- Create Outbox table if it does not exists
            CREATE TABLE IF NOT EXISTS outbox_message (
            	id UUID PRIMARY KEY,
            	type VARCHAR(255) NOT NULL,
            	content_JSONB NOT NULL,
            	occured_on_utc TIMESTAMP WITH TIME ZONE NOT NULL,
            	processed_on_utc TIMESTAMP WITH TIME ZONE  NULL,
            	error TEXT NULL
            );

            """;
        using NpgsqlConnection connection = await dataSource.OpenConnectionAsync();
        await connection.ExecuteAsync(sql);
    }

}
