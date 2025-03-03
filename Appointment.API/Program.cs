using System;
using Appointment.Core.Interfaces;
using Appointment.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Appointment.Infrastructure.Data;
using Appointment.Infrastructure.Interface;
using Appointment.Core.Services;
var builder = WebApplication.CreateBuilder(args);

// ✅ Configure PostgreSQL with EF Core

builder.Services.AddDbContext<AppointmentDbContext>(option =>
{
    option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Register Dependencies
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();

// Other services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
