using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRover.Domain.Commands
{
    public class TurnLeftCommand : IRoverCommand
    {
        public const char CommandLetter = 'L';
        public bool IsMovementCommand => false;


        public int BatteryCost { get; }
        public TurnLeftCommand()
        {
            this.BatteryCost = 2;
        }

        public bool ExecuteCommand(Rover rover)
        {
            rover.TurnLeft();
            return true;
        }
    }
}
