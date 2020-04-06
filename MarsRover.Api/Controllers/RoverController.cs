using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarsRover.Contracts;
using MarsRover.Domain;
using MarsRover.Domain.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MarsRover.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoverController : ControllerBase
    {


        private readonly ILogger<RoverController> _logger;

        public RoverController(ILogger<RoverController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public ActionResult<RoverReport> SimulateRun(RoverInput input)
        {
            var rover = new Rover(Position.FromDto(input.initialPosition), input.battery, PlanetMap.FromArray(input.terrain), new CommandTranslator());
            rover.RunCommands(string.Join("", input.commands));
            
            return Ok(rover.GenerateReport());
        }
    }
}
