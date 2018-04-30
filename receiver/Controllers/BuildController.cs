using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Newtonsoft.Json;

namespace receiver.Controllers
{
    [Route("api/[controller]")]
    public class BuildController : Controller
    {
        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]EventGridEvent[] value)
        {
            if (value == null || value.Length != 1 || value[0].Data == null)
            {
                Console.WriteLine("Bad event.");
                return BadRequest();
            }

            Console.WriteLine($"Received event with type {value[0].EventType}");

            if (value[0].EventType == "Microsoft.EventGrid.SubscriptionValidationEvent")
            {
                var subscriptionValidation = value[0].Data.ToObject<SubscriptionValidationEvent>();
                Console.WriteLine($"Validating subscription with token {subscriptionValidation.ValidationCode}");
                
                return Ok(new SubscriptionValidationResponse
                {
                    ValidationResponse = subscriptionValidation.ValidationCode   
                });
            }

            Console.WriteLine($"Parsing payload {value[0].Data}...");
            var msg = value[0].Data["message"].ToString();
            Console.WriteLine($"Received message: {msg}");

            return Ok();
        }
    }
}
