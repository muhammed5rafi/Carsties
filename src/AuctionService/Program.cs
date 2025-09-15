using AuctionService.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddDbContext<AuctionDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDb"));
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}


app.UseAuthentication();
app.MapControllers();

try
{
    DbInitilizer.InitDb(app);
} catch (Exception ex) {
    Console.WriteLine(ex.ToString());
}
app.Run();

