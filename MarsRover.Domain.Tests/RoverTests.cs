using MarsRover.Contracts;
using MarsRover.Domain.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MarsRover.Domain.Tests
{
    [TestClass]
    public class RoverTests
    {
        [TestMethod]
        public void When_Rover_Lands_It_Is_In_Initial_Position_With_Initial_Charge_And_Position_Has_Been_Visited()
        {
            var startingPosition = new Position(0, 0, Direction.East);
            var startingBattery = 10;
            var testMap = new PlanetMap(new Dictionary<Location, MapCellResource>()
            {
                { new Location(0, 0), MapCellResource.Ferrum }
            });
            ICommandTranslator commandTranslator = null;
            var rover = new Rover(startingPosition, startingBattery, testMap, commandTranslator);

            rover.CurrentPosition.ShouldBe(startingPosition);
            rover.CellsVisited.Count.ShouldBe(1);
            rover.CellsVisited.First().ShouldBe(startingPosition.Coordinate);
            rover.BatteryCharge.ShouldBe(startingBattery);
        }
        [DataTestMethod]
        [DataRow(Direction.East, 2, 1, "F", 3)]
        [DataRow(Direction.North, 1, 0, "F", 3)]
        [DataRow(Direction.West, 0, 1, "F", 3)]
        [DataRow(Direction.South, 1, 2, "F", 3)]
        [DataRow(Direction.East, 0, 1, "B", 3)]
        [DataRow(Direction.North, 1, 2, "B", 3)]
        [DataRow(Direction.West, 2, 1, "B", 3)]
        [DataRow(Direction.South, 1, 0, "B", 3)]
        public void When_Rover_Moves_Forward_Facing_Direction_New_Position_Is_Expected_And_Battery_Is_Consumed(Direction direction, int expectedX, int expectedY, string command, int batteryExpense)
        {
            var expectedCoordinate = new Location(expectedX, expectedY);
            var startingPosition = new Position(1, 1, direction);
            var startingBattery = 10;
            Dictionary<Location, MapCellResource> coordinates = CreateMap(3, 3);
            var testMap = new PlanetMap(coordinates);
            ICommandTranslator commandTranslator = new CommandTranslator();
            var rover = new Rover(startingPosition, startingBattery, testMap, commandTranslator);

            rover.RunCommands(command);
            rover.CurrentPosition.Coordinate.ShouldBe(expectedCoordinate);
            rover.BatteryCharge.ShouldBe(startingBattery - batteryExpense);
        }

        [DataTestMethod]
        [DataRow(Direction.East, Direction.South, "R", 2)]
        [DataRow(Direction.West, Direction.North, "R", 2)]
        [DataRow(Direction.North, Direction.East, "R", 2)]
        [DataRow(Direction.South, Direction.West, "R", 2)]
        [DataRow(Direction.East, Direction.North, "L", 2)]
        [DataRow(Direction.West, Direction.South, "L", 2)]
        [DataRow(Direction.North, Direction.West, "L", 2)]
        [DataRow(Direction.South, Direction.East, "L", 2)]
        public void When_Rover_Turns_Facing_Direction_New_Direction_Is_Expected_And_Battery_Is_Consumed(Direction startDirection, Direction expectedDirection, string command, int batteryExpense)
        {
            var startingPosition = new Position(1, 1, startDirection);
            var startingBattery = 10;
            var coordinates = CreateMap(1, 1);
            var testMap = new PlanetMap(coordinates);
            ICommandTranslator commandTranslator = new CommandTranslator();
            var rover = new Rover(startingPosition, startingBattery, testMap, commandTranslator);

            rover.RunCommands(command);
            rover.CurrentPosition.Facing.ShouldBe(expectedDirection);
            rover.BatteryCharge.ShouldBe(startingBattery - batteryExpense);
        }

        [TestMethod]
        public void When_Rover_Collects_Sample_Sample_Is_Collected_And_Battery_Is_Consumed()
        {
            var startingPosition = new Position(0, 0, Direction.East);
            var startingBattery = 10;
            var coordinates = CreateMap(1, 1);
            var testMap = new PlanetMap(coordinates);
            ICommandTranslator commandTranslator = new CommandTranslator();
            var rover = new Rover(startingPosition, startingBattery, testMap, commandTranslator);

            rover.RunCommands("S");
            rover.CollectedSamples.Count.ShouldBe(1);
            rover.CollectedSamples[0].ShouldBe(MapCellResource.Ferrum);
            rover.BatteryCharge.ShouldBe(startingBattery - 8);
        }
        [TestMethod]
        public void When_Rover_Extends_Solar_Panel_Battery_Is_Charged_And_Battery_Is_Consumed()
        {
            var startingPosition = new Position(1, 1, Direction.East);
            var startingBattery = 10;
            var coordinates = CreateMap(1, 1);
            var testMap = new PlanetMap(coordinates);
            ICommandTranslator commandTranslator = new CommandTranslator();
            var rover = new Rover(startingPosition, startingBattery, testMap, commandTranslator);

            rover.RunCommands("E");
            rover.BatteryCharge.ShouldBe(startingBattery - 1 + 10);
        }
        [TestMethod]
        public void When_Rover_Moves_Towards_Obstacle_A_Backoff_Strategy_Is_Played_Instead()
        {
            //First backoff is ERF
            var startingPosition = new Position(5, 5, Direction.East);
            var startingBattery = 10;
            var coordinates = CreateMap(10, 10);
            coordinates.Remove(new Location(6, 5));
            coordinates.Add(new Location(6, 5), MapCellResource.Obstacle);

            var testMap = new PlanetMap(coordinates);
            ICommandTranslator commandTranslator = new CommandTranslator();
            var rover = new Rover(startingPosition, startingBattery, testMap, commandTranslator);

            rover.RunCommands("F");
            rover.BatteryCharge.ShouldBe(startingBattery - 1 + 10 - 2 - 3);
            rover.CurrentPosition.Facing.ShouldBe(Direction.South);
            rover.CurrentPosition.Coordinate.ShouldBe(new Location(5, 6));
        }
        [TestMethod]
        public void When_Rover_Plays_A_Backoff_Strategy_And_Finds_Another_Obstacle_Strategy_Is_Interrupted_And_Next_Strategy_Plays()
        {
            //First backoff is ERF
            //Second backoff is ELF
            //Third backoff is ELLF
            var startingPosition = new Position(5, 5, Direction.East);
            var startingBattery = 10;
            var coordinates = CreateMap(10, 10);
            coordinates.Remove(new Location(6, 5));
            coordinates.Add(new Location(6, 5), MapCellResource.Obstacle);
            coordinates.Remove(new Location(5, 6));
            coordinates.Add(new Location(5, 6), MapCellResource.Obstacle);

            var testMap = new PlanetMap(coordinates);
            ICommandTranslator commandTranslator = new CommandTranslator();
            var rover = new Rover(startingPosition, startingBattery, testMap, commandTranslator);

            rover.RunCommands("F");
            rover.BatteryCharge.ShouldBe(startingBattery - 1 + 10 - 2 - 1 + 10 - 2 - 1 + 10 - 2 - 2 - 3);
            rover.CurrentPosition.Facing.ShouldBe(Direction.West);
            rover.CurrentPosition.Coordinate.ShouldBe(new Location(4, 5));
        }

        private static Dictionary<Location, MapCellResource> CreateMap(int width, int height)
        {
            var coordinates = new Dictionary<Location, MapCellResource>();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    coordinates.Add(new Location(x, y), MapCellResource.Ferrum);
                }
            }

            return coordinates;
        }
    }
}
