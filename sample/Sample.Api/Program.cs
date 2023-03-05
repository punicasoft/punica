using Microsoft.EntityFrameworkCore;
using Punica;
using Punica.Bp.Auditing.EFCore.Configurations;
using Punica.Bp.Core;
using Punica.Bp.Ddd.EFCore.Extensions.DependencyInjection;
using Punica.Bp.Ddd.EFCore.Configurations;
using Punica.Bp.Ddd.EFCore.Interceptors;
using Punica.Bp.Ddd.EFCore.Interceptors.Events;
using Punica.Bp.EFCore.Configurations;
using Punica.Bp.EFCore.Middleware;
using Punica.Bp.MultiTenancy;
using Punica.Bp.MultiTenancy.EFCore.Configurations;
using Punica.Bp.MultiTenancy.EFCore.Interceptor;
using Sample.Application.Orders;
using Sample.Infrastructure;
using Sample.Application.Orders.Commands;

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
builder.Services.AddTransient<IEntityTypeConfiguration, DomainConfiguration>();
builder.Services.AddTransient<IMiddlewareProvider, MiddlewareProvider>();

builder.Services.AddAuditing();
builder.Services.AddScoped<IEntityInterceptor, TenantInterceptor>();
builder.Services.AddTransient<IEntityInterceptor, DomainEntityEventInterceptor>();
builder.Services.AddSingleton<IEventTriggerCache, EventTriggerCache>();



builder.Services.AddScoped<ITenantContext, TenantContext>();
builder.Services.AddScoped<IUserContext, UserContext>();

builder.Services.AddRepositories<OrderDbContext>();

builder.Services.AddScoped<IDateTime, BasicDateTime>();

builder.Services.AddScoped<IOrderQueries, OrderQueries2>();

builder.Services.AddHttpContextAccessor();



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
