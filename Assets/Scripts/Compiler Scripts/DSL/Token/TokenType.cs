namespace DSL
{
    /// <summary>
    /// Represents the type of a token.
    /// </summary>
    public enum TokenType
    {
         // Keywords
        LET,            // Represents the "let" keyword
        IN,             // Represents the "in" keyword
        IF,             // Represents the "if" keyword
        ELSE,           // Represents the "else" keyword
        FUNCTION,       // Represents the "function" keyword
        CARD,           //Represents the "Card" keyword
        EFFECT,         //Represents the "Effect" keyword
        WHILE,
        FOR,
        TRUE,
        FALSE,

        // Variables
        IDENTIFIER,     // Represents an identifier (variable name)
        NUMBERTYPE,     // Represents a number Identification
        STRINGTYPE,     // Represents a string Identification
        BOOLEANTYPE,    // Represents a boolean Identification
        NUMBER,         // Represents a numeric value
        STRING,         // Represents a string value
        BOOLEAN,        // Represents a boolean value

        // Separators
        LEFT_PAREN,     // Represents the left parenthesis "("
        RIGHT_PAREN,    // Represents the right parenthesis ")"
        LEFT_BRACE,     // Represents the left brace "{"
        RIGHT_BRACE,    // Represents the right brace "}"
        LEFT_BRACKET,   //Represents the left bracket "["
        RIGHT_BRACKET,  //Represents the right bracket "]"
        DOT,            //Represents a dot "."
        COLON,          //Represents a colon ":"
        SEMICOLON,      // Represents a semicolon ";"
        COMMA,          // Represents a comma ","
        QUOTATIONMARKS, // Represents a Quotation marks "" 

        // Operators
        PLUS,           // Represents the addition operator "+"
        MINUS,          // Represents the subtraction operator "-"
        MULTIPLY,       // Represents the multiplication operator "*"
        DIVIDE,         // Represents the division operator "/"
        MODULUS,        // Represents the modulus operator "%"
        POWER,          // Represents the exponentiation operator "^"
        AND,            // Represents the logical AND operator "&"
        OR,             // Represents the logical OR operator "|"
        NOT,            // Represents the logical NOT operator "!"
        NOT_EQUAL,      // Represents the inequality operator "!="
        EQUAL,          // Represents the equality operator "=="
        ASSIGN,         // Represents the assignment operator "="
        GREATER,        // Represents the greater than operator ">"
        GREATER_EQUAL,  // Represents the greater than or equal to operator ">="
        LESS,           // Represents the less than operator "<"
        LESS_EQUAL,     // Represents the less than or equal to operator "<="
        CONCAT,         // Represents the string concatenation operator "@"
        CONCAT_CONCAT,   //Represents the mark to ignore keywords "@@"
        LAMBDA,         // Represents the lambda function operator "=>"
        PLUS_PLUS_RIGHT,      //Represents the operator "++" form right
        PLUS_PLUS_LEFT,  //Represents the operator "++" from left
        PLUS_EQUALS,    //Represents the operator "+="
        MINUS_MINUS_LEFT,    //Represents the operator "--" from left
        MINUS_MINUS_RIGHT,   // Represents the operator "--" from right
        MINUS_EQUALS,   //Represents the operator "-="


        //Card Keywords//

        NAME,
        PARAMS,
        ACTION,
        TYPE,
        FACTION,
        ATTACK,
        RANGE,
        ONACTIVATION,
        SELECTOR,
        SINGLE,
        PREDICATE,
        POSTACTION,
        SOURCE,
        MELEE,
        RANGED,
        SIEGE,
       
        //Effect Keywords
        POINTER,
        ONACTIVATIONEFFECT,
        METHOD,

        //Unknown code//
        UNKNOWN,


        // End of File
        EOF             // Represents the end of file marker
    }
}