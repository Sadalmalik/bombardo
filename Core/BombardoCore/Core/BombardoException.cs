using System;

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

        public BombardoException(string methodName, Exception intern) : base($"<{methodName}> {intern.Message}", intern)
        {
        }
    }
}