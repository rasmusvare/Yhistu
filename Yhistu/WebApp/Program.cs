using System.Globalization;
using System.Text;
using App.BLL;
using App.Contracts.BLL;
using App.Contracts.DAL;
using App.DAL.EF;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("PostgresConnectionCleverCloud");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString);
    options.EnableSensitiveDataLogging();
});

builder.Services.AddScoped<IAppUnitOfWork, AppUOW>();
builder.Services.AddScoped<IAppBLL, AppBLL>();

builder.Services.AddAutoMapper(
    typeof(App.DAL.EF.AutoMapperConfig),
    typeof(App.BLL.AutoMapperConfig),
    typeof(App.Public.DTO.v1.MapperConfig.AutoMapperConfig)
);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddIdentity<AppUser, AppRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredUniqueChars = 0;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 1; // UI validation is still fixed at 6
        }
    )
    .AddDefaultUI()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services
    .AddAuthentication()
    .AddCookie(options => { options.SlidingExpiration = true; })
    .AddJwtBearer(cfg =>
    {
        cfg.RequireHttpsMetadata = false;
        cfg.SaveToken = true;
        cfg.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            ClockSkew = TimeSpan.Zero // Remove delay of token when expire
        };
    });

builder.Services.AddControllersWithViews();
builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsAllowAll",
            policyBuilder =>
            {
                policyBuilder.AllowAnyOrigin();
                policyBuilder.AllowAnyHeader();
                policyBuilder.AllowAnyMethod();
            });
    }
);

var supportedCultures = builder
    .Configuration
    .GetSection("SupportedCultures")
    .GetChildren()
    .Select(x => new CultureInfo(x.Value))
    .ToArray();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    // DateTime and currency support
    options.SupportedCultures = supportedCultures;

    // UI translated strings
    options.SupportedUICultures = supportedCultures;

    // If nothing is found, use this
    options.DefaultRequestCulture =
        new RequestCulture(
            builder.Configuration["DefaultCulture"],
            builder.Configuration["DefaultCulture"]);

    options.SetDefaultCulture(builder.Configuration["DefaultCulture"]);

    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        // Order is important, it's in which order they will be evaluated
        // and support for ?culture=ua-UA
        new QueryStringRequestCultureProvider(),
        new CookieRequestCultureProvider()
    };
});

// Add support for DateOnly Json conversion
builder.Services
    .AddControllers(options => options.UseDateOnlyTimeOnlyStringConverters())
    .AddJsonOptions(options => options.UseDateOnlyTimeOnlyStringConverters());

// API versioning
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    // In case of no explicit version
    options.DefaultApiVersion = new ApiVersion(1, 0);
});

// Swagger support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen();

// ======================================= Pipeline setup and start of web =======================================

var app = builder.Build();

AppDataHelper.SetupAppData(app, app.Environment, app.Configuration);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors("CorsAllowAll");

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint(
            $"/swagger/{description.GroupName}/swagger.json",
            description.GroupName.ToUpperInvariant()
        );
    }

    // serve from root
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseRequestLocalization(app.Services.GetService<IOptions<RequestLocalizationOptions>>()?.Value!);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

// Needed for integration testing. Do not write anything here
public partial class Program
{
}
