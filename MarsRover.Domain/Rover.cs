using MarsRover.Contracts;
using MarsRover.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarsRover.Domain
{
    /// <summary>
    /// Main domain object representing the robot that will move around the planet
    /// </summary>
    public class Rover
    {
        private readonly ICommandTranslator commandTranslator;

        public Position CurrentPosition { get; private set; }
        private PlanetMap Map { get; }
        public int BatteryCharge { get; private set; }
        public List<MapCellResource> CollectedSamples { get; }
        public List<Location> CellsVisited { get; }
        private int RanOutOfBattery { get; set; }
        private List<string> BackoffStrategies { get; set; }
        private int BackoffStrategiesFailed { get; set; }

        public Rover(Position startingPosition, int startingBatteryCharge, PlanetMap map, ICommandTranslator commandTranslator)
        {
            this.CurrentPosition = startingPosition;
            this.BatteryCharge = startingBatteryCharge;
            this.Map = map;
            this.commandTranslator = commandTranslator;
            this.CollectedSamples = new List<MapCellResource>();
            this.CellsVisited = new List<Location>()
            {
                startingPosition.Coordinate
            };

            this.BackoffStrategies = new List<string>()
            {
                "ERF",
                "ELF",
                "ELLF",
                "EBRF",
                "EBBLF",
                "EFF",
                "EFLFLF",
            };
        }
        public void RunCommands(string commandString)
        {
            var commands = this.commandTranslator.TranslateCommands(commandString);
            ExecuteCommands(commands, triggerBackoff: true);
        }


        public bool MoveForward()
        {
            var newPosition = this.CurrentPosition.MoveForward();
            return UpdateOnValidPosition(newPosition);
        }
        public bool MoveBackward()
        {
            var newPosition = this.CurrentPosition.MoveBackward();
            return UpdateOnValidPosition(newPosition);
        }
        private bool UpdateOnValidPosition(Position newPosition)
        {
            if (this.Map.CanRobotMoveToCoordinates(newPosition.Coordinate))
            {
                this.CurrentPosition = newPosition;
                return true;
            }

            return false;
        }
        public void TurnLeft()
        {
            var newPosition = this.CurrentPosition.TurnLeft();
            this.CurrentPosition = newPosition;
        }
        public void TurnRight()
        {
            var newPosition = this.CurrentPosition.TurnRight();
            this.CurrentPosition = newPosition;
        }
        public void TakeSample()
        {
            var sample = this.Map[this.CurrentPosition.Coordinate.X, this.CurrentPosition.Coordinate.Y];
            this.CollectedSamples.Add(sample);
        }
        public void ExtendSolarPanels()
        {
            this.BatteryCharge += 10;
        }
        private void RunBackOffStrategies()
        {
            bool isMovementSuccessful = true;
            foreach (var strategy in this.BackoffStrategies)
            {
                var commands = this.commandTranslator.TranslateCommands(strategy);
                isMovementSuccessful = ExecuteCommands(commands, triggerBackoff: false);
                if (isMovementSuccessful) break;
            }

            if (!isMovementSuccessful) this.BackoffStrategiesFailed += 1;
        }
        private bool ExecuteCommands(List<IRoverCommand> commands, bool triggerBackoff)
        {
            foreach (var command in commands)
            {
                if (command.BatteryCost < this.BatteryCharge)
                {
                    var commandExecuted = command.ExecuteCommand(this);
                    if (commandExecuted) this.BatteryCharge -= command.BatteryCost;
                    if (commandExecuted && command.IsMovementCommand) this.CellsVisited.Add(this.CurrentPosition.Coordinate);
                    if (!commandExecuted && command.IsMovementCommand)
                    {
                        if (triggerBackoff) this.RunBackOffStrategies();
                        if(!triggerBackoff) return false;
                    }
                }
                else
                {
                    this.RanOutOfBattery += 1;
                }
            }
            return true;
        }

        public RoverReport GenerateReport()
        {
            var ret = new RoverReport()
            {
                Battery = this.BatteryCharge,
                FinalPosition = this.CurrentPosition.ToDto(),
                SamplesCollected = this.CollectedSamples.Select(sample => MapResourceToString(sample)).ToList(),
                VisitedCells = this.CellsVisited,
            };

            return ret;
        }
        private string MapResourceToString(MapCellResource resource)
        {
            switch (resource)
            {
                case MapCellResource.Ferrum:
                    return "Fe";
                case MapCellResource.Selenium:
                    return "Se";
                case MapCellResource.Water:
                    return "W";
                case MapCellResource.Silicon:
                    return "Si";
                case MapCellResource.Zinc:
                    return "Zn";
                case MapCellResource.Obstacle:
                    return "Obs";
                default:
                    return "Obs";
            }
        }
    }
}
