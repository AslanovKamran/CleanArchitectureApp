using System.Net;

namespace WildPathApp.Core.Domain.Api;

public class ApiResponse<T>
{
    public T? Data { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public string? Message { get; set; }
    public bool Success { get; set; }

    public ApiResponse(){ }

    public ApiResponse(T? data, HttpStatusCode statusCode, string? message, bool success)
    {
        Data = data;
        StatusCode = statusCode;
        Message = message;
        Success = success;
    }
}
