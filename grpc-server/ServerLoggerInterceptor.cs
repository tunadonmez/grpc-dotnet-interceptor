using Grpc.Core.Interceptors;
using Grpc.Core;

using Serilog;
using Serilog.Sinks.Elasticsearch;

public class ServerLoggerInterceptor: Interceptor
{
  public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
    TRequest request,
    ServerCallContext context,
    UnaryServerMethod<TRequest, TResponse> continuation)
  {
    LogCall<TRequest, TResponse>(MethodType.Unary, context);
    return continuation(request, context);
  }

  private void LogCall<TRequest, TResponse>(MethodType methodType, ServerCallContext context)
    where TRequest : class
    where TResponse : class
  {
    var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build(); 
    Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(config)
                        .Enrich.FromLogContext()                        
                        .WriteTo.Debug()
                        .WriteTo.Console()
                        .WriteTo.Elasticsearch(ConfigureElasticSink(config))
                        .Enrich.WithMachineName()
                        .CreateLogger();
    Log.Logger.Information($"Starting call. Type: {methodType}. Request: {typeof(TRequest)}. Response: {typeof(TResponse)}");
  }

ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration)
{
	return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
	{
		AutoRegisterTemplate = true,
        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
		IndexFormat = $"grpc-server"
	};
}
}