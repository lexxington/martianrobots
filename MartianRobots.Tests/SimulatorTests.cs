using MartianRobots.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xunit;

namespace MartianRobots.Tests
{
    public class SimulatorTests
	{
		private readonly ISimulator _simulator;

		public SimulatorTests()
		{
			var serviceProvider = new ServiceCollection()
				.AddSingleton<ISimulator, Simulator>()
				.BuildServiceProvider();

			_simulator = serviceProvider.GetRequiredService<ISimulator>();
		}

		[Fact]
		public void TestInputOutput()
		{
			_simulator.AddInstructionRange(new[]
			{
				"5 3",
				"1 1 E",
				"RFRFRFRF",
				"3 2 N",
				"FRRFLLFFRRFLL",
				"0 3 W",
				"LLFFFLFLFL"
			});

			var expected = string.Join(Environment.NewLine, "1 1 E", "3 3 N LOST", "2 3 S");
			Assert.Equal(expected, string.Join(Environment.NewLine, _simulator.GetOutput()));
		}

		[Fact]
		public void TestMininalGrid()
		{
			_simulator.AddInstructionRange(new[]
			{
				"0 0",
				"0 0 E",
				"FRFRFRFRF",
				"0 0 N",
				"FRFLFFRFL"
			});

			var expected = string.Join(Environment.NewLine, "0 0 E LOST", "0 0 N");
			Assert.Equal(expected, string.Join(Environment.NewLine, _simulator.GetOutput()));
		}

		[Theory]
		[InlineData("5 3", "RFRFRFRF", "3 2 N", "FRRFLLFFRRFLL", "0 3 W", "LLFFFLFLFL")]
		[InlineData("5 3", "1 1 E", "3 2 N", "FRRFLLFFRRFLL", "0 3 W", "LLFFFLFLFL")]
		[InlineData("1 1 E", "RFRFRFRF", "3 2 N", "FRRFLLFFRRFLL", "0 3 W", "LLFFFLFLFL")]
		[InlineData("5 3", "3 4 N", "FRRFLLFFRRFLL")]
		public void ShouldFailWithException(params string[] input)
		{
			Assert.ThrowsAny<Exception>(() => _simulator.AddInstructionRange(input));
		}

		[Theory]
		[InlineData("1 1 N", "FRLF")]
		[InlineData("1 1 E", "FFFFF")]
		[InlineData("1 1 S", "RRRRFFFLLLL")]
		[InlineData("1 1 W", "FF")]
		public void ShouldntLostSecondTime(string initialState, string command)
		{
			_simulator.AddInstructionRange(new[]
			{
				"2 2",
				initialState,
				command,
				initialState,
				command
			});

			Assert.DoesNotContain("LOST", _simulator.GetOutput().Last());
		}
	}
}
