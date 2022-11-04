using MartianRobots.Abstraction;
using MartianRobots.Model.Enums;
using System;
using System.Collections.Generic;

namespace MartianRobots
{
    public class SimulatorV2 : ISimulator
	{
		private IGrid _grid;
		private SimulatorState _state = SimulatorState.WaitForInit;

		public SimulatorV2(IGrid grid)
		{
			_grid = grid;
		}

		public void AddInstruction(string input)
		{
			switch (_state)
			{
				case SimulatorState.WaitForInit:
					_grid.Init(input);
					_state = SimulatorState.WaitForRobot;
					break;
				case SimulatorState.WaitForRobot:
					_grid.AddRobot(input);
					_state = SimulatorState.WaitForRobotCommand;
					break;
				case SimulatorState.WaitForRobotCommand:
					_grid.DoInstructions(input);
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
