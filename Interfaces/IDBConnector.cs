
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuleEngine.Interfaces
{
    public interface IDBConnector
    {
        public Task AddString(string text);
        public Task<string> GetTopMembers();
    }
}
