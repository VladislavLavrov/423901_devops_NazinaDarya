using OpenTelemetry.Metrics;
using OpenTelemetry;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// === ƒќЅј¬Ћя≈ћ OpenTelemetry дл€ метрик ===
builder.Services.AddOpenTelemetry()
    .WithMetrics(meterProviderBuilder =>
    {
        // 1. Ёкспортер дл€ Prometheus
        meterProviderBuilder.AddPrometheusExporter();

        // 2. ћетрики дл€ ASP.NET Core компонентов
        meterProviderBuilder.AddMeter(
            "Microsoft.AspNetCore.Hosting",
            "Microsoft.AspNetCore.Server.Kestrel",
            "Microsoft.AspNetCore.Http.Connections"
        );

        // 3. Ќастройка гистограммы дл€ длительности запросов
        meterProviderBuilder.AddView("http.server.request.duration",
            new ExplicitBucketHistogramConfiguration
            {
                Boundaries = new[]
                {
                    0,      // < 0 секунд
                    0.005,  // < 5 миллисекунд
                    0.01,   // < 10 миллисекунд
                    0.025,  // < 25 миллисекунд
                    0.05,   // < 50 миллисекунд
                    0.075,  // < 75 миллисекунд
                    0.1,    // < 100 миллисекунд
                    0.25,   // < 250 миллисекунд
                    0.5,    // < 500 миллисекунд
                    0.75,   // < 750 миллисекунд
                    1,      // < 1 секунды
                    2.5,    // < 2.5 секунд
                    5,      // < 5 секунд
                    7.5,    // < 7.5 секунд
                    10      // < 10 секунд
                }
            });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// === ƒќЅј¬Ћя≈ћ endpoint дл€ сбора метрик Prometheus ===
app.MapPrometheusScrapingEndpoint();

app.Run();