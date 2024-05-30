using Cursova.Enumerations;

namespace Cursova.Utilities
{
    public class Node : IComparable<Node>
    {
        public int[,] Board { get; }
        private int[,] Goal { get; }
        private int GScore { get; }
        public int FScore { get; internal set; }
        private Node? Ancestor { get; }

        public Node(int[,] board, int[,] goal)
        {
            Board = board;
            Goal = goal;
            Ancestor = null;
            GScore = 0;
            FScore = ManhattanHeuristic();
        }
        private Node(Node parent, int[,] board)
        {
            Board = board;
            Goal = parent.Goal;
            Ancestor = parent;
            GScore = parent.GScore + 1;
            FScore = GScore + ManhattanHeuristic();
        }
        private int ManhattanHeuristic()
        {
            var distance = 0;
            var size = Board.GetLength(0);
        
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    int value = Board[i, j];
                    if (value != 0)
                    {
                        (int goalX, int goalY) = FindPositionInGoal(value);
                        distance += Math.Abs(i - goalX) + Math.Abs(j - goalY);
                    }
                }
            }

            return distance;
        }
        private (int, int) FindPositionInGoal(int value)
        {
            var size = Goal.GetLength(0);
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    if (Goal[i, j] == value)
                    {
                        return (i, j);
                    }
                }
            }
        
            throw new ArgumentException($"Value {value} not found in goal state.");
        }
        public List<Node> Expand()
        {
            List<Node> successors = new();
            var(row, column) = FindEmpty(Board);
            var steps = FindSteps(row, column);
            
            foreach (Movement step in steps)
            {
                var board = (int[,])Board.Clone();
                switch (step)
                {
                    case Movement.Up:
                        Swap(ref board, row, column, row + 1, column);
                        break;
                    case Movement.Down:
                        Swap(ref board, row, column, row - 1, column);
                        break;
                    case Movement.Left:
                        Swap(ref board, row, column, row, column + 1);
                        break;
                    case Movement.Right:
                        Swap(ref board, row, column, row, column - 1);
                        break;
                }

                if (Ancestor is null || IsNotStepBack(board, Ancestor.Board))
                {
                    successors.Add(new Node(this, board));
                }
            }
            return successors;
        }
        public List<Node> Solution()
        {
            List<Node> nodes = new List<Node>();
            Node node = this;
            while (node.Ancestor != null)
            {
                nodes.Add(node);
                node = node.Ancestor;
            }
            nodes.Reverse();
            return nodes;
        }
        private static void Swap(ref int[,] board, int row = 0, int column = 0, int secondRow = 0, int secondColumn = 0)
        { 
            (board[row, column], board[secondRow, secondColumn]) = 
                (board[secondRow, secondColumn], board[row, column]) ;
        }
        
        private static (int, int) FindEmpty(int[,] board)
        {
            var size = board.GetLength(0);
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if (board[y, x] == 0)
                    {
                        return (y, x);
                    }
                }
            }

            return (-1, -1);
        }
        private static List<Movement> FindSteps(int row, int column)
        {
            List<Movement> moves = new();
            if (row <= 1)
            {
                moves.Add(Movement.Up);
            }
            if (row >= 1)
            {
                moves.Add(Movement.Down);
            }
            if (column <= 1)
            {
                moves.Add(Movement.Left);
            }
            if (column >= 1)
            {
                moves.Add(Movement.Right);
            }
            return moves;
        }
        
        public bool IsGoal()
        {
            var size = Board.GetLength(0);
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    if (this.Board[y, x] != this.Goal[y, x])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public bool IsEqual(Node node)
        {
            var size = Board.GetLength(0);
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    if (this.Board[y, x] != node.Board[y, x])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool IsVisited(List<Node> visited)
        {
            foreach (var visit in visited)
            {
                if (IsEqual(visit))
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsSolvable()
        {
            var invCountArray = new List<int>(9);
            foreach(var el in Board) invCountArray.Add(el);
            
            var heuristic = 0;
            for (var y = 0; y < 9; y++)
            {
                for (var x = y + 1; x < 9; x++)
                {
                    if (invCountArray[x] != 0 && invCountArray[y] > invCountArray[x])
                    {
                        heuristic += 1;
                    }
                }
            }
            return heuristic % 2 == 0;
        }
        private static bool IsNotStepBack(int[,] successorBoard, int[,] ancestorBoard)
        {
            for (var y = 0; y < 3; y++)
            {
                for (var j = 0; j < 3; j++)
                {
                    if (successorBoard[y, j] != ancestorBoard[y, j])
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        
        public int CompareTo(Node? other)
        {
            return (other != null) ? FScore.CompareTo(other.FScore) : int.MaxValue;
        }
    }
}
