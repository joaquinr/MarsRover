using MarsRover.Contracts;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MarsRover.Domain
{
    /// <summary>
    /// Represents the planet where the Rover has landed. Handles checking if a move is valid for the specific terrain and stores resources at coordinates
    /// </summary>
    public class PlanetMap
    {
        Dictionary<Location, MapCellResource> MapCells;

        public PlanetMap(Dictionary<Location, MapCellResource> mapCells)
        {
            this.MapCells = mapCells;
        }

        /// <summary>
        /// Default property to get the resource in a coordinate
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>Resource value</returns>
        /// <remarks>If we pass coordinates that are out of bounds, they are considered an obstacle to avoid the Rover falling off the planet</remarks>
        public MapCellResource this[int x, int y]
        {
            get 
            {
                if (!this.MapCells.ContainsKey(new Location(x, y))) return MapCellResource.Obstacle;
                return this.MapCells[new Location(x, y)]; 
            }
        }

        /// <summary>
        /// Checks if the Rover can move to a specific X/Y position
        /// </summary>
        /// <param name="coordinate">Intended location</param>
        /// <returns></returns>
        public bool CanRobotMoveToCoordinates(Location coordinate)
        {
            return !(this[coordinate.X, coordinate.Y] == MapCellResource.Obstacle);
        }

        public static PlanetMap FromArray(string[][] terrain)
        {
            var mapCells = new Dictionary<Location, MapCellResource>();
            for (int y = 0; y < terrain.GetLength(0); y += 1) 
            {
                var yRow = terrain[y];
                for (int x = 0; x < yRow.GetLength(0); x += 1) 
                {
                    MapCellResource resource = MapStringToResources(yRow[x]);
                    mapCells.Add(new Location(x, y), resource);
                }
            }

            return new PlanetMap(mapCells);
        }

        private static MapCellResource MapStringToResources(string stringResource)
        {
            var resource = MapCellResource.Obstacle;
            switch (stringResource)
            {
                case "Fe":
                    resource = MapCellResource.Ferrum;
                    break;
                case "Se":
                    resource = MapCellResource.Selenium;
                    break;
                case "W":
                    resource = MapCellResource.Water;
                    break;
                case "Si":
                    resource = MapCellResource.Silicon;
                    break;
                case "Zn":
                    resource = MapCellResource.Zinc;
                    break;
                case "Obs":
                    resource = MapCellResource.Obstacle;
                    break;
                default:
                    resource = MapCellResource.Obstacle;
                    break;
            }

            return resource;
        }
    }
}
