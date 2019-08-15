using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelpDesk.Bll.Components
{
    public static class CodeGenerator
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "abcdefghijqlmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
