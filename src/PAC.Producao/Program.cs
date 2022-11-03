using Microsoft.EntityFrameworkCore;
using PAC.Producao.Configurations;
using PAC.Producao.Consumidores;
using PAC.Producao.Data;
using PAC.Producao.Jobs;
using PAC.Shared.Mensagens;
using Quartz;
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

    builder.Services.AddDbContext<ProducaoContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("Database"))
    );

    builder.Services.AddSingleton<ConcurrentQueue<IntegracaoMensagem>>();

    builder.Services.AddTransient<OperariosConsumidor>();

    // CAP - Event bus
    builder.Services.AddCap(options =>
    {
        options.UseEntityFramework<ProducaoContext>(options => options.Schema = "ProducaoCap");

        options.UseRabbitMQ("localhost");

        options.UseDashboard();
    });

    // Quartz.NET - Background job scheduler
    builder.Services.AddQuartz(config =>
    {
        //config.UseMicrosoftDependencyInjectionScopedJobFactory(); -> Obsoleto
        config.UseMicrosoftDependencyInjectionJobFactory();

        config.AddJobConfig<OperarioEventosIntegracaoJob>(builder.Configuration);
    });

    // Integração do Quartz.NET com Hosted Services do .NET
    builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
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
}

#endregion