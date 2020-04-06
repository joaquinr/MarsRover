using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRover.Contracts
{
    /// <summary>
    /// Represents the coordinates and direction of the Rover. Handles basic movement and turning rules
    /// </summary>
    public class Position
    {
        public Location Coordinate { get; set; }
        public Direction Facing { get; set; }

        public Position(int x, int y, Direction facing)
        {
            Coordinate = new Location(x, y);
            Facing = facing;
        }

        /// <summary>
        /// Calculates the position the Rover would have if it moves forward from the current position
        /// </summary>
        /// <returns>Expected position after the move</returns>
        public Position MoveForward()
        {
            Position newPosition;

            switch (Facing)
            {
                case Direction.North:
                    newPosition = new Position(Coordinate.X, Coordinate.Y - 1, Facing);
                    break;
                case Direction.South:
                    newPosition = new Position(Coordinate.X, Coordinate.Y + 1, Facing);
                    break;
                case Direction.East:
                    newPosition = new Position(Coordinate.X + 1, Coordinate.Y, Facing);
                    break;
                case Direction.West:
                    newPosition = new Position(Coordinate.X - 1, Coordinate.Y, Facing);
                    break;
                default:
                    newPosition = this;
                    break;
            }

            return newPosition;
        }

        /// <summary>
        /// Calculates the position the Rover would have if it moves backwards from the current position
        /// </summary>
        /// <returns>Expected position after the move</returns>
        public Position MoveBackward()
        {
            Position newPosition;

            switch (Facing)
            {
                case Direction.North:
                    newPosition = new Position(Coordinate.X, Coordinate.Y + 1, Facing);
                    break;
                case Direction.South:
                    newPosition = new Position(Coordinate.X, Coordinate.Y - 1, Facing);
                    break;
                case Direction.East:
                    newPosition = new Position(Coordinate.X - 1, Coordinate.Y, Facing);
                    break;
                case Direction.West:
                    newPosition = new Position(Coordinate.X + 1, Coordinate.Y, Facing);
                    break;
                default:
                    newPosition = this;
                    break;
            }

            return newPosition;
        }

        /// <summary>
        /// Calculates the position the Rover would have if it turns left from the current position
        /// </summary>
        /// <returns>Expected position after the move</returns>
        public Position TurnLeft()
        {
            Position newPosition;
            switch (Facing)
            {
                case Direction.North:
                    newPosition = new Position(Coordinate.X, Coordinate.Y, Direction.West);
                    break;
                case Direction.South:
                    newPosition = new Position(Coordinate.X, Coordinate.Y, Direction.East);
                    break;
                case Direction.East:
                    newPosition = new Position(Coordinate.X, Coordinate.Y, Direction.North);
                    break;
                case Direction.West:
                    newPosition = new Position(Coordinate.X, Coordinate.Y, Direction.South);
                    break;
                default:
                    newPosition = this;
                    break;
            }
            return newPosition;
        }
        /// <summary>
        /// Calculates the position the Rover would have if it turns right from the current position
        /// </summary>
        /// <returns>Expected position after the move</returns>
        public Position TurnRight()
        {
            Position newPosition;
            switch (Facing)
            {
                case Direction.North:
                    newPosition = new Position(Coordinate.X, Coordinate.Y, Direction.East);
                    break;
                case Direction.South:
                    newPosition = new Position(Coordinate.X, Coordinate.Y, Direction.West);
                    break;
                case Direction.East:
                    newPosition = new Position(Coordinate.X, Coordinate.Y, Direction.South);
                    break;
                case Direction.West:
                    newPosition = new Position(Coordinate.X, Coordinate.Y, Direction.North);
                    break;
                default:
                    newPosition = this;
                    break;
            }
            return newPosition;
        }

        /// <summary>
        /// Converts the current Position into a DTO for its use in the client
        /// </summary>
        /// <returns>Position in a DTO shape</returns>
        public PositionDto ToDto()
        {
            return new PositionDto(this.Coordinate.X, this.Coordinate.Y, this.Facing);
        }
        /// <summary>
        /// Converts a DTO into a new instance of Position that can handle business rules
        /// </summary>
        /// <param name="initialPosition">Position to convert</param>
        /// <returns>Position in a full business object shape</returns>
        public static Position FromDto(PositionDto initialPosition)
        {
            return new Position(initialPosition.Location.X, initialPosition.Location.Y, initialPosition.Facing);
        }
        public override string ToString()
        {
            return $"{this.Coordinate.ToString()} H: {this.Facing}";
        }
    }
}
