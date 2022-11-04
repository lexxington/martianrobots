using MartianRobots.Abstraction;
using MartianRobots.Model.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace MartianRobots
{
    public class SimulatorV2 : ISimulator
	{
		private IGrid _grid;
		private ILogger<SimulatorV2> _logger;
		private SimulatorState _state = SimulatorState.WaitForInit;

		public SimulatorV2(IGrid grid, ILogger<SimulatorV2> logger)
		{
			_grid = grid;
			_logger = logger;
		}

		public void AddInstruction(string input)
		{
			switch (_state)
			{
				case SimulatorState.WaitForInit:
					_grid.Init(input);
					_logger.LogInformation($"Initialized grid with coordinates: {input}");
					_state = SimulatorState.WaitForRobot;
					break;
				case SimulatorState.WaitForRobot:
					_grid.AddRobot(input);
					_logger.LogInformation($"Added robot with initial state: {input}");
					_state = SimulatorState.WaitForRobotCommand;
					break;
				case SimulatorState.WaitForRobotCommand:
					_grid.DoInstructions(input);
					_logger.LogInformation($"Did robot instructions: {input}");
					_state = SimulatorState.WaitForRobot;
					break;

				default:
					throw new NotImplementedException();
			}
		}

		public void AddInstructionRange(IEnumerable<string> inputs)
		{
			foreach (var input in inputs)
				AddInstruction(input);
		}

		public IEnumerable<string> GetOutput()
		{
			return _grid.GetState();
		}
	}
}
