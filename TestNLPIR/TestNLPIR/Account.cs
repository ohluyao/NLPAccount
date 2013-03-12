using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNLPIR
{
    class Account
    {
        public string user { get; set; }
        public string datetime { get; set; }
        public string position { get; set; }
        public string cost { get; set; }

        public override string ToString()
        {
            return "user: " + user + "\ndatetime: " + datetime + "\nposition" + position + "\ncost:" + cost;
        }
    }
}
