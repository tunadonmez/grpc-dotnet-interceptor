using grpc_server.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;

var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var serilog = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();
var builder = WebApplication.CreateBuilder(args);
builder.WebHost
.ConfigureKestrel(options => { options.ListenLocalhost(5000, o => o.Protocols = HttpProtocols.Http2); })   // Setup a HTTP/2 endpoint without TLS.
.ConfigureLogging(logging => { logging.AddSerilog(serilog); })
.ConfigureServices(services => { services.AddHttpContextAccessor(); })
;

// Add services to the container.
builder.Services.AddGrpc();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();