namespace ys.api.properties.Helpers;

public class Helpers: IHelpers
{

    /// <summary>
    /// Converts PostgreSQL URI format (Railway) to standard connection string format.
    /// Example: postgresql://user:pass@host:port/db -> Host=host;Port=port;Database=db;Username=user;Password=pass
    /// </summary>
    public string ConvertPostgresUriToConnectionString(string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("Connection string cannot be null or empty");
        }

        connectionString = connectionString.Trim();

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("Connection string cannot be null or empty after trimming");
        }

        var postgresqlIndex = connectionString.IndexOf("postgresql://", StringComparison.OrdinalIgnoreCase);
        var postgresIndex = connectionString.IndexOf("postgres://", StringComparison.OrdinalIgnoreCase);

        if (postgresqlIndex > 0)
        {
            connectionString = connectionString.Substring(postgresqlIndex);
        }
        else if (postgresIndex > 0)
        {
            connectionString = connectionString.Substring(postgresIndex);
        }

        if (connectionString.Contains("Host=", StringComparison.OrdinalIgnoreCase))
        {
            return connectionString;
        }

        if (connectionString.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase) ||
            connectionString.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase))
        {
            try
            {
                var withoutProtocol = connectionString.Substring(connectionString.IndexOf("://", StringComparison.Ordinal) + 3);

                var atIndex = withoutProtocol.LastIndexOf('@');
                if (atIndex < 0)
                {
                    throw new ArgumentException("Invalid connection string format: missing '@' separator");
                }

                var credentialsPart = withoutProtocol.Substring(0, atIndex);
                var hostPart = withoutProtocol.Substring(atIndex + 1);

                var colonIndex = credentialsPart.IndexOf(':');
                var username = colonIndex > 0 ? Uri.UnescapeDataString(credentialsPart.Substring(0, colonIndex)) : credentialsPart;
                var password = colonIndex > 0 ? Uri.UnescapeDataString(credentialsPart.Substring(colonIndex + 1)) : "";

                var slashIndex = hostPart.IndexOf('/');
                var database = slashIndex >= 0 ? hostPart.Substring(slashIndex + 1) : "";

                if (database.Contains('?'))
                {
                    database = database.Substring(0, database.IndexOf('?'));
                }

                if (database.Contains("postgresql://", StringComparison.OrdinalIgnoreCase) ||
                    database.Contains("postgres://", StringComparison.OrdinalIgnoreCase))
                {
                    var lastSlashIndex = database.LastIndexOf('/');
                    if (lastSlashIndex >= 0 && lastSlashIndex < database.Length - 1)
                    {
                        database = database.Substring(lastSlashIndex + 1);
                    }
                }

                database = database.Trim();

                if (string.IsNullOrEmpty(database))
                {
                    throw new ArgumentException("Database name not found in connection string URI");
                }

                var hostAndPort = slashIndex >= 0 ? hostPart.Substring(0, slashIndex) : hostPart;
                var portColonIndex = hostAndPort.LastIndexOf(':');
                var host = portColonIndex > 0 ? hostAndPort.Substring(0, portColonIndex) : hostAndPort;
                var port = 5432;

                if (portColonIndex > 0 && int.TryParse(hostAndPort.Substring(portColonIndex + 1), out var parsedPort))
                {
                    port = parsedPort;
                }

                var result = $"Host={host};Port={port};Database={database};Username={username};Password={password};";
                return result;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Failed to parse PostgreSQL connection URI: {ex.Message}", ex);
            }
        }

        return connectionString;
    }

}

public interface IHelpers
{
    public string ConvertPostgresUriToConnectionString(string connectionString);
}
