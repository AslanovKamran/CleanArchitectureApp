namespace WildPathApp.Core.Domain.Models;

public abstract class BaseModel
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
