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
        public string type { get; set; }
        public override string ToString()
        {
            return "type:\t\t"+ type + Environment.NewLine + "user:\t\t" + user + Environment.NewLine + "datetime:\t\t" + datetime + Environment.NewLine + "position\t\t" + position + Environment.NewLine + "\n\rcost:\t\t" + cost + Environment.NewLine;
        }
    }
}
