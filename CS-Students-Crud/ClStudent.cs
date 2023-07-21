using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Students_Crud
{
    internal class ClStudent
    {
        public int id { get; set; }
        public string name { get; set; }
        public string mail { get; set; }
        public byte[] picture { get; set; }
        public string comment { get; set; }
    }
}
