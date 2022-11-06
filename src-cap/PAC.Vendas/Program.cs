using FluentScheduler;
using Microsoft.EntityFrameworkCore;
using PAC.Shared.Mensagens;
using PAC.Vendas.Consumidores;
using PAC.Vendas.Data;
using PAC.Vendas.Jobs;
using System.Collections.Concurrent;


var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder);

var app = builder.Build();

ConfigurePipeline(app);

app.Run();


#region método locais

static void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddDbContext<VendasContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("Database"))
    );

    builder.Services.AddSingleton<ConcurrentQueue<IntegracaoMensagem>>();

    builder.Services.AddTransient<VendedoresConsumidor>();

    builder.Services.AddTransient<VendedorEventosIntegracaoJob>();

    // CAP - Event bus
    builder.Services.AddCap(options =>
    {
        options.UseEntityFramework<VendasContext>(options => options.Schema = "VendasCap");

        options.UseRabbitMQ("localhost");

        options.UseDashboard();
    });
}

static void ConfigurePipeline(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    // FluentScheduler - Background job scheduler
    JobManager.Initialize(new ScheduleJobRegistry(app.Services));
}

#endregion