using Microsoft.EntityFrameworkCore;

using ToDo.DataAccess;
using ToDo.Domain.Models;

namespace ToDo.Web.Services;

public class ToDoService
{
    private readonly ToDoDbContext _context;

    public event Action OnUpdate;

    public IEnumerable<ToDoItem> ToDoItems => _context.ToDoItems.ToArray();

    public ToDoService(IDbContextFactory<ToDoDbContext> contextFactory)
    {
        _context = contextFactory.CreateDbContext();
    }

    public async Task InsertToDoItem(ToDoItem item, int parentId = 0)
    {
        if (parentId == 0)
        {
            await _context.ToDoItems.AddAsync(item);
        }
        else
        {
            item.IsSubItem = true;
            var parent = _context.ToDoItems.First(x => x.Id == parentId);
            parent.SubItems.Add(item);
            _context.Update(parent);
        }

        await _context.SaveChangesAsync();

        OnUpdate?.Invoke();
    }

    public async Task UpdateToDoItem(ToDoItem item)
    {
        _context.Update(item);
        await _context.SaveChangesAsync();

        OnUpdate?.Invoke();
    }

    public async Task UpdateSubItemsFor(ToDoItem item)
    {
        foreach (var subItem in item.SubItems)
        {
            _context.Update(subItem);
        }

        await _context.SaveChangesAsync();

        OnUpdate?.Invoke();
    }

    public async Task DeleteToDoItem(ToDoItem item)
    {
        _context.ToDoItems.Remove(item);
        await _context.SaveChangesAsync();

        OnUpdate?.Invoke();
    }
}
