namespace Domain.Models;

public class ErrorResponse
{
    public string Message { get; set; }
    public string? Details { get; set; }
    public int StatusCode { get; set; }
    public DateTime Timestamp { get; set; }

    public ErrorResponse(string message, int statusCode, string? details = null)
    {
        Message = message;
        StatusCode = statusCode;
        Details = details;
        Timestamp = DateTime.UtcNow;
    }
}