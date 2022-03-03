using System.Threading.Tasks;
using Grpc.Net.Client;
using grpc_client;
using Grpc.Core;
using Serilog;
using Serilog.Context;
using Serilog.Extensions.Logging;
using Microsoft.Extensions.Configuration;

AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);   //  HTTP/2 without SSL
var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var serilog = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();

var correlationId = Guid.NewGuid().ToString();
using(LogContext.PushProperty("CorrelationId",correlationId))
{
    using var channel = GrpcChannel.ForAddress("http://localhost:5000",
        new GrpcChannelOptions { LoggerFactory =  new SerilogLoggerFactory(serilog, true, null) });

    var client = new Greeter.GreeterClient(channel);
    var request =new HelloRequest { Name = "KIBANA" };
    serilog.Information($"Request: {request.ToString()}" );
    var reply = await client.SayHelloAsync(request,
        new Metadata() { new Metadata.Entry("Correlation-Id",correlationId)  });
        
    serilog.Information($"Response: {reply.Message}");
}

Console.ReadKey();