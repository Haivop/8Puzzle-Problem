using Cursova.Enumerations;

namespace Cursova.Utilities
{
    internal readonly struct ResultInfo(Node node, Status state)
    {
        public Node Node { get; } = node;
        public Status State { get; } = state;
        public int FScore { get; } = node.FScore;
    }
}
