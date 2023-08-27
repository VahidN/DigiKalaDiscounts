using DigiKalaDiscounts.Services;
using DigiKalaDiscounts.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

SetCurrentDirectory();

var builder = Host.CreateDefaultBuilder(args)
                  .ConfigureServices((context, services) => services.AddServices(context.Configuration));
var app = builder.Build();
await app.Services.GetRequiredService<IAppRunnerService>().StartAsync(args);

static void SetCurrentDirectory()
{
    var dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    if (dirName is null)
    {
        throw new InvalidOperationException("dirName is null");
    }

    Directory.SetCurrentDirectory(dirName);
}
