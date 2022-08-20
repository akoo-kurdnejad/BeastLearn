using System;
using System.Collections.Generic;
using System.Text;

namespace BeastLearn.Application.Generators
{
    public class FixedText
    {
        public static string FixedEmail(string email)
        {
            return email.Trim().ToLower();
        }
    }
}
