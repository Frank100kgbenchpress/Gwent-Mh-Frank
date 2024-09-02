using System;
namespace DSL
{
    public class Pointer : Node
    {
        public string Pointer_;
        public Pointer(string pointer) => Pointer_ = pointer;
        public void Print(int pos = 0) => Console.WriteLine(new string(' ', pos) + "Pointer: " + Pointer_);
    }   
}