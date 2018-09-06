﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace receiver.Controllers
{
    [Route("api/[controller]")]
    public class BuildController : Controller
    {
        private ILogger _logger;

        public BuildController(ILogger<BuildController> logger)
        {
            _logger = logger;
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]EventGridEvent[] value)
        {
            if (value == null || value.Length != 1 || value[0].Data == null)
            {
                _logger.LogError("Bad event.");
                return BadRequest();
            }

            _logger.LogInformation($"Received event with type {value[0].EventType}");

            if (value[0].EventType == "Microsoft.EventGrid.SubscriptionValidationEvent")
            {
                var subscriptionValidation = value[0].Data.ToObject<SubscriptionValidationEvent>();
                _logger.LogInformation($"Validating subscription with token {subscriptionValidation.ValidationCode}");
                
                return Ok(new SubscriptionValidationResponse
                {
                    ValidationResponse = subscriptionValidation.ValidationCode   
                });
            }

            _logger.LogInformation($"Parsing payload {value[0].Data}...");
            var msg = value[0].Data["message"].ToString();
            _logger.LogInformation($"Received message: {msg}");

            return Ok();
        }
    }
}
