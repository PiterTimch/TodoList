using System.ComponentModel.DataAnnotations;

namespace DAL.Entities.Base;

public abstract class BaseEntity<T> 
{
    [Key]
    public T Id { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}