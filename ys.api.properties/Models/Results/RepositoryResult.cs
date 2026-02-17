namespace ys.api.properties.Models.Results;

/// <summary>
/// Enum representing the status of the repository operation.
/// </summary>
public enum StatusCode
{
    /// <summary>
    /// Indicates the operation was successful.
    /// </summary>
    Success,

    /// <summary>
    /// Indicates the operation failed.
    /// </summary>
    Failure
}

/// <summary>
/// Represents the result of a repository operation, including status, message, exception, and data.
/// </summary>
/// <typeparam name="T">The type of data returned by the repository operation.</typeparam>
public class RepositoryResult<T>
{
    /// <summary>
    /// Gets or sets the status of the repository operation.
    /// </summary>
    public required StatusCode Status { get; init; }

    /// <summary>
    /// Gets or sets the message associated with the repository operation.
    /// </summary>
    public required string Message { get; init; }

    /// <summary>
    /// Gets or sets the exception that occurred during the repository operation, if any.
    /// </summary>
    public Exception? Exception { get; init; }

    /// <summary>
    /// Gets or sets the data returned by the repository operation, if any.
    /// </summary>
    public T? Data { get; init; }

    /// <summary>
    /// Creates a successful repository result.
    /// </summary>
    /// <param name="data">The data returned by the operation.</param>
    /// <param name="message">A message describing the success.</param>
    /// <returns>A <see cref="RepositoryResult{T}"/> with a success status.</returns>
    public static RepositoryResult<T> SuccessResult(T data, string message)
        => new RepositoryResult<T> { Status = StatusCode.Success, Data = data, Message = message };

    /// <summary>
    /// Creates a failed repository result.
    /// </summary>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <returns>A <see cref="RepositoryResult{T}"/> with a failure status.</returns>
    public static RepositoryResult<T> FailureResult(Exception exception)
        => new RepositoryResult<T> { Status = StatusCode.Failure, Exception = exception, Message = exception.Message };
}
