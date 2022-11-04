using System.Collections.Generic;

namespace MartianRobots.Abstraction
{
    public interface ISimulator
    {
        void AddInstruction(string input);
        void AddInstructionRange(IEnumerable<string> inputs);
        IEnumerable<string> GetOutput();
    }
}
