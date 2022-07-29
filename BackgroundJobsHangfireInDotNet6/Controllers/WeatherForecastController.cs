using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundJobsHangfireInDotNet6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            #region BackgroundJob.Enqueue
            //Execute method in runtime
            //BackgroundJob.Enqueue(() => SendMessage("Email"));
            #endregion

            #region BackgroundJob.Schedule
            //Execute method after specific time
            //Console.WriteLine(DateTime.Now);
            //BackgroundJob.Schedule(()=>SendMessage("Email"),TimeSpan.FromMinutes(1));   
            #endregion

            #region RecurringJob

            //Execute method every specific time
            RecurringJob.AddOrUpdate(() => SendMessage("Email"), Cron.Minutely);
            #endregion

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [ApiExplorerSettings(IgnoreApi =true)]
        public void SendMessage(string Email)
        {
            Console.WriteLine("Email sent");
            Console.WriteLine(DateTime.Now);
        }
    }
}