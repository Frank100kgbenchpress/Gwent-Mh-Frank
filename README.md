# GWENT MH PROJECT

## Gwent Interpreter Project

This project is an interpreter for the popular card game Gwent, developed by Frank Alberto Piz Torriente.

### Important

## Token System (Token.cs and TokenType.cs)

Our Gwent interpreter uses a robust token system defined in Token.cs and TokenType.cs:

### Token.cs
- Defines the Token class representing individual lexical units
- Each Token contains Type, Lexeme, Literal value, Line, and Column information
- Provides a concise constructor for easy token creation

### TokenType.cs
- Enumerates all possible token types in our Gwent DSL
- Includes general programming constructs like keywords, operators, and literals
- Features Gwent-specific token types such as CARD, EFFECT, FACTION, and RANGE
- Supports advanced language features with tokens like LAMBDA and PREDICATE
- Includes tokens for game mechanics like ONACTIVATION and POSTACTION

This token system forms the foundation of our lexical analysis, enabling precise interpretation of Gwent DSL code.

### Important
Abstract Syntax Tree (AST) Scripts
Our Gwent interpreter uses a set of AST (Abstract Syntax Tree) scripts to represent the structure of the Gwent DSL code. The main AST scripts are:

Program.cs
Defines the Program class, which serves as the root node of the AST
Contains lists of CardNode and EffectNode objects
Provides a Print method for debugging, which recursively prints the entire AST structure
CardAST.cs
Defines the CardNode class, representing a Gwent card in the AST
Includes properties for card attributes such as Type, Name, Faction, Power, Range, and OnActivation
Contains nested classes for each card attribute (e.g., Name, CardType, Faction, etc.)
Implements Print methods for each class to facilitate debugging and visualization of the AST
EffectAST.cs
Defines various classes related to card effects and actions in the Gwent DSL
Key classes include EffectNode, OnActivation, Selector, Predicate, PostAction, and Action
Represents complex effect structures such as activation conditions, selectors, and post-actions
Implements Print methods for each class to aid in debugging and AST visualization
These AST scripts work together to create a comprehensive representation of Gwent cards and their effects, forming the backbone of our interpreter's ability to understand and process Gwent DSL code.
[!TIP]
AST conteins all necesary classes to represent the structure of the Gwent DSL code.(variable, variable complex , statements, etc.)

### Project Description

1 Lexer
The Lexer class is a crucial component of our Gwent interpreter, responsible for tokenizing the input source code. It performs the following key functions:

- Scans the input string and breaks it down into individual tokens
- Recognizes various token types including keywords, identifiers, literals, and operators
- Handles single-character tokens, multi-character tokens, and complex tokens like strings and numbers
- Implements error detection for unexpected characters and unfinished strings
- Utilizes a dictionary to efficiently identify keywords specific to our Gwent DSL
- Supports Gwent-specific tokens such as card attributes, effect keywords, and game-related functions

The Lexer is designed to be flexible and extensible, allowing for easy addition of new keywords and token types as the Gwent DSL evolves.

## Parser (Parser.cs)

The Parser class is a fundamental component of our Gwent interpreter, responsible for analyzing the syntactic structure of the tokenized input. Key features include:

- Implements recursive descent parsing for the Gwent Domain-Specific Language (DSL)
- Constructs an Abstract Syntax Tree (AST) representing the structure of the Gwent cards and effects
- Handles parsing of card properties, effects, selectors, and actions
- Supports complex language constructs such as predicates, lambda functions, and variable assignments
- Implements error handling and reporting for syntax errors
- Parses control flow structures like for-loops and while-loops
- Manages expression parsing with proper operator precedence
- Handles Gwent-specific concepts such as card types, factions, and game zones (deck, hand, field, etc.)

The Parser is designed to be robust and extensible, allowing for easy addition of new language features as the Gwent DSL evolves.

## Semantic Analyzer (Semantic.cs)

The SemanticalCheck class in Semantic.cs performs crucial semantic analysis for our Gwent interpreter. Key features include:

- Validates the semantic correctness of Gwent cards and effects
- Implements type checking for expressions and assignments
- Verifies the correct usage of variables, functions, and game-specific constructs
- Checks for proper scoping of variables in different contexts
- Validates card properties such as type, name, faction, power, and range
- Ensures correct usage of selectors, predicates, and post-actions in card effects
- Performs semantic checks on control flow structures like for and while loops
- Validates function calls and their arguments
- Implements a symbol table for tracking variable declarations and types
- Provides detailed error reporting for semantic issues

The semantic analyzer ensures that the Gwent DSL code is not only syntactically correct but also logically sound and adheres to the game's rules and constraints.

[!HINT] The SymbolTable.cs script is a crucial component of our Gwent interpreter's semantic analysis:

Implements a SymbolTable class for managing variable and function scopes
Uses a stack of dictionaries to handle nested scopes
Provides methods for pushing and popping scopes
Includes functionality to define and look up variables and functions
Implements error checking for duplicate definitions and undeclared identifiers
Supports type tracking for variables and function return types
Enables efficient scope-aware symbol management throughout the interpretation process
This symbol table implementation ensures proper variable and function handling, supporting the semantic integrity of our Gwent DSL.

[!HINT] The Context.cs script is a key component in our Gwent interpreter:

Defines a Context class that manages the game state and resources
Contains dictionaries for storing cards, effects, and variables
Includes a reference to the TurnSystem for game flow management
Provides methods for adding and retrieving cards and effects
Implements error checking for duplicate card and effect names
Utilizes Unity's ScriptableObject for card instantiation
Serves as a central hub for accessing and manipulating game elements during interpretation
This Context class acts as a crucial bridge between the interpreter and the game state, ensuring efficient management of Gwent-specific resources and logic.

[!IMPORTANT] The Evaluator.cs script is a core component of our Gwent interpreter:

Implements the Evaluator class responsible for executing card effects
Takes a Card and Context as input to evaluate effects
Contains methods to evaluate On Activation Elements (OAE), effects, and post-actions
Implements a selector system to filter and choose target cards
Handles variable assignments and context updates during effect evaluation
Supports various card zones (hand, deck, field) for both players
Executes action blocks and statement sequences
Integrates with the TurnSystem to manage player turns and game state
Provides a flexible framework for implementing complex card interactions and game mechanics
This Evaluator forms the heart of our Gwent interpreter, translating the parsed DSL into actual game actions and state changes.


### Installation

[Copy Assets folder to your Unity project]

### Usage

[Play a game of Gwent and can use the Gwent DSL to create new cards and effects]

### Features

[Play and Compile]

### Contributing

[follow if yoy like]



### Contact
gmaill:
[piz.frank29@gmail.com]
