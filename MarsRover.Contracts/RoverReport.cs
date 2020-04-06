using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRover.Contracts
{
    /// <summary>
    /// Output DTO with all required information from the simulated run
    /// </summary>
    public class RoverReport
    {
        /// <summary>
        /// Grid coordinates visited during the simulation
        /// </summary>
        public List<Location> VisitedCells { get; set; }
        /// <summary>
        /// List of samples collected during the simulation
        /// </summary>
        public List<string> SamplesCollected { get; set; }
        /// <summary>
        /// Battery level at the end of the run
        /// </summary>
        public int Battery { get; set; }
        /// <summary>
        /// Location and heading at the end of the run
        /// </summary>
        public PositionDto FinalPosition { get; set; }
    }
}
