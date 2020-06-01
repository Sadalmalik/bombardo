namespace Bombardo.V1
{
    static class StringExtensions
    {
        public static string Repeat(this string instr, int n)
        {
            var result = string.Empty;

            for (var i = 0; i < n; i++)
                result += instr;

            return result;
        }
    }
}
