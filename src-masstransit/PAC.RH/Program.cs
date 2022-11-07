using Hangfire;
using Hangfire.SqlServer;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using PAC.RH.Consumidores;
using PAC.RH.Data;
using PAC.RH.Jobs;
using PAC.Shared.Mensagens;
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

    builder.Services.AddDbContext<RhContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("Database"))
    );

    builder.Services.AddSingleton<ConcurrentQueue<IntegracaoMensagem>>();

    builder.Services.AddTransient<FuncionariosConsumidor>();

    builder.Services.AddTransient<FuncionarioEventosIntegracaoJob>();

    // Hangfire - Background job scheduler
    builder.Services.AddHangfire(
        configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(builder.Configuration.GetConnectionString("Database"), new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true,
                SchemaName = "RhHangFire"
            })
    );

    builder.Services.AddHangfireServer();

    // MassTransit - Event bus
    builder.Services.AddMassTransit(config =>
    {
        config.AddConsumer<OperarioNomeAlteradoConsumidor>();
        config.AddConsumer<VendedorInfoPessoaisAlteradaConsumidor>();

        config.UsingRabbitMq((context, cfg) =>
        {
            var eventBusSection = builder.Configuration.GetSection("EventBus:Topics");

            cfg.ReceiveEndpoint(eventBusSection["OperarioNomeAlterado"], e => e.ConfigureConsumer<OperarioNomeAlteradoConsumidor>(context));
            cfg.ReceiveEndpoint(eventBusSection["VendedorInfoPessoaisAlterado"], e => e.ConfigureConsumer<VendedorInfoPessoaisAlteradaConsumidor>(context));
        });
    });

    builder.Services.AddOptions<MassTransitHostOptions>()
        .Configure(options =>
        {
            options.WaitUntilStarted = true;

            // if specified, limits the wait time when starting the bus
            options.StartTimeout = TimeSpan.FromSeconds(10);

            // if specified, limits the wait time when stopping the bus
            options.StopTimeout = TimeSpan.FromSeconds(30);
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

    // Hangfire - Background job scheduler
    app.UseHangfireDashboard(); //app.MapHangfireDashboard();
    
    //app.UseHangfireServer(); -> Executar os jobs em background usando múltiplas threads
    
    RecurringJob.AddOrUpdate<FuncionarioEventosIntegracaoJob>(
        job => job.Executar(),
        "0/15 * * ? * *" // A cada 15 segundos
    );
}

#endregion