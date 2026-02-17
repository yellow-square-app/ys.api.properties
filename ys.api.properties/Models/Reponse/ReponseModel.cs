namespace ys.api.properties.Models.Reponse;

/// <summary>
/// Represents a standard response model for API interactions, encapsulating the response status, message, returned data, and any associated exception.
/// </summary>
/// <typeparam Name="T">The type of the data being returned in the response.</typeparam>
public class ResponseModel<T>
{
    /// <summary>
    /// Gets or sets the status of the response.
    /// A value of <c>true</c> indicates success, while <c>false</c> indicates failure.
    /// </summary>
    public bool Status { get; set; }

    /// <summary>
    /// Gets or sets the message associated with the response.
    /// This can provide additional context or detail, such as error descriptions or success messages.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets the data returned in the response.
    /// The type of the data is specified by the generic type parameter <typeparamref Name="T"/>.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Gets or sets the exception information associated with the response, if any.
    /// This is typically populated when an error occurs.
    /// </summary>
    public Exception? Exception { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResponseModel{T}"/> class.
    /// By default, the <see cref="Status"/> property is set to <c>true</c>.
    /// </summary>
    public ResponseModel()
    {
        Status = true;
    }
}
