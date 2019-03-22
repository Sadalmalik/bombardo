using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bombardo
{
    public class Frame
    {
        public string name;
        public Atom value;

        public Frame(string name, Atom value)
        {
            this.name = name;
            this.value = value;
        }
    }
}