using Store.Contexts;
using Store.Contexts.Abstractions;
using Store.Dto;
using Store.Services;
using Store.Services.Abstractions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<DbOptions>(builder.Configuration.GetSection("DbOptions"));
builder.Services.AddSingleton<ILiteDbContext, DbContext>();
builder.Services.AddTransient<IMoneyTransactionService, MoneyTransactionService>();
builder.Services.AddControllers();

var app = builder.Build();
app.UseRouting();
app.MapControllers();
app.Run();
