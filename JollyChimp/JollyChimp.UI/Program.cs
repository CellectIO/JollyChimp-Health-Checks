using JollyChimp.Core.Common.Encryption;
using JollyChimp.Core.Common.Models.Configs.Startup;
using JollyChimp.Core.Data.Builders;
using JollyChimp.HealthChecks.Startup;

var builder = WebApplication.CreateBuilder(args);

//-------------------
//CONFIGURE CONFIGS
//-------------------

var serverConfig = builder.Configuration.GetSection("JollyChimp").Get<ServerStartupConfig>()!;

EncryptionExtension.SetEncryptionKey(serverConfig.EncryptionKey);

HealthChecksStartupConfig jollyChimpSettings;
using (var configBuilder = JollyChimpStartupConfigBuilder.Init(serverConfig))
{
    jollyChimpSettings = configBuilder
        .ClearDeleteQueue()
        .SetUiSettings()
        .SetEndPoints()
        .SetWebHooks()
        .SetHealthChecks()
        .Return();
}

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.RegisterJollyChimp(jollyChimpSettings);

var app = builder.Build();

//-------------------
//CONFIGURE MIDDLEWARE
//-------------------

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

//-------------------
//MAP ENDPOINTS
//-------------------

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapJollyChimp(jollyChimpSettings);

//-------------------
//GO APE SHIT
//-------------------

app.Run();