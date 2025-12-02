using ecom.DBcontexts;
using OncallActingDriver._Services;
using Pluso.Api.Repositorys;
using Pluso.Api.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy
            .AllowAnyOrigin()   // for development only
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDbContext, DbContext>();
builder.Services.AddScoped<ITenantRepository, TenantRepository>();
builder.Services.AddScoped<ITenantService, TenantService>();


builder.Services.AddScoped<ITenantSubscriptionRepository, TenantSubscriptionRepository>();
builder.Services.AddScoped<ITenantSubscriptionService, TenantSubscriptionService>();

builder.Services.AddHttpClient();
builder.Services.AddScoped<IRazorpay_VerificationRepository, Razorpay_VerificationRepository>();
builder.Services.AddScoped<RazorpayService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
