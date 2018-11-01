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

        private List<Fact> conditions = new List<Fact>();

        private List<Fact> conclusions = new List<Fact>();

        public string Id { get { return id; } }

        public List<Fact> Conditions { get { return conditions; } }

        public List<Fact> Conclusions { get { return conclusions; } }
    }
}
