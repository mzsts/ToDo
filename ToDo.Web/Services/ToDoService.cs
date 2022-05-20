using Microsoft.EntityFrameworkCore;

using ToDo.DataAccess;
using ToDo.Domain.Models;

namespace ToDo.Web.Services;

public class ToDoService
{
    private readonly IDbContextFactory<ToDoDbContext> _contextFactory;

    public event Action OnUpdate;

    public IEnumerable<ToDoItem> ToDoItems => _contextFactory.CreateDbContext().ToDoItems.ToArray();

    public ToDoService(IDbContextFactory<ToDoDbContext> contextFactory) => _contextFactory = contextFactory;

    public async Task InsertToDoItem(ToDoItem item, int parentId = 0)
    {
        using(var context = await _contextFactory.CreateDbContextAsync())
        { 
            if (parentId == 0)
            {
                await context.ToDoItems.AddAsync(item);
            }
            else
            {
                item.IsSubItem = true;
                var parent = context.ToDoItems.First(x => x.Id == parentId);
                parent.SubItems.Add(item);
                context.Update(parent);
            }

            await context.SaveChangesAsync();
        }
        OnUpdate?.Invoke();
    }

    public async Task UpdateToDoItem(ToDoItem item)
    {
        using (var context = await _contextFactory.CreateDbContextAsync())
        {
            context.Update(item);
            await context.SaveChangesAsync();
        }

        OnUpdate?.Invoke();
    }

    public async Task DeleteToDoItem(ToDoItem item)
    {
        using (var context = await _contextFactory.CreateDbContextAsync())
        {
            context.ToDoItems.Remove(item);
            await context.SaveChangesAsync();
        }

        OnUpdate?.Invoke();
    }
}
