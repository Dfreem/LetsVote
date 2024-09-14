using LetsVote.Client.Pages;
using LetsVote.Components;
using LetsVote.Client.Extensions;
using LetsVote.Hubs;
using Microsoft.AspNetCore.Builder;
using LetsVote.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<HostOptions>(config =>
{
    config.ServicesStartConcurrently = true;
    config.ServicesStopConcurrently = true;
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddSignalR(config =>
{
    config.EnableDetailedErrors = true;
    config.MaximumParallelInvocationsPerClient = 2;
});
builder.Services.RegisterCommonServices();
builder.Services.AddHostedService<TugGameService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(LetsVote.Client._Imports).Assembly);
app.MapHub<TugHub>("/tug-game");

app.Run();
