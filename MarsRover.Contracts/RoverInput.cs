using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRover.Contracts
{
    /// <summary>
    /// DTO for getting client input into the system.
    /// </summary>
    public struct RoverInput
    {
        /// <summary>
        /// 2dimensional planet grid with coordinates and resources present in those coordinates
        /// </summary>
        /// <remarks>Uses a jagged array due to limitations in the way .net Core 3 serializes arrays</remarks>
        public string[][] terrain { get; set; }
        /// <summary>
        /// Initial battery for the Rover
        /// </summary>
        public int battery { get; set; }
        /// <summary>
        /// List of commands that the Rover will run
        /// </summary>
        public string[] commands { get; set; }
        /// <summary>
        /// Location and direction for the Rover at landing
        /// </summary>
        public PositionDto initialPosition { get; set; }
    }
}
