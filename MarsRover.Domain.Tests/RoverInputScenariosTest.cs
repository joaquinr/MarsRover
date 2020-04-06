using MarsRover.Contracts;
using MarsRover.Domain.Commands;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Shouldly;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MarsRover.Domain.Tests
{
    [TestClass]
    public class RoverInputScenariosTest
    {
        [DataTestMethod]
        [DataRow("test_run_1.json", "test_sol_1.json")]
        [DataRow("test_run_2.json", "test_sol_2.json")]
        public void When_Running_An_Input_File_We_Get_Expected_Results(string inputFile, string expectedOutputFile)
        {
            var jsonInput = File.ReadAllText(inputFile);
            var jsonExpectedOutput = File.ReadAllText(expectedOutputFile);

            var input = JsonConvert.DeserializeObject<RoverInput>(jsonInput);
            var initialFacing = input.initialPosition.Facing;
            var startPosition = new Position(input.initialPosition.Location.X, input.initialPosition.Location.Y, initialFacing);

            var rover = new Rover(startPosition, input.battery, PlanetMap.FromArray(input.terrain), new CommandTranslator());

            var commands = string.Join("", input.commands);

            rover.RunCommands(commands);

            var outputs = rover.GenerateReport();

            var serializerOptions = new JsonSerializerSettings();
            serializerOptions.Converters.Add(new StringEnumConverter());
            var actualOutputs = JsonConvert.SerializeObject(outputs, serializerOptions);

            

            JObject sourceJObject = JsonConvert.DeserializeObject<JObject>(jsonExpectedOutput);
            JObject targetJObject = JsonConvert.DeserializeObject<JObject>(actualOutputs);
            var sb = new StringBuilder();

            if (!JToken.DeepEquals(sourceJObject, targetJObject))
            {
                foreach (KeyValuePair<string, JToken> sourceProperty in sourceJObject)
                {
                    JProperty targetProp = targetJObject.Property(sourceProperty.Key);

                    if (!JToken.DeepEquals(sourceProperty.Value, targetProp.Value))
                    {
                        sb.AppendLine(string.Format("{0} property value is changed from {1} to {2}", sourceProperty.Key, sourceProperty.Value, targetProp.Value));
                    }
                    
                }
            }
            sb.ToString().ShouldBeEmpty();
        }
    }
}
