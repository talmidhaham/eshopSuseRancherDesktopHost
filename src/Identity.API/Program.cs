
using Serilog;
using Serilog.Events;

var _logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft",LogEventLevel.Debug)
    .WriteTo.Seq("http://10.43.11.19:5341",
        Serilog.Events.LogEventLevel.Verbose)
    .MinimumLevel.Debug()
    .CreateLogger();


    _logger.Information("Enter Identity Api Builder");

    _logger.Information("Enter Identity Api Builder 3");

    var builder = WebApplication.CreateBuilder(args);

    _logger.Information("Before AddServiceDefaults");
    builder.AddServiceDefaults();

        _logger.Information("After AddServiceDefaults");

    builder.Services.AddControllersWithViews();

     _logger.Information("Before AddNpgsqlDbContext<ApplicationDbContext>(IdentityDB);");
    builder.AddNpgsqlDbContext<ApplicationDbContext>("IdentityDB");

    //builder.AddSqlServerDbContext<ApplicationDbContext>("IdentityDB");

    _logger.Information(builder.Configuration.GetConnectionString("IdentityDB"));

    // Apply database migration automatically. Note that this approach is not
    // recommended for production scenarios. Consider generating SQL scripts from
    // migrations instead.
  

    builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

    builder.Services.AddMigration<ApplicationDbContext, UsersSeed>();


    _logger.Information("AddIdentity Added");

    builder.Services.AddIdentityServer(options =>
    {
        options.IssuerUri = "null";
        options.Authentication.CookieLifetime = TimeSpan.FromHours(2);

        options.Events.RaiseErrorEvents = true;
        options.Events.RaiseInformationEvents = true;
        options.Events.RaiseFailureEvents = true;
        options.Events.RaiseSuccessEvents = true;
        //options.KeyManagement.KeyPath = "/lolo/keys";
    })
    .AddInMemoryIdentityResources(Config.GetResources())
    .AddInMemoryApiScopes(Config.GetApiScopes())
    .AddInMemoryApiResources(Config.GetApis())
    .AddInMemoryClients(Config.GetClients(builder.Configuration))
    .AddAspNetIdentity<ApplicationUser>()
    .AddDeveloperSigningCredential(true,"/lolo/tempkey.jwk"); // Not recommended for production - you need to store your key material somewhere secure


        _logger.Warning("AddIdentityServer Added");

    builder.Services.AddTransient<IProfileService, ProfileService>();
    builder.Services.AddTransient<ILoginService<ApplicationUser>, EFLoginService>();
    builder.Services.AddTransient<IRedirectService, RedirectService>();

    builder.Logging.AddSerilog(_logger);
    var app = builder.Build();

    app.MapDefaultEndpoints();

    app.UseStaticFiles();

    // This cookie policy fixes login issues with Chrome 80+ using HTTP
    app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });
    app.UseRouting();
    app.UseIdentityServer();
    app.UseAuthorization();

    app.MapDefaultControllerRoute();

    app.Run();



