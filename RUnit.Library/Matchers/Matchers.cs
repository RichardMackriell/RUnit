using RUnit.Errors;
using RUnit.Introspection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUnit.Library.Matchers
{
    public static class Matchers
    {
        public static bool Same<T>(T a, T b)
        {
            if (a?.Equals(b) ?? false)
                throw new MatchedSuccessException();
            else
                throw new MatchedFailureException();
        }
        public static bool Different<T>(T a, T b)
        {
            if (!a?.Equals(b) ?? true)
                throw new MatchedSuccessException();
            else
                throw new MatchedFailureException();
        }
        public static bool GreaterThan(double a, double b)
        {
            if (a > b)
                throw new MatchedSuccessException();
            else
                throw new MatchedFailureException();
        }
        public static bool GreaterThan(int a, int b)
        {
            if (a > b)
                throw new MatchedSuccessException();
            else
                throw new MatchedFailureException();
        }
        public static bool LessThan(double a, double b)
        {
            if (a < b)
                throw new MatchedSuccessException();
            else
                throw new MatchedFailureException();
        }
        public static bool LessThan(int a, int b)
        {
            if (a < b)
                throw new MatchedSuccessException();
            else
                throw new MatchedFailureException();
        }        
    }
}
