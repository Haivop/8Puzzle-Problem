namespace Cursova.Utilities
{ 
    internal class PriorityQueue<TNode> where TNode : IComparable<TNode>
    {
        private readonly List<TNode> _elements = new();
        public bool IsEmpty() => _elements.Count == 0;
        public void Enqueue(TNode item)
        {
            _elements.Add(item);
            HeapifyUp(_elements.Count - 1);
        }
        public TNode this[int index]
        {
            get
            {
                if (index < 0 || index >= _elements.Count)
                {
                    throw new IndexOutOfRangeException("Index out of range");
                }
                return _elements[index];
            }
            set
            {
                if (index < 0 || index >= _elements.Count)
                {
                    throw new IndexOutOfRangeException("Index out of range");
                }
                _elements[index] = value;
            }
        }
        public TNode Dequeue()
        {
            if (_elements.Count == 0)
                throw new InvalidOperationException("The priority queue is empty.");

            var minItem = _elements[0];
            _elements[0] = _elements[_elements.Count - 1];
            _elements.RemoveAt(_elements.Count - 1);

            if (_elements.Count > 0)
                HeapifyDown(0);

            return minItem;
        }
        public bool Contains(TNode item)
        {
            return _elements.Contains(item);
        }
        public void Update(TNode item)
        {
            int index = _elements.IndexOf(item);
            if (index >= 0)
            {
                _elements[index] = item;
                HeapifyUp(index);
                HeapifyDown(index);
            }
        }
        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parentIndex = (index - 1) / 2;
                if (_elements[index].CompareTo(_elements[parentIndex]) >= 0)
                    break;

                Swap(index, parentIndex);
                index = parentIndex;
            }
        }
        private void HeapifyDown(int index)
        {
            int lastIndex = _elements.Count - 1;
            while (index < lastIndex)
            {
                int leftChildIndex = 2 * index + 1;
                int rightChildIndex = 2 * index + 2;

                if (leftChildIndex > lastIndex) break;

                int smallerChildIndex = leftChildIndex;
                if (rightChildIndex <= lastIndex && _elements[rightChildIndex].CompareTo(_elements[leftChildIndex]) < 0)
                {
                    smallerChildIndex = rightChildIndex;
                }

                if (_elements[index].CompareTo(_elements[smallerChildIndex]) <= 0) break;

                Swap(index, smallerChildIndex);
                index = smallerChildIndex;
            }
        }
        private void Swap(int index1, int index2)
        {
            (_elements[index1], _elements[index2]) = (_elements[index2], _elements[index1]);
        }
    }
}


    