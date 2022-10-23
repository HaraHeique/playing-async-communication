using Microsoft.EntityFrameworkCore;
using PAC.Producao.Consumidores;
using PAC.Producao.Data;
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