using FluentScheduler;
using MassTransit;
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

    // MassTransit - Event bus
    builder.Services.AddMassTransit(options =>
    {
        options.AddConsumer<FuncionarioAtualizadoConsumidor>(typeof(FuncionarioAtualizadoMensagem));
        options.AddConsumer<FuncionarioDesligadoConsumidor>(typeof(FuncionarioDesligadoMensagem));
        options.AddConsumer<FuncionarioRegistradoConsumidor>(typeof(FuncionarioVendasRegistradoMensagem));

        options.UsingRabbitMq();
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

    // FluentScheduler - Background job scheduler
    JobManager.Initialize(new ScheduleJobRegistry(app.Services));
}

#endregion