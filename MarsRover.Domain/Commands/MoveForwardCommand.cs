using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRover.Domain.Commands
{
    public class MoveForwardCommand : IRoverCommand
    {
        public const char CommandLetter = 'F';
        public bool IsMovementCommand => true;


        public int BatteryCost { get; }

        public MoveForwardCommand()
        {
            this.BatteryCost = 3;
        }
        public bool ExecuteCommand(Rover rover)
        {
            return rover.MoveForward();
        }
    }
}
