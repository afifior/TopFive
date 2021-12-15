using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TopFive
{
    public class HeapElement
    {
        public int Index { get; set; }
        public string Value { get; set; }

    }
    public class MaxHeap
    {
        private static readonly object _locker = new object();
        private readonly HeapElement[] _elements;
        private Dictionary<string, int> _values;
        private int _size;

        public MaxHeap(int size)
        {
            _elements = new HeapElement[size];
            _values = new Dictionary<string, int>();
        }

        private int GetLeftChildIndex(int elementIndex) => 2 * elementIndex + 1;
        private int GetRightChildIndex(int elementIndex) => 2 * elementIndex + 2;
        private int GetParentIndex(int elementIndex) => (elementIndex - 1) / 2;

        private bool HasLeftChild(int elementIndex) => GetLeftChildIndex(elementIndex) < _size;
        private bool HasRightChild(int elementIndex) => GetRightChildIndex(elementIndex) < _size;
        private bool IsRoot(int elementIndex) => elementIndex == 0;

        private HeapElement GetLeftChild(int elementIndex) => _elements[GetLeftChildIndex(elementIndex)];
        private HeapElement GetRightChild(int elementIndex) => _elements[GetRightChildIndex(elementIndex)];
        private HeapElement GetParent(int elementIndex) => _elements[GetParentIndex(elementIndex)];

        private void Swap(int firstIndex, int secondIndex)
        {
            var temp = _elements[firstIndex];
            _elements[firstIndex] = _elements[secondIndex];
            _elements[secondIndex] = temp;
        }

        public bool IsEmpty()
        {
            return _size == 0;
        }

        public HeapElement Peek()
        {
            if (_size == 0)
                return null;

            return _elements[0];
        }

        public HeapElement Pop()
        {
            if (_size == 0)
                return null;

            var result = _elements[0];
            _elements[0] = _elements[_size - 1];
            _values.Remove(result.Value);
            _size--;

            ReCalculateDown();

            return result;
        }

        public void Add(HeapElement element)
        {
            if(_values.ContainsKey(element.Value))
            {
                lock (_locker)
                {
                    for (int i = 0; i < _elements.Length; i++)
                    {
                        if (_elements[i].Value.Equals(element.Value))
                        {
                            {
                                _elements[i].Index = element.Index;
                                for (int index = i; index > 0; index--)
                                {
                                    Swap(index, index - 1);
                                }

                                ReCalculateUp();
                            }
                            return;
                        }
                    }
                }
            }
            if (_size == _elements.Length)
            {
                if (_elements[_elements.Length - 1].Index < element.Index)
                {
                    lock (_locker)
                    {
                        if (_elements[_elements.Length - 1].Index < element.Index) 
                        { 
                            _values.Remove(_elements[_elements.Length - 1].Value);
                            _elements[_elements.Length - 1] = element;
                            _values[element.Value] = 1;
                            ReCalculateUp();
                        }
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                lock (_locker)
                {
                    _elements[_size] = element;
                    _size++;
                    _values[element.Value] = 1;
                    ReCalculateUp();
                }
            }
        }

        private void ReCalculateDown()
        {
            int index = 0;
            while (HasLeftChild(index))
            {
                var biggerIndex = GetLeftChildIndex(index);
                if (HasRightChild(index) && GetRightChild(index)?.Index > GetLeftChild(index)?.Index)
                {
                    biggerIndex = GetRightChildIndex(index);
                }

                if (_elements[biggerIndex]?.Index < _elements[index]?.Index)
                {
                    break;
                }

                Swap(biggerIndex, index);
                index = biggerIndex;
            }
        }

        private void ReCalculateUp()
        {
            var index = _size - 1 ;
            while (!IsRoot(index) && _elements[index]?.Index > GetParent(index)?.Index)
            {
                var parentIndex = GetParentIndex(index);
                Swap(parentIndex, index);
                index = parentIndex;
            }
        }
        async public Task<string> ToStringAsync()
        {
            var task = Task.Run(() =>
            {
                var sb = new StringBuilder();
                for (int i = 0; i < _size; i++)
                {
                    sb.Append(_elements[i].Value)
                       .Append(" ")
                       .Append(_elements[i].Index)
                       .Append("\r\n");
                }
                return sb.ToString();
            });

            return await task;
        }
    }
}
