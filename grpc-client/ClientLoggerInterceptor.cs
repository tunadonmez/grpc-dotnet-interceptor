using Grpc.Core.Interceptors;
using Grpc.Core;

using Serilog;
using Serilog.Sinks.Elasticsearch;

public class ClientLoggerInterceptor:Grpc.Core.Interceptors.Interceptor
{
  /*
  public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
    TRequest request,
    ClientInterceptorContext<TRequest, TResponse> context,
    AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
  {
    LogCall(context.Method);

    return continuation(request, context);
  }

  private void LogCall<TRequest, TResponse>(Method<TRequest, TResponse> method)
    where TRequest : class
    where TResponse : class
  {
    Log.Logger = new LoggerConfiguration()
                        .Enrich.FromLogContext()                        
                        .WriteTo.Debug()
                        //.WriteTo.Console()
                        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200") ){
             AutoRegisterTemplate = true,
             AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
             IndexFormat = $"grpc-client"
     })
                        .Enrich.WithMachineName()
                        .CreateLogger();
    Log.Logger.Information($"Starting call. Type: {method.Type}. Request: {typeof(TRequest)}. Response: {typeof(TResponse)}");

  }
  

 */

}
