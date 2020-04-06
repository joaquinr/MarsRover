using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRover.Domain.Commands
{
    public class TurnRightCommand : IRoverCommand
    {
        public const char CommandLetter = 'R';
        public bool IsMovementCommand => false;

        public int BatteryCost { get; }
        public TurnRightCommand()
        {
            this.BatteryCost = 2;
        }

        public bool ExecuteCommand(Rover rover)
        {
            rover.TurnRight();
            return true;
        }
    }
}
