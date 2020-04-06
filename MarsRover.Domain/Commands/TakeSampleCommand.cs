using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRover.Domain.Commands
{
    public class TakeSampleCommand : IRoverCommand
    {
        public const char CommandLetter = 'S';
        public bool IsMovementCommand => false;

        public int BatteryCost { get; }

        public TakeSampleCommand()
        {
            this.BatteryCost = 8;
        }

        public bool ExecuteCommand(Rover rover)
        {
            rover.TakeSample();
            return true;
        }
    }
}
