using System;
using System.Collections.Generic;
using System.Text;

namespace Bombardo.Core
{
    public class BombardoException : Exception
    {
        public BombardoException(string message) : base(message)
        {
        }

        public BombardoException(Exception intern) : base(intern.Message, intern)
        {
        }

        public BombardoException(string methodName, Exception intern) : base(string.Format("<{0}> {1}", methodName, intern.Message), intern)
        {
        }
    }
}