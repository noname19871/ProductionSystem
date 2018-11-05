using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionSystem
{
    class Rule
    {
        private string id;

        private List<string> conditions = new List<string>();

        private string conclusion;

        private int order;

        public string Id { get { return id; } }

        public List<string> Conditions { get { return conditions; } }

        public string Conclusion { get { return conclusion; } }

        public int Order { get { return order; } set { order = value; } }

        public Rule (string i, List<string> cond, string concl)
        {
            id = i;
            conditions = cond;
            conclusion = concl;
            order = 0;
        }
    }
}
