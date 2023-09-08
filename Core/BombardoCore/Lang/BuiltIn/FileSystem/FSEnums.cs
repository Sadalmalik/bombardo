using System.Text;

namespace Bombardo.Core
{
    public enum FSMode
    {
        text,
        bin
    }
    
    public enum FSEncoding
    {
        acii, utf7, utf8, utf32
    }

    public static class FSEncodingExtensions
    {
        public static Encoding GetEncoding(this FSEncoding encoding)
        {
            switch (encoding)
            {
                case FSEncoding.acii:  return Encoding.ASCII;
                case FSEncoding.utf7:  return Encoding.UTF7;
                case FSEncoding.utf8:  return Encoding.UTF8;
                case FSEncoding.utf32: return Encoding.UTF32;
            }

            return null;
        }
    }
}