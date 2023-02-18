using Microsoft.EntityFrameworkCore;
using Punica.Bp.Auditing.EFCore.Configurations;
using Punica.Bp.Ddd.EFCore.Filters;
using Punica.Bp.EFCore.Configurations;
using Punica.Bp.EFCore.Middleware;
using Punica.Bp.MultiTenancy.EFCore.Configurations;
using Punica.Bp.MultiTenancy.EFCore.Filters;
using Sample.Application.Orders;
using Sample.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediator(typeof(CreateOrderRequest));

// Add Db Context

builder.Services.AddDbContext<OrderDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddTransient<IEntityTypeConfiguration, DateTimeConfiguration>();
builder.Services.AddTransient<IEntityTypeConfiguration, AuditEntityConfigurations>();
builder.Services.AddTransient<IEntityTypeConfiguration, TenantConfiguration>();

//builder.Services.AddAuditing();
//builder.Services.AddScoped<ITrackingFilter, TenantFilter>();
//builder.Services.AddScoped<ITrackingFilter, DomainEventFilter>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
