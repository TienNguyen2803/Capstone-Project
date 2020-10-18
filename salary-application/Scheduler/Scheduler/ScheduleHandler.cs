using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Scheduler
{
    public static class ScheduleHandler
    {
        public static HttpClient client;

        [FunctionName("Handler")]
        public static async Task Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger log)
        {
            try
            {
                ///api/Schedule/check-payroll-sch
                client = new HttpClient();
                var uri = new Uri("http://localhost:3911/api/Schedule/check-payroll-sch");
                //var uri = new Uri("http://http://spcs.azurewebsites.net/api/Schedule/check-payroll-sch");
                client.BaseAddress = uri;
                await client.GetAsync(uri);

                ///api/Schedule/send-emai-sch;
                client = new HttpClient();
                uri = new Uri("http://localhost:3911/api/Schedule/send-email-sch");
                //var uri = new Uri("http://http://spcs.azurewebsites.net/api/Schedule/send-email-sch");
                client.BaseAddress = uri;
                await client.GetAsync(uri);

                ///api/Schedule/update-payroll-sch
                client = new HttpClient();
                uri = new Uri("http://localhost:3911/api/Schedule/update-payroll-sch");
                //var uri = new Uri("http://http://spcs.azurewebsites.net/api/Schedule/update-payroll-sch");
                client.BaseAddress = uri;
                await client.GetAsync(uri);

                ///api/Schedule/check-payroll-sch
                client = new HttpClient();
                uri = new Uri("http://localhost:3911/api/Schedule/active-doc-sch");
                //var uri = new Uri("http://http://spcs.azurewebsites.net/Schedule/active-doc-sch");
                client.BaseAddress = uri;
                await client.GetAsync(uri);

            }
            catch (Exception e)
            {
                log.LogInformation($"Error: {e}");
                throw e;
            }
        }
    }
}
