using System;
using System.Collections.Generic;
using System.Drawing;
using MartianRobots.Model.Exceptions;

namespace MartianRobots.Model
{
	public class Grid
	{
		public int Width { get; }
		public int Height { get; }
		private readonly HashSet<Point> m_scents = new HashSet<Point>();

		public Grid(string upperRightCoordinates)
		{
			if (String.IsNullOrWhiteSpace(upperRightCoordinates))
				throw new ArgumentException("Upper-right coordinates of the rectangular world not specified", nameof(upperRightCoordinates));

			string[] splits = upperRightCoordinates.Split(" ", StringSplitOptions.RemoveEmptyEntries);

			if (splits.Length != 2)
				throw new ArgumentException("Invalid format of upper-right coordinates", nameof(upperRightCoordinates));

			Width = ParseCoordinate(splits[0], "X") + 1;
			Height = ParseCoordinate(splits[1], "Y") + 1;
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
	}
}
