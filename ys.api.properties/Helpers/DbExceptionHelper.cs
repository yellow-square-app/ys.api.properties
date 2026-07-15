using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace ys.api.properties.Helpers;

/// <summary>
/// Extracts user-friendly error details from database exceptions.
/// </summary>
public static class DbExceptionHelper
{
    /// <summary>
    /// Extracts a human-readable detail string from a <see cref="DbUpdateException"/>.
    /// Maps common PostgreSQL error codes to clear validation messages.
    /// </summary>
    public static string ExtractDetail(DbUpdateException ex)
    {
        if (ex.InnerException is PostgresException pgEx)
        {
            return pgEx.SqlState switch
            {
                // NOT NULL violation
                "23502" => $"A required field is missing: '{pgEx.ColumnName}' cannot be null.",
                // Unique constraint violation
                "23505" => $"A duplicate value was detected: {pgEx.ConstraintName}.",
                // Foreign key violation
                "23503" => $"A referenced record does not exist: {pgEx.ConstraintName}.",
                // Check constraint violation
                "23514" => $"A value failed validation: {pgEx.ConstraintName}.",
                // String/data too long
                "22001" => "A field value exceeds the maximum allowed length.",
                // Invalid text representation (e.g. wrong type for jsonb)
                "22P02" => "A field value has an invalid format.",
                _ => pgEx.MessageText,
            };
        }

        return "An unexpected database error occurred.";
    }
}
