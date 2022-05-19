using Microsoft.EntityFrameworkCore;

using MudBlazor.Services;

using ToDo.DataAccess;
using ToDo.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddMudServices();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextFactory<ToDoDbContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("ToDo.Web")));

builder.Services.AddSingleton<ToDoService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
