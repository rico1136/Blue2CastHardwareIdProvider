using Microsoft.AspNetCore.Mvc;
using HardwareID;

namespace WindowsServiceApiDemo.Controllers
{
	[ApiController]
	[Route("Home")]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private string runningMessage = "WindowsServiceApiDemo is running...";
		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		public string Get() 
		{
			_logger.LogInformation(runningMessage);

			try
			{
				SystemIdentifier systemIdentifier = new SystemIdentifier();

				Console.WriteLine("CPU Identifier: " + systemIdentifier.CPUIdentifier);
				Console.WriteLine("Disk Serial Number: " + systemIdentifier.DiskSerialNumber);
				Console.WriteLine("BIOS Identifier: " + systemIdentifier.BIOSIdentifier);

				string input = systemIdentifier.ToJson();
				return input;
			}
			catch (PlatformNotSupportedException ex)
			{
				Console.WriteLine("Error: " + ex.Message);
				return ex.Message;
			}
		}
	}
}
