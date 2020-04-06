using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRover.Contracts
{
    /// <summary>
    /// Combination of Location and Direction to represent the current geographical position of the Rover. DTO created to avoid exposing Position business logic to external clients
    /// </summary>
    public struct PositionDto
    {
        public Location Location { get; set; }
        public Direction Facing { get; set; }

        public PositionDto(int x, int y, Direction direction)
        {
            Location = new Location(x, y);
            Facing = direction;
        }

        public override string ToString()
        {
            return $"{this.Location.ToString()} H: {this.Facing}";
        }
    }
}
