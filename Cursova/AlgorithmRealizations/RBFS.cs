using Cursova.Utilities;
using Cursova.Enumerations;

namespace Cursova.AlgorithmRealizations
{
    internal static class R
    {
        private static int _iterations;
        private static int _nodesInMemory;
        private static int _nodeGened;
        private static int _nodesExpanded;
        public static (List<Node>?, string, string) BFS(Node node)
        {
            _iterations = 0; _nodesInMemory = 0; 
            
            var result = RecursiveBestFirstSearch(node, int.MaxValue);
            var path = result.Node.Solution();
            _nodesInMemory += path.Count;
            
            return (result.State != Status.Success)
                ? (null, _iterations.ToString(), "0")
                : (path, _iterations.ToString(), _nodesInMemory.ToString());
        }
        private static ResultInfo RecursiveBestFirstSearch(Node current, int limit)
        {
            _iterations++;
            if (current.FScore > limit) return new ResultInfo(current, Status.Failure);
            if (current.IsGoal()) return new ResultInfo(current, Status.Success);
            
            
            var prioritizedSuccessors = new PriorityQueue<Node>();
            var successors = current.Expand();
            if (successors.Count == 0) return new ResultInfo(current, Status.Failure);
            
            foreach (var child in successors) prioritizedSuccessors.Enqueue(child);

            var best = prioritizedSuccessors[0];
            while (best.FScore <= limit)
            {
                best = prioritizedSuccessors[0];

                var alternative = successors.Count > 1 ? prioritizedSuccessors[1].FScore : int.MaxValue;
                var result = RecursiveBestFirstSearch(best, Math.Min(limit, alternative));

                if (result.State == Status.Success)
                {
                    _nodesInMemory += successors.Count;
                    return result;
                }

                best.FScore = result.FScore;
                prioritizedSuccessors.Update(best);
            }
            return new ResultInfo(best, Status.Failure);
        }
    }
}