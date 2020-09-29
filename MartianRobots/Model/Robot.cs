using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MartianRobots.Model.Enums;
using MartianRobots.Model.Exceptions;

namespace MartianRobots.Model
{
	public class Robot
	{
		private readonly Grid m_grid;
		public Point Position { get; private set; }
		public Orientation Orientation { get; private set; }
		public bool Lost { get; private set; }

		public Robot(Grid grid, string initialState)
		{
			m_grid = grid ?? throw new ArgumentNullException(nameof(grid));

			if (String.IsNullOrWhiteSpace(initialState))
				throw new ArgumentException("Position of robot not specified", nameof(initialState));

			string[] splits = initialState.Split(" ", StringSplitOptions.RemoveEmptyEntries);

			if (splits.Length != 3)
				throw new ArgumentException("Invalid position of robot", nameof(initialState));

			Position = new Point(ParseCoordinate(splits[0], "X", m_grid.Width), ParseCoordinate(splits[1], "Y", m_grid.Height));
			Orientation = ParseOrientation(splits[2]);
		}

		public void Do(string instructions)
		{
			if (instructions.Length > 100)
				throw new ArgumentException("Instruction string is out of limit", nameof(instructions));

			foreach (var letter in instructions)
			{
				try
				{
					Instruction instruction = ToInstruction(letter);
					Do(instruction);
				}
				catch (LostException)
				{
					Lost = true;
					break;
				}
			}
		}

		public string GetCurrentState()
		{
			string result = $"{Position.X} {Position.Y} {OrientationToString()}";
			return Lost ? result + " LOST" : result;
		}

		#region Private Methods

		private int ParseCoordinate(string text, string coordinateName, int bound)
		{
			if (!Int32.TryParse(text, out int number))
				throw new ArgumentException($"Invalid format of {coordinateName} coordinate", coordinateName);

			if (number < 0 || number >= bound)
				throw new ArgumentOutOfRangeException(coordinateName);

			return number;
		}

		private Orientation ParseOrientation(string orientation)
		{
			switch (orientation)
			{
				case "N":
					return Orientation.North;
				case "E":
					return Orientation.East;
				case "S":
					return Orientation.South;
				case "W":
					return Orientation.West;
			}

			throw new ArgumentException("Invalid format of orientation", nameof(orientation));
		}

		private string OrientationToString()
		{
			switch (Orientation)
			{
				case Orientation.North:
					return "N";
				case Orientation.East:
					return "E";
				case Orientation.South :
					return "S";
				case Orientation.West:
					return "W";
			}

			throw new NotImplementedException();
		}

		private Instruction ToInstruction(char instruction)
		{
			switch (instruction)
			{
				case 'L':
					return Instruction.Left;
				case 'R':
					return Instruction.Right;
				case 'F':
					return Instruction.Forward;
			}

			throw new ArgumentException($"Invalid instruction '{instruction}'", nameof(instruction));
		}

		private void Rotate(int step)
		{
			int count = Enum.GetValues(typeof(Orientation)).Length;
			Orientation = (Orientation)(((int)Orientation + step + count) % count);
		}

		private void Do(Instruction instruction)
		{
			switch (instruction)
			{
				case Instruction.Left:
					Rotate(-1);
					break;
				case Instruction.Right:
					Rotate(1);
					break;
				case Instruction.Forward:
					Position = m_grid.MoveRobot(Position, GetDelta());
					break;
			}
		}

		private Point GetDelta()
		{
			switch (Orientation)
			{
				case Orientation.North:
					return new Point(0, 1);
				case Orientation.East:
					return new Point(1, 0);
				case Orientation.South:
					return new Point(0, -1);
				case Orientation.West:
					return new Point(-1, 0);
			}

			throw new NotImplementedException();
		}
		
		#endregion

	}
}
