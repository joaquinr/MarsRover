using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRover.Domain.Commands
{
    /// <summary>
    /// Defines operations that the Rover can handle
    /// </summary>
    public interface IRoverCommand
    {
        /// <summary>
        /// Flag to determine if this is a command that changes location
        /// </summary>
        bool IsMovementCommand { get; }
        /// <summary>
        /// Cost of performing the command
        /// </summary>
        int BatteryCost { get; }
        /// <summary>
        /// Action that the Rover will take
        /// </summary>
        /// <param name="rover">Rover instance that will run the command</param>
        /// <returns>True if the Command has been performed, false if it could not be performed due to any circumstances like hitting an obstacle</returns>
        bool ExecuteCommand(Rover rover);
    }
}
