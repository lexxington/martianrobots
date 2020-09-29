using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MartianRobots.Model;
using MartianRobots.Model.Enums;

namespace MartianRobots
{
	public class Simulator
	{
		private Grid m_grid;
		private SimulatorState m_state = SimulatorState.WaitForInit;
		private readonly List<Robot> m_robots = new List<Robot>();

		public void AddInstruction(string input)
		{
			switch (m_state)
			{
				case SimulatorState.WaitForInit:
					m_grid = new Grid(input);
					m_state = SimulatorState.WaitForRobot;
					break;
				case SimulatorState.WaitForRobot:
					m_robots.Add(new Robot(m_grid, input));
					m_state = SimulatorState.WaitForRobotCommand;
					break;
				case SimulatorState.WaitForRobotCommand:
					m_robots.Last().Do(input);
					m_state = SimulatorState.WaitForRobot;
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
			return m_robots.Select(r => r.GetCurrentState()).ToArray();
		}
	}
}
