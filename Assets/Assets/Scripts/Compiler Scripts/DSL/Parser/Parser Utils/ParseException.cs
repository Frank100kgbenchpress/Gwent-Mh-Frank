using System;

namespace DSL
{
    public class ParseException : Exception
    {
        public ParseException(string message) : base(message){}
    }
}