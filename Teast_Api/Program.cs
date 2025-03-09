var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var conncationsString = builder.Configuration.GetConnectionString("DefaultConnection")
               ?? throw new InvalidOperationException("ConnectionString Not Found");
// Connection String Virsions
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(conncationsString));

builder.Services.AddApplicationServices();

// Add Controllers
builder.Services.AddControllers();

//  Injection BaseRpositoryPattern
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Add ExceptionHandlingMiddleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Redirect root to swagger
app.MapGet("/", () => Results.Redirect("/swagger"));
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
