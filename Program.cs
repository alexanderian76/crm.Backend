using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddDbContext<CrmDbContext>(options =>
{
	var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

	if (string.IsNullOrEmpty(connectionString))
	{

	}
	else
	{
		options.UseNpgsql(connectionString,
			npgsqlOptions =>
			{
				npgsqlOptions.MigrationsAssembly(typeof(Program).Assembly.FullName);
				npgsqlOptions.EnableRetryOnFailure(
					maxRetryCount: 5,
					maxRetryDelay: TimeSpan.FromSeconds(30),
					errorCodesToAdd: null);
			});
	}
});

new Configuration(builder.Services);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}



/*
using (var db = new CrmDbContext())
{
	db.Database.Migrate();
}*/
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseStaticFiles();


app.UseAuthentication();
app.UseAuthorization();

app.UseWebSockets();

app.MapControllers();

app.MapHealthChecks("/healthCheck");

app.Run();

