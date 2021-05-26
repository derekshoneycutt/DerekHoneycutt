using DerekHoneycutt.Data.Extensions;
using Microsoft.Azure.Functions.Worker.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using System.Threading.Tasks;

namespace DerekHoneycutt.Api
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices((context, services) =>
                {
                    var jsonOptions = new JsonSerializerOptions()
                    {
                        IgnoreNullValues = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };
                    jsonOptions.Converters.Add(new RestModels.PageJsonConverter());
                    services.AddSingleton(jsonOptions);

                    services.AddDerekHoneycuttServices(context.Configuration);
                })
                .Build();

            host.Run();
        }
    }
}