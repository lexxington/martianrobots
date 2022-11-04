using System.Collections.Generic;
using System.Drawing;

namespace MartianRobots.Abstraction
{
    public interface IGrid
    {
        int Width { get; }
        int Height { get; }

        void Init(string upperRightCoordinates);
        void AddRobot(string initialState);
        void DoInstructions(string instructions);
        Point MoveRobot(Point current, Point delta);
        IEnumerable<string> GetState();
    }
}
