using System;
using MartianRobots.Model;
using Xunit;

namespace MartianRobots.Tests
{
	public class GridTests
	{
		[Theory]
		[InlineData("")]
		[InlineData("1 2 3")]
		[InlineData("1 s")]
		public void ShouldFailWithArgumentException(string upperRightCoordinates)
		{
			Assert.Throws<ArgumentException>(() => new Grid(upperRightCoordinates));
		}

		[Theory]
		[InlineData("-2 45")]
		[InlineData("2 -5")]
		[InlineData("1 51")]
		[InlineData("100 50")]
		public void ShouldFailWithArgumentOutOfRangeException(string upperRightCoordinates)
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => new Grid(upperRightCoordinates));
		}

		[Theory]
		[InlineData("0 0", 1, 1)]
		[InlineData("50 10", 51, 11)]
		public void TestWidthHeight(string upperRightCoordinates, int expectedWidth, int expectedHeight)
		{
			Grid grid = new Grid(upperRightCoordinates);
			Assert.Equal(expectedWidth, grid.Width);
			Assert.Equal(expectedHeight, grid.Height);
		}
	}
}
