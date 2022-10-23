using Hangfire;
using Hangfire.SqlServer;
using Microsoft.EntityFrameworkCore;
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

    // CAP - Event bus
    builder.Services.AddCap(options =>
    {
        options.UseEntityFramework<RhContext>(options => options.Schema = "RhCap");

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

    // Hangfire - Background job scheduler
    app.UseHangfireDashboard(); //app.MapHangfireDashboard();

    RecurringJob.AddOrUpdate(
        "FuncionarioEventosIntegracao",
        () => new FuncionarioEventosIntegracaoJob(app.Services).Executar(),
        "0/15 * * ? * *" // A cada 15 segundos
    );
}

#endregion