using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRover.Domain.Commands
{
    public class ExtendSolarPanelsCommand : IRoverCommand
    {
        public const char CommandLetter = 'E';
        public int BatteryCost { get; set; }

        public bool IsMovementCommand => false;

        public ExtendSolarPanelsCommand()
        {
            this.BatteryCost = 1;
        }

        public bool ExecuteCommand(Rover rover)
        {
            rover.ExtendSolarPanels();
            return true;
        }
    }
}
