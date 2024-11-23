namespace WildPathApp.Core.Domain.Exceptions;

public class RecordNotFoundException : Exception
{
    public string EntityName { get; }

    public RecordNotFoundException(string entityName) : base($"{entityName} Not Found!")
    {
        EntityName = entityName;
    }

    public override string ToString() => Message;

}
