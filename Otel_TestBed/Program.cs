using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Instrumentation.Digma;
using OpenTelemetry.Instrumentation.Digma.Helpers;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Otel_TestBed;
using Otel_TestBed.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddOpenTelemetry().WithTracing(providerBuilder =>
{
    providerBuilder.ConfigureResource(resourceBuilder =>
        {
            resourceBuilder.AddEnvironmentVariableDetector()
                .AddService(serviceName: "otel-test", serviceVersion: "0.01a")
                .AddTelemetrySdk();
        }).AddSource("*")
        .AddHttpClientInstrumentation(options =>
        {
            options.RecordException = true;
        })
        .AddAspNetCoreInstrumentation(options =>
        {
            options.RecordException = true;
        })
        .AddConsoleExporter()
        .AddOtlpExporter(x =>
        {
            x.Protocol = OtlpExportProtocol.HttpProtobuf;
            x.ExportProcessorType = ExportProcessorType.Simple;
        });
});

builder.Services.AddTransient<ISampleService, SampleService>().Decorate<ISampleService>(x => TraceDecorator<ISampleService>.Create(x,decorateAllMethods:true));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
