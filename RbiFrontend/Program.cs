using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RbiFrontend;
using RbiFrontend.ApiAccess;
using RbiFrontend.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//Scoped and Singleton is the same in Wasm

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7132/api/") });

builder.Services.AddScoped<RecipeSource>();
builder.Services.AddScoped<TagSource>();

builder.Services.AddScoped<UserService>();
builder.Services.AddSingleton<UnitService>();

await builder.Build().RunAsync();

