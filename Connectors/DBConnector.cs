
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using RuleEngine.Interfaces;
using TopFive;

namespace RuleEngine.Connectors
{
    public class DBConnector : IDBConnector
    {
        private static ConcurrentDictionary<string, int> _dictionary;
        private static MaxHeap _heap;
        static DBConnector()
        {
            _heap = new MaxHeap(5);
             int initialCapacity = 101;
             int concurrencyLevel = Environment.ProcessorCount * 2;
             _dictionary = new ConcurrentDictionary<string, int>(concurrencyLevel, initialCapacity);
        }

        public async Task AddString(string text)
        {
            var task = Task.Run(() =>
            {
                var chunks = text?.Split(',');
                chunks.ToList().ForEach(chunk =>
               {
                   if (_dictionary.ContainsKey(chunk))
                   {
                       _dictionary[chunk] = ++_dictionary[chunk];
                   }
                   else
                   {
                       _dictionary[chunk] = 1;
                   }
                   _heap.Add(new HeapElement() { Index = _dictionary[chunk], Value = chunk });
               });
            });
            await task;


        }
        public async Task<string> GetTopMembers()
        {
            return await _heap.ToStringAsync();
        }
    }
}

