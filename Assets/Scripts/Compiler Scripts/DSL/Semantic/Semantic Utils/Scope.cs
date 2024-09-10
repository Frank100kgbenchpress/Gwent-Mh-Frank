using System.Collections.Generic;
namespace DSL
{
    public class Scope
    {
        public Scope? Parent;
        public List<string> elements = new();
        public Scope CreateChild()
        {
            Scope child = new Scope();
            child.Parent = this;
            return child;
        }
    }   
}