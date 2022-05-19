using Microsoft.EntityFrameworkCore;

using MudBlazor.Services;

using ToDo.DataAccess;
using ToDo.Web.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextFactory<ToDoDbContext>(options => 
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("ToDo.Web")));

builder.Services.AddControllers();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddSingleton<ToDoService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRequestLocalization(GetLocalizationOptions());

app.UseRouting();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

RequestLocalizationOptions GetLocalizationOptions()
{
    var caltures = app.Configuration.GetSection("Caltures")
        .GetChildren().ToDictionary(x => x.Key, x => x.Value);

    var supportedCaltures = caltures.Keys.ToArray();

    return new RequestLocalizationOptions()
        .AddSupportedCultures(supportedCaltures)
        .AddSupportedUICultures(supportedCaltures);
}