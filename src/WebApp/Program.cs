using Serilog;
using Serilog.Events;

var _logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft",LogEventLevel.Debug)
    .WriteTo.Seq("http://10.43.11.19:5341",
        Serilog.Events.LogEventLevel.Verbose)
    .MinimumLevel.Debug()
    .CreateLogger();



    _logger.Information("Enter WebApp Builder ");

var builder = WebApplication.CreateBuilder(args);


    _logger.Information("After WebApp Builder ");

builder.AddServiceDefaults();

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.AddApplicationServices();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAntiforgery();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.MapForwarder("/product-images/{id}", "http://catalog-api", "/api/v1/catalog/items/{id}/pic");

app.Run();
