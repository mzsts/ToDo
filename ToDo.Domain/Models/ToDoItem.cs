using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using ToDo.Domain.Enums;

namespace ToDo.Domain.Models;

public class ToDoItem
{
    private ToDoItemStatus status = ToDoItemStatus.Assigned;

    [Key]
    public int Id { get; set; }
    
    [NotMapped]
    public bool HasSubItems => SubItems is not null && SubItems.Any();

    public bool IsSubItem { get; set; }

    [MinLength(5)]
    [MaxLength(40)]
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; }

    [MinLength(5)]
    [Required(ErrorMessage = "Description is required")]
    public string Description { get; set; }

    public ToDoItemStatus Status 
    {
        get => status;
        set 
        {
            status = value switch
            {
                ToDoItemStatus.Suspended => status == ToDoItemStatus.InProgress ? ToDoItemStatus.Suspended : status,
                ToDoItemStatus.Compleated => status == ToDoItemStatus.InProgress && CanBeCompleated() ? SetCompleated() : status,
                _ => value
            };
        } 
    }

    public DateTime? CreationDate { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "EstimationTime is required")]
    public TimeSpan? EstimationTime { get; set; }

    public TimeSpan? CompletionTime { get; set; }

    public DateTime? CompletionDate { get; set; }

    public ICollection<ToDoItem> SubItems { get; set; } = new List<ToDoItem>();

    private ToDoItemStatus SetCompleated()
    {
        foreach(var item in SubItems)
        {
            item.Status = SetCompleated();
        }

        SetCompletionTime();

        return ToDoItemStatus.Compleated;
    }

    private bool CanBeCompleated()
    {
        return status is ToDoItemStatus.InProgress && IsSubItemsCanBeCompleated();
    }

    private bool IsSubItemsCanBeCompleated()
    {
        if (SubItems is null || SubItems?.Any() is false)
        {
            return true;
        }

        foreach (var item in SubItems)
        {
            if (item.CanBeCompleated() is false || item.Status is not ToDoItemStatus.Compleated)
            {
                return false;
            }
        }

        return true;
    }

    private void SetCompletionTime()
    {
        CompletionDate = DateTime.Now;
        CompletionTime = CreationDate?.Subtract(CompletionDate.Value);
    }
}
