using System;
using System.Collections.Generic;
namespace DSL
{
    
    public class Args : Node
    {
        public List<Node> Arguments;
        public Args() => Arguments = new List<Node>();
        public void Print(int pos = 0)
        {
            Console.WriteLine(new string(' ', pos) + "Args:");
            foreach (var arg in Arguments)
            {
                arg.Print(pos + 2);
            }
        }
    }   
}