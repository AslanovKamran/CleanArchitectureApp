namespace WildPathApp.Core.Domain.Api;

public class ApiRequest<T>
{
    public required T Data { get; set; }
}
