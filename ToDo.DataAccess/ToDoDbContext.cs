
using Microsoft.EntityFrameworkCore;

using ToDo.Domain.Models;

namespace ToDo.DataAccess;

public class ToDoDbContext : DbContext
{
    public DbSet<ToDoItem> ToDoItems { get; set; }

    public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options)
    { }
}
