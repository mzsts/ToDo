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
            switch(value)
            {
                case ToDoItemStatus.Suspended:
                    status = status == ToDoItemStatus.InProgress ? value : status;
                    ResetCopletionDateTime();
                    break;
                case ToDoItemStatus.Compleated:
                    if (CanBeCompleated())
                    {
                        SetCompleated();
                    }
                    break;
                default:
                    status = value;
                    ResetCopletionDateTime();
                    break;
            }
        } 
    }

    public DateTime? CreationDate { get; set; } = DateTime.Now;

    public long EstimationTimePeriodTicks { get; set; }
    public long CompletionTimePeriodTicks { get; set; }

    [Required(ErrorMessage = "EstimationTime is required")]
    [NotMapped]
    public TimeSpan? EstimationTime 
    {
        get => TimeSpan.FromTicks(EstimationTimePeriodTicks);
        set => EstimationTimePeriodTicks = value.HasValue ? value.Value.Ticks : 0;
    }

    [NotMapped]
    public TimeSpan? CompletionTime 
    {
        get => TimeSpan.FromTicks(CompletionTimePeriodTicks);
        set => CompletionTimePeriodTicks = value.HasValue ? value.Value.Ticks : 0;
    }

    public DateTime? CompletionDate { get; set; }

    public ICollection<ToDoItem> SubItems { get; set; } = new List<ToDoItem>();

    private void SetCompleated()
    {
        status = ToDoItemStatus.Compleated;

        foreach(var item in SubItems)
        {
            item.SetCompleated();
        }

        SetCompletionTime();
    }

    private bool CanBeCompleated()
    {
        return (status is ToDoItemStatus.InProgress or ToDoItemStatus.Compleated) && IsSubItemsCanBeCompleated();
    }

    private bool IsSubItemsCanBeCompleated()
    {
        if (SubItems is null || SubItems?.Any() is false)
        {
            return true;
        }

        foreach (var item in SubItems)
        {
            if (item.CanBeCompleated() is false)
            {
                return false;
            }
        }

        return true;
    }

    private void ResetCopletionDateTime()
    {
        CompletionDate = null;
        CompletionTime = null;
    }

    private void SetCompletionTime()
    {
        CompletionDate = DateTime.Now;
        CompletionTime = CompletionDate?.Subtract(CreationDate.Value);
    }
}
