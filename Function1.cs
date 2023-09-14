using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

public class Function1
{ 

    [FunctionName("SSE")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
    {
        var response = req.HttpContext.Response;
        response.StatusCode = 200;
        try
        { 
            response.Headers.Add("Content-Type", "text/event-stream");

            for (int i = 0; i < 10; i++)
            {
                var messageBytes = ASCIIEncoding.ASCII.GetBytes($"data: Message {i}\n\n");
                await response.Body.WriteAsync(messageBytes, 0, messageBytes.Length);
                await response.Body.FlushAsync();
                await Task.Delay(1000); // Simulating some delay
            }
            response.Clear();
        }catch(Exception e)
        {
            await Console.Out.WriteLineAsync(e.Message);
        }
        finally
        {

        }
        return new NullResult();
    }

    public class NullResult : IActionResult
    {
        public Task ExecuteResultAsync(ActionContext context) 
          => Task.FromResult(Task.CompletedTask);
    }
}
