using System;
using System.Collections.Generic;
namespace DSL
{
    public class Lexer
    {
        readonly List<Token> Tokens = new List<Token>();
        static Dictionary<string,Token> keywords = new Dictionary<string, Token>();
        string Input{get;}
        int start = 0;
        int current = 0;
        int line = 1;
        public Lexer(string input)
        {
            Input=input;

            keywords.Add("while", new Token(TokenType.WHILE,"while","while",0,0));
            keywords.Add("for", new Token(TokenType.FOR,"for","for",0,0));
            keywords.Add("in", new Token(TokenType.IN,"in","in",0,0));
            keywords.Add("true", new Token(TokenType.TRUE,"true","true",0,0));
            keywords.Add("false", new Token(TokenType.FALSE,"false","false",0,0));
            
            keywords.Add("card", new Token(TokenType.CARD, "card", "card", 0, 0));
            keywords.Add("effect", new Token(TokenType.EFFECT, "effect", "effect", 0, 0));
            keywords.Add("Name", new Token(TokenType.NAME, "Name", "Name", 0, 0));
            keywords.Add("Params", new Token(TokenType.PARAMS, "Params", "Params", 0, 0));
            keywords.Add("Action", new Token(TokenType.ACTION, "Action", "Action", 0, 0));
            keywords.Add("Type", new Token(TokenType.TYPE, "Type", "Type", 0, 0));
            keywords.Add("Faction", new Token(TokenType.FACTION, "Faction", "Faction", 0, 0));
            keywords.Add("Power", new Token(TokenType.POWER, "Power", "Power", 0, 0));
            keywords.Add("Range", new Token(TokenType.RANGE, "Range", "Range", 0, 0));
            keywords.Add("OnActivation", new Token(TokenType.ONACTIVATION, "OnActivation", "OnActivation", 0, 0));
            keywords.Add("Effect", new Token(TokenType.ONACTIVATIONEFFECT, "Effect", "Effect", 0, 0));
            keywords.Add("Selector", new Token(TokenType.SELECTOR, "Selector", "Selector", 0, 0));
            keywords.Add("Single", new Token(TokenType.SINGLE, "Single", "Single", 0, 0));
            keywords.Add("Predicate", new Token(TokenType.PREDICATE, "Predicate", "Predicate", 0, 0));
            keywords.Add("PostAction", new Token(TokenType.POSTACTION, "PostAction", "PostAction", 0, 0));
            keywords.Add("Source", new Token(TokenType.SOURCE, "Source", "Source", 0, 0));

            keywords.Add("TriggerPlayer", new Token(TokenType.POINTER, "TriggerPlayer", "TriggerPlayer", 0, 0));
            keywords.Add("Board", new Token(TokenType.POINTER, "Board", "Board", 0, 0));
            keywords.Add("HandOfPlayer", new Token(TokenType.POINTER, "HandOfPlayer", "HandOfPlayer", 0, 0));
            keywords.Add("DeckOfPlayer", new Token(TokenType.POINTER, "DeckOfPlayer", "DeckOfPlayer", 0, 0));
            keywords.Add("FieldOfPlayer", new Token(TokenType.POINTER, "FieldOfPlayer", "FieldOfPlayer", 0, 0));
            keywords.Add("GraveyardOfPlayer", new Token(TokenType.POINTER, "GraveyardOfPlayer", "GraveyardOfPlayer", 0, 0));

            keywords.Add("Find", new Token(TokenType.METHOD, "Find", "Find", 0, 0));
            keywords.Add("Push", new Token(TokenType.METHOD, "Push", "Push", 0, 0));
            keywords.Add("SendBottom", new Token(TokenType.METHOD, "SendBottom", "SendBottom", 0, 0));
            keywords.Add("Pop", new Token(TokenType.METHOD, "Pop", "Pop", 0, 0));
            keywords.Add("Remove", new Token(TokenType.METHOD, "Remove", "Remove", 0, 0));
            keywords.Add("Shuffle", new Token(TokenType.METHOD, "Shuffle", "Shuffle", 0, 0));

            keywords.Add("Number", new Token(TokenType.NUMBERTYPE, "Number", "Number", 0, 0));
            keywords.Add("String", new Token(TokenType.STRINGTYPE, "String", "String", 0, 0));
            keywords.Add("Bool", new Token(TokenType.BOOLEANTYPE, "Bool", "Bool", 0, 0));
        }
        public List<Token> ScanTokens()
        {
            try
            {
                while (!IsAtTheEnd())
                {
                    start = current;
                    ScanToken();
                }
                Tokens.Add(new Token(TokenType.EOF, "", "" , line, current));
                return Tokens;
            }
            catch (ParseException ex)
            {
                Console.WriteLine($"Lexic error: {ex.Message}");
                throw;
            }
        }
        void ScanToken()
        {
            char c = Advance();
            switch(c)
            {
                //Single-character token
                case '(': AddToken(TokenType.LEFT_PAREN); break;
                case ')': AddToken(TokenType.RIGHT_PAREN); break;
                case '{': AddToken(TokenType.LEFT_BRACE); break;
                case '}': AddToken(TokenType.RIGHT_BRACE); break;
                case '[': AddToken(TokenType.LEFT_BRACKET); break;
                case ']': AddToken(TokenType.RIGHT_BRACKET); break;
                case ',': AddToken(TokenType.COMMA); break;
                case '.': AddToken(TokenType.DOT); break;
                case ':': AddToken(TokenType.COLON); break;
                case ';': AddToken(TokenType.SEMICOLON); break;
                case '*': AddToken(TokenType.MULTIPLY); break;
                case '^': AddToken(TokenType.POWER); break;
                case '%': AddToken(TokenType.MODULUS); break;
                //One,two or three character token
                case '+':
                if(Match('+')) AddToken(TokenType.PLUS_PLUS_LEFT);
                else if(Match('=')) AddToken(TokenType.PLUS_EQUALS);
                else AddToken(TokenType.PLUS);
                break;
                case '-':
                if(Match('-')) AddToken(TokenType.MINUS_MINUS_LEFT);
                else if(Match('=')) AddToken(TokenType.MINUS_EQUALS);
                else AddToken(TokenType.MINUS);
                break;
                case '!':
                AddToken(Match('=') ? TokenType.NOT_EQUAL : TokenType.NOT); 
                break;
                case '=':
                if(Match('=')) AddToken(TokenType.EQUAL);
                else if(Match('>')) AddToken(TokenType.LAMBDA);
                else AddToken(TokenType.ASSIGN);
                break;
                case '<':
                AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                break;
                case '>':
                AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                break;
                case '/':
                if(Match('/'))
                {
                    while(Peek() != '\n' && !IsAtTheEnd())
                    {
                        Advance();
                    }
                }
                else AddToken(TokenType.DIVIDE);
                break;
                case'@':
                AddToken(Match('@') ? TokenType.CONCAT_CONCAT : TokenType.CONCAT); 
                break;
                case'|':
                if(Match('|')) AddToken(TokenType.OR);
                break;
                case'&':
                if(Match('&')) AddToken(TokenType.AND);
                break;
                case'"': String(); break;
                case ' ':
                case '\r':
                case '\t':
                // Ignore whitespace.
                break;
                case '\n':
                line++;
                break;
                default:
                if(char.IsDigit(c)) Number();
                else if(LexerUtils.IsAlphabet(c)) Identifier();
                else throw new ParseException(line + " Unexpected character." + start + " " + current);
                break;
            }
        }
        bool IsAtTheEnd() => current >= Input.Length;
        char Advance()
        {
            current ++;
            return Input[current - 1];
        }
        bool Match(char expected)
        {
            if(IsAtTheEnd()) return false;
            if(Input[current] != expected) return false;
            current++;
            return true;
        }
        char Peek()
        {
            if(IsAtTheEnd()) return '\0';
            return Input[current];
        }
        char PeekNext()
        {
            if(current + 1 >= Input.Length) return '\0';
            return Input[current + 1];
        }
        void String()
        {
            while(Peek() != '"' && !IsAtTheEnd())
            {
                if(Peek() == '\n') line ++;
                Advance();
            }
            if(IsAtTheEnd())
            {
                throw new ParseException($"{line}: Unfinished string.");
            }
            Advance();
            string value = Input.Substring(start+1,current-start-2);
            AddToken(TokenType.STRING, value);
        }
        void Number()
        {
            while(char.IsDigit(Peek())) Advance();
            if(Peek() == '.' && char.IsDigit(PeekNext()))
            { 
                Advance();
                while(char.IsDigit(Peek())) Advance();
            }
            AddToken(TokenType.NUMBER,Double.Parse(Input.Substring(start,current-start)));
        }
        
        void Identifier()
        {
            while(LexerUtils.IsAlphabetOrNum(Peek())) Advance();
            string text = Input.Substring(start,current-start);
            TokenType type = TokenType.IDENTIFIER;
            if(keywords.ContainsKey(text))
            {
                Token token = keywords[text];
                Tokens.Add(new Token(token.Type,token.Lexeme,token.Lexeme,line,current/line)); 
            }
            else AddToken(type);
        }
        void AddToken(TokenType type) => AddToken(type,""); 
        void AddToken(TokenType type, Object literal)
        {
            string text = Input.Substring(start,current-start);
            Tokens.Add(new Token(type,text,literal,line,current/line));
        }
    }   
}