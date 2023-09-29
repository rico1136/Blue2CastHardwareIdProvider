using App.WindowsService;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting.Systemd;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using System.Runtime.InteropServices;

//HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWindowsService(options =>
{
	options.ServiceName = ".NET Joke Service";
});

if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
	LoggerProviderOptions.RegisterProviderOptions<
	EventLogSettings, EventLogLoggerProvider>(builder.Services);

	builder.Services.AddHostedService<WindowsBackgroundService>();
}
else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
{
	builder.Host.UseSystemd();
	builder.Services.AddHostedService<LinuxBackgroundService>();
}
else
{
	throw new PlatformNotSupportedException("Unsupported OS.");
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var host = builder.Build();

host.UseHttpsRedirection();

host.UseAuthorization();

host.MapControllers();

host.Run("http://localhost:5000");