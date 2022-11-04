using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MartianRobots.Abstraction;
using MartianRobots.Model.Exceptions;

namespace MartianRobots.Model
{
	public class Grid : IGrid
	{
		private readonly HashSet<Point> m_scents = new HashSet<Point>();
		private readonly List<Robot> _robots = new List<Robot>();

		public int Width { get; private set; }
		public int Height { get; private set; }

		public Grid() { }

		public Grid(string upperRightCoordinates)
		{
			Init(upperRightCoordinates);
		}

		public Point MoveRobot(Point current, Point delta)
		{
			Point newPoint = new Point(current.X + delta.X, current.Y + delta.Y);

			bool lost = newPoint.X < 0 || newPoint.Y < 0 || newPoint.X >= Width || newPoint.Y >= Height;

			if (!lost)
				return newPoint;

			if (m_scents.Contains(current))
				return current;

			m_scents.Add(current);
			throw new LostException();
		}

		private int ParseCoordinate(string text, string coordinateName)
		{
			if (!Int32.TryParse(text, out int number))
				throw new ArgumentException($"Invalid format of {coordinateName} coordinate", coordinateName);

			if (number < 0 || number > 50)
				throw new ArgumentOutOfRangeException(coordinateName);

			return number;
		}

        public void Init(string upperRightCoordinates)
        {
			if (String.IsNullOrWhiteSpace(upperRightCoordinates))
				throw new ArgumentException("Upper-right coordinates of the rectangular world not specified", nameof(upperRightCoordinates));

			string[] splits = upperRightCoordinates.Split(" ", StringSplitOptions.RemoveEmptyEntries);

			if (splits.Length != 2)
				throw new ArgumentException("Invalid format of upper-right coordinates", nameof(upperRightCoordinates));

			Width = ParseCoordinate(splits[0], "X") + 1;
			Height = ParseCoordinate(splits[1], "Y") + 1;
		}

        public void AddRobot(string initialState)
        {
			_robots.Add(new Robot(this, initialState));
        }

        public void DoInstructions(string instructions)
        {
			_robots.Last().Do(instructions);
        }

        public IEnumerable<string> GetState()
        {
			return _robots
				.Select(r => r.GetCurrentState())
				.ToList();
		}
    }
}
