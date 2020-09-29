using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MartianRobots.Model;
using Xunit;

namespace MartianRobots.Tests
{
	public class SimulatorTests
	{
		[Fact]
		public void TestInputOutput()
		{
			Simulator simulator = new Simulator();
			simulator.AddInstructionRange(new[]
			{
				"5 3",
				"1 1 E",
				"RFRFRFRF",
				"3 2 N",
				"FRRFLLFFRRFLL",
				"0 3 W",
				"LLFFFLFLFL"
			});

			string expected = String.Join(Environment.NewLine, "1 1 E", "3 3 N LOST", "2 3 S");
			Assert.Equal(expected, String.Join(Environment.NewLine, simulator.GetOutput()));
		}

		[Fact]
		public void TestMininalGrid()
		{
			Simulator simulator = new Simulator();
			simulator.AddInstructionRange(new[]
			{
				"0 0",
				"0 0 E",
				"FRFRFRFRF",
				"0 0 N",
				"FRFLFFRFL"
			});

			string expected = String.Join(Environment.NewLine, "0 0 E LOST", "0 0 N");
			Assert.Equal(expected, String.Join(Environment.NewLine, simulator.GetOutput()));
		}

		[Theory]
		[InlineData("5 3", "RFRFRFRF", "3 2 N", "FRRFLLFFRRFLL", "0 3 W", "LLFFFLFLFL")]
		[InlineData("5 3", "1 1 E", "3 2 N", "FRRFLLFFRRFLL", "0 3 W", "LLFFFLFLFL")]
		[InlineData("1 1 E", "RFRFRFRF", "3 2 N", "FRRFLLFFRRFLL", "0 3 W", "LLFFFLFLFL")]
		[InlineData("5 3", "3 4 N", "FRRFLLFFRRFLL")]
		public void ShouldFailWithException(params string[] input)
		{
			Simulator simulator = new Simulator();
			Assert.ThrowsAny<Exception>(() => simulator.AddInstructionRange(input));
		}

		[Theory]
		[InlineData("1 1 N", "FRLF")]
		[InlineData("1 1 E", "FFFFF")]
		[InlineData("1 1 S", "RRRRFFFLLLL")]
		[InlineData("1 1 W", "FF")]
		public void ShouldntLostSecondTime(string initialState, string command)
		{
			Simulator simulator = new Simulator();
			simulator.AddInstructionRange(new[]
			{
				"2 2",
				initialState,
				command,
				initialState,
				command
			});

			Assert.DoesNotContain("LOST", simulator.GetOutput().Last());
		}
	}
}
