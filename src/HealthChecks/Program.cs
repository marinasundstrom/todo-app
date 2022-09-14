﻿var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddHealthChecksUI()
    .AddSqliteStorage("Data Source=healthchecks.db;");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.MapHealthChecksUI();

app.Run();

