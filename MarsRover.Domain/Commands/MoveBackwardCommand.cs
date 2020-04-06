using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRover.Domain.Commands
{
    public class MoveBackwardCommand : IRoverCommand
    {
        public const char CommandLetter = 'B';
        public bool IsMovementCommand => true;


        public int BatteryCost { get; }

        public MoveBackwardCommand()
        {
            this.BatteryCost = 3;
        }
        public bool ExecuteCommand(Rover rover)
        {
            return rover.MoveBackward();
        }
    }
}
