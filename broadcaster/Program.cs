using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace publisher
{
    class Program
    {
        private static readonly string Key = Environment.GetEnvironmentVariable("EVENT_GRID_KEY");
        private static readonly string Endpoint = Environment.GetEnvironmentVariable("EVENT_GRID_URL");

        static void Main(string[] args)
        {
            if (args.Length != 1) 
            {
                Console.WriteLine("Pass the message to send.");
                return;
            }
            SendEvent(args[0]).Wait();
            Console.WriteLine("Successfully published.");
        }

        private static async Task SendEvent(string msg)
        {
            List<object> events = new List<object>();
            dynamic payload = new JObject();
            payload.Id = Guid.NewGuid().ToString();
            payload.EventType = "BuildMessage";
            payload.Subject = msg;
            payload.EventTime = DateTimeOffset.Now.ToString("o");
            payload.Data = new JObject();
            payload.Data.message = msg;
            events.Add(payload);
            var content = JsonConvert.SerializeObject(events);
            Console.WriteLine(content);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("aeg-sas-key", Key);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var result = await client.PostAsync(Endpoint, httpContent);
            var resultText = await result.Content.ReadAsStringAsync();
            Console.WriteLine($"Response: {result.StatusCode} - {resultText}.");
        }
    }
}
