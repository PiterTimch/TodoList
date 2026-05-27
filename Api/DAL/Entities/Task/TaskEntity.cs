using DAL.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities.Task;

[Table("tbl_tasks")]
public class TaskEntity : BaseEntity<long>
{
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(300)]
    public string Description { get; set; } = null!;

    public DateTime? DueDate { get; set; }

    public bool IsCompleted { get; set; }
}
