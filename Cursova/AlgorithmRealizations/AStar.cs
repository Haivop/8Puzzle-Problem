using Cursova.Utilities;

namespace Cursova.AlgorithmRealizations
{
    internal static class A
    {
        private static int _nodesInMemory;
        private static int _iterations;
        public static (List<Node>?, string, string) Star(Node start)
        {
            _nodesInMemory = 0;
            _iterations = 0;
            var queue = new PriorityQueue<Node>();
            var visited = new List<Node>();
            queue.Enqueue(start);

            while (!queue.IsEmpty())
            {
                _iterations++;
                var current = queue.Dequeue();
                visited.Add(current);
                _nodesInMemory += 1;

                if (current.IsGoal())
                    return (current.Solution(), _iterations.ToString(), _nodesInMemory.ToString());
                
                
                var successors = current.Expand();

                foreach (var child in successors)
                {
                    if (child.IsVisited(visited)) continue;

                    if (queue.Contains(child)) {
                        queue.Update(child);
                    }
                    else {
                        queue.Enqueue(child);
                    }
                }
            } 
            return (null, _iterations.ToString(), _nodesInMemory.ToString());
        }
    }
}

