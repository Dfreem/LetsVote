using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using LetsVotes.Client.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.RegisterCommonServices();

await builder.Build().RunAsync();
