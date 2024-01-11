// Program.cs
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using ShopOnlineBlazerWASM.Client.Services;
using ShopOnlineBlazerWASM.Client;
using Microsoft.AspNetCore.Components.Authorization;
using ShopOnlineBlazerWASM.Client.Authentication;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddBlazoredSessionStorage();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

builder.Services.AddScoped<IProductService, ProductService>(); // Register ProductService here
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>(); // Register ShoppingCartService here
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
