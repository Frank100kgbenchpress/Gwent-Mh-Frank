using System;
namespace DSL
{
    public class Token
    {
        public TokenType Type{get;set;}
        public string Lexeme{get;set;}
        public Object Literal{get;set;}
        public int Line{get;set;}
        public int Column{get;set;}
        public Token(TokenType type, string lexeme, Object literal, int line, int column) => (Type,Lexeme,Literal,Line,Column) = (type,lexeme,literal,line,column);
    }      
}