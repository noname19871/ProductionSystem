using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionSystem
{
    class Fact
    {
        
        private string id;
        private string value;

        public string Id { get { return id; } }

        public string Value { get { return value; } }

        public Fact(string i, string val)
        {
            id = i;
            value = val;
        }
    }
}
