using System;
using System.Drawing;
using System.Text;
using MartianRobots.Model;
using MartianRobots.Model.Enums;
using Xunit;

namespace MartianRobots.Tests
{
	public class RobotTests
	{
		[Theory]
		[InlineData("")]
		[InlineData("1 2 3")]
		[InlineData("1 S")]
		[InlineData("1 2 Z")]
		public void ShouldFailWithArgumentException(string initialState)
		{
			Grid grid = new Grid("10 10");
			Assert.Throws<ArgumentException>(() => new Robot(grid, initialState));
		}

		[Theory]
		[InlineData("-2 45 N")]
		[InlineData("2 -5 E")]
		[InlineData("1 2 W")]
		[InlineData("100 50 S")]
		public void ShouldFailWithArgumentOutOfRangeException(string initialState)
		{
			Grid grid = new Grid("1 1");
			Assert.Throws<ArgumentOutOfRangeException>(() => new Robot(grid, initialState));
		}

		[Fact]
		public void ShouldFailWithLotOfInstrustions()
		{
			Grid grid = new Grid("10 10");
			Robot robot = new Robot(grid, "0 0 N");

			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < 101; i++)
				builder.Append(i % 2 == 0 ? "R" : "L");

			Assert.Throws<ArgumentException>(() => robot.Do(builder.ToString()));
		}

		[Theory]
		[InlineData("0 0 S", 0, 0, Orientation.South)]
		[InlineData("0 10 W", 0, 10, Orientation.West)]
		[InlineData("25 50 N", 25, 50, Orientation.North)]
		[InlineData("50 50 E", 50, 50, Orientation.East)]
		public void TestPositionOrientation(string initialState, int expectedX, int expectedY, Orientation expectedOrientation)
		{
			Grid grid = new Grid("50 50");
			Robot robot = new Robot(grid, initialState);
			Assert.Equal(new Point(expectedX, expectedY), robot.Position);
			Assert.Equal(expectedOrientation, robot.Orientation);
		}

		[Theory]
		[InlineData("0 0 N", "L", Orientation.West)]
		[InlineData("0 0 E", "L", Orientation.North)]
		[InlineData("0 0 S", "L", Orientation.East)]
		[InlineData("0 0 W", "L", Orientation.South)]
		public void ShouldRotateToLeft(string initialState, string command, Orientation expectedOrientation)
		{
			Grid grid = new Grid("0 0");
			Robot robot = new Robot(grid, initialState);
			robot.Do(command);
			Assert.Equal(expectedOrientation, robot.Orientation);
		}

		[Theory]
		[InlineData("0 0 N", "R", Orientation.East)]
		[InlineData("0 0 E", "R", Orientation.South)]
		[InlineData("0 0 S", "R", Orientation.West)]
		[InlineData("0 0 W", "R", Orientation.North)]
		public void ShouldRotateToRight(string initialState, string command, Orientation expectedOrientation)
		{
			Grid grid = new Grid("0 0");
			Robot robot = new Robot(grid, initialState);
			robot.Do(command);
			Assert.Equal(expectedOrientation, robot.Orientation);
		}

		[Theory]
		[InlineData("5 5 N", "F", "5 6 N")]
		[InlineData("5 5 E", "FF", "7 5 E")]
		[InlineData("5 5 S", "FFF", "5 2 S")]
		[InlineData("5 5 W", "FFFF", "1 5 W")]
		public void ShouldMoveForward(string initialState, string command, string expectedState)
		{
			Grid grid = new Grid("10 10");
			Robot robot = new Robot(grid, initialState);
			robot.Do(command);
			Assert.Equal(expectedState, robot.GetCurrentState());
		}

		[Theory]
		[InlineData("5 5 N", "FRRFLL")]
		[InlineData("5 5 E", "")]
		[InlineData("5 5 S", "RRRRLLLL")]
		[InlineData("5 5 W", "LRLRLRLRLR")]
		public void ShouldntChangeState(string initialState, string command)
		{
			Grid grid = new Grid("10 10");
			Robot robot = new Robot(grid, initialState);
			robot.Do(command);
			Assert.Equal(initialState, robot.GetCurrentState());
		}

		[Theory]
		[InlineData("1 1 N", "FRLF")]
		[InlineData("1 1 E", "FFFFF")]
		[InlineData("1 1 S", "RRRRFFFLLLL")]
		[InlineData("1 1 W", "FF")]
		public void ShouldLost(string initialState, string command)
		{
			Grid grid = new Grid("2 2");
			Robot robot = new Robot(grid, initialState);
			robot.Do(command);
			Assert.EndsWith("LOST", robot.GetCurrentState());
		}
	}
}
