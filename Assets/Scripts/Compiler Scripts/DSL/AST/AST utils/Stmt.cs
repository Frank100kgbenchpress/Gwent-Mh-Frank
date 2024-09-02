namespace DSL
{
    public interface Stmt : Node
    {
       public void Execute(Context context); 
    }  
}