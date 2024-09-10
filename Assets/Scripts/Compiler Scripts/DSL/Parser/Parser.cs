using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DSL
{   
    public class Parser
    {
        List<Token> Tokens{get;}    
        int Current = 0;
        public Exception Ex;
        public Parser(List<Token> tokens) => Tokens = tokens;
        #region Parser Utils
        public bool Match(TokenType type)
        {
            if(Check(type))
            {
                Advance();
                return true;
            }
            return false;
        }
        public bool Check(TokenType type) => !IsAtEnd() && Peek().Type == type;
        public bool LookAhead(TokenType type) => Current < Tokens.Count - 1 && Tokens[Current + 1].Type == type;
        public bool IsAtEnd() => Peek().Type == TokenType.EOF;
        public Token Peek() => Tokens[Current];
        Token Advance()
        {
            if(!IsAtEnd()) Current++;
            return Previous();
        }
        public Token Previous() => Tokens[Current - 1];
        Token Consume(TokenType type, string message)
        {
            UnityEngine.Debug.Log(Peek().Type + " " + Peek().Lexeme);
            if(Check(type)) return Advance();
            else throw new ParseException($"'{Peek().Lexeme}' in line {Peek().Line}: {message}");
        }
        #endregion
        #region Node Parser
        public Node Parse()  
        {  
            Program program = new Program();  

            try
            {
                while(!IsAtEnd())
                {
                    if(Match(TokenType.CARD))    AddCardOrEffect(program,true); // Parse Card //
                    else if(Match(TokenType.EFFECT))    AddCardOrEffect(program,false); // Parse Effect //
                    else    throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Card or Effect expected.");
                }
            }
            catch(Exception ex)
            {
                Ex=ex;
            } 

            return program;  
        }  
        void AddCardOrEffect(Program program, bool cardOfEffect)
        {
            Consume(TokenType.LEFT_BRACE, "Expected '{' after card");
            if(cardOfEffect)    program.CardNodes.Add(ParseCard());   
            else   program.CardNodes.Add(ParseCard());
            Consume(TokenType.RIGHT_BRACE, "Expected '}' after card declaration");
        }
        #endregion  
        #region CardNode Parser
        CardNode ParseCard()
        {
            CardNode card = new CardNode();
            int[] counter = new int[6];
            ParseCardProperties(card,counter);  
            CheckCardPropertyErrors(counter); 
            return card;
        }
        void ParseCardProperties(CardNode card , int[] counter)
        {
            while (!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())  
            {  
                if (Match(TokenType.TYPE))      ParseType(card, counter);    
                else if (Match(TokenType.NAME))  ParseCardName(card, counter);    
                else if (Match(TokenType.FACTION))      ParseFaction(card, counter);   
                else if (Match(TokenType.POWER))      ParsePower(card, counter);   
                else if (Match(TokenType.RANGE))      ParseRange(card, counter);  
                else if (Match(TokenType.ONACTIVATION))      ParseCardOnActivation(card, counter);  
                else      throw new ParseException($"'{Peek().Lexeme}' in {Peek().Line}: Invalid Card property.");        
            }
        }
        void CheckCardPropertyErrors(int[] counter)
        {
            if(counter[0]<1) throw new ParseException("A Type property is missing from card");
            else if(counter[0]>1) throw new ParseException("Only one Type is allowed");
            if(counter[1]<1) throw new ParseException("A Name property is missing from card");
            else if(counter[1]>1) throw new ParseException("Only one Name is allowed");
            if(counter[2]<1) throw new ParseException("A Faction property is missing from card");
            else if(counter[2]>1) throw new ParseException("Only one Faction is allowed");
            if(counter[3]<1) throw new ParseException("A Power property is missing from card");
            else if(counter[3]>1) throw new ParseException("Only one Power is allowed");
            if(counter[4]<1) throw new ParseException("A Range property is missing from card");
            else if(counter[4]>1) throw new ParseException("Only one Range is allowed");
            if(counter[5]<1) throw new ParseException("An OnActivation property is missing from card");
            else if(counter[5]>1) throw new ParseException("Only one OnActivation is allowed");
        }
        void ParseType(CardNode card, int[] counter)  
        {  
            counter[0]++;  
            Consume(TokenType.COLON, "Expected ':' after Type");  
            card.Type = new CardType(ParseExpression());  
            if(!Check(TokenType.RIGHT_BRACE))Consume(TokenType.COMMA,"Expected ',' after expression");  
        }  
        void ParseCardName(CardNode card, int[] counter)  
        {  
            counter[1]++;  
            Consume(TokenType.COLON, "Expected ':' after Name");  
            card.Name = new Name(ParseExpression());  
            if(!Check(TokenType.RIGHT_BRACE))Consume(TokenType.COMMA,"Expected ',' after expression");  
        }  
        void ParseFaction(CardNode card, int[] counter)  
        {  
            counter[2]++;  
            Consume(TokenType.COLON, "Expected ':' after Faction");  
            card.Faction = new Faction(ParseExpression());  
            if(!Check(TokenType.RIGHT_BRACE))Consume(TokenType.COMMA,"Expected ',' after expression");  
        }  
        void ParsePower(CardNode card, int[] counter)  
        {  
            counter[3]++;  
            Consume(TokenType.COLON, "Expected ':' after Power");  
            card.Power = new Power(ParseExpression());  
            if(!Check(TokenType.RIGHT_BRACE))Consume(TokenType.COMMA,"Expected ',' after expression");  
        }  
        void ParseRange(CardNode card, int[] counter)  
        {  
            counter[4]++;  
            Consume(TokenType.COLON, "Expected ':' after Range");  
            Consume(TokenType.LEFT_BRACKET, "Expected '['");  
            List<Expression> expressions = new List<Expression>();  
            
            for (int i = 0; i < 3; i++)  
            {  
                expressions.Add(ParseExpression());  
                if (Match(TokenType.COMMA)) continue;  
                else break;  
            }  

            Consume(TokenType.RIGHT_BRACKET, "Expected ']'");  
            if(!Check(TokenType.RIGHT_BRACE))Consume(TokenType.COMMA,"Expected ',' after expression");  
            card.Range = new Range(expressions.ToArray());  
        }
        void ParseCardOnActivation(CardNode card , int[]counter)
        {
            counter[5]+=1;
            card.OnActivation = ParseOnActivation();
        }
        #endregion
        #region Effect Parser
        EffectNode ParseEffect()
        {
            EffectNode effect = new EffectNode();
            int[] counter = new int[3];
            ParseEffectProperties(effect,counter);
            CheckEffectPropertiesError(counter);
            return effect;
        }
        void ParseEffectProperties(EffectNode effect , int[] counter)
        {
            while(!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
            {
                if(Match(TokenType.NAME)) ParseEffectName(effect,counter);
                else if(Match(TokenType.PARAMS)) ParseEffectParams(effect,counter);
                else if(Match(TokenType.ACTION)) ParseEffectAction(effect,counter);
                else    throw new ParseException($"'{Peek().Lexeme}' in {Peek().Line}: Invalid Effect property.");
            }
        }
        void ParseEffectName(EffectNode effect , int[] counter)
        {
            counter[0]+=1;
            Consume(TokenType.COLON,"Expected ':' after Name");
            effect.Name = new Name(ParseExpression());
            if(!Check(TokenType.RIGHT_BRACE))Consume(TokenType.COMMA,"Expected ',' after expression");
        }
        void ParseEffectParams(EffectNode effect , int[] counter)
        {
            counter[1]+=1;
            Consume(TokenType.COLON,"Expected ':' after Params");
            effect.Params = GetParams();
        }
        void ParseEffectAction(EffectNode effect , int[] counter)
        {
            counter[2]+=1;
            Consume(TokenType.COLON,"Expected ':' after Action");
            effect.Action = ParseAction();
        }
        void CheckEffectPropertiesError(int[] counter)
        {
            if(counter[0]<1) throw new ParseException("A Name property is missing from effect");
            else if(counter[0]>1) throw new ParseException("Only one Name is allowed");
            if(counter[1]>1) throw new ParseException("Only one Params is allowed");
            if(counter[2]<1) throw new ParseException("An Action property is missing from effect");
            else if(counter[2]>1) throw new ParseException("Only one Action is allowed");
        }
        #endregion
        #region On Activation Parser
        OnActivation ParseOnActivation()
        {
            Consume(TokenType.COLON,"Expected ':' after OnActivation");
            Consume(TokenType.LEFT_BRACKET,"Expected '['");
            OnActivation onActivation = new OnActivation();
            while(!Check(TokenType.RIGHT_BRACKET) && !IsAtEnd())
            {
                onActivation.Elements.Add(ParseOAE());
                if(!Check(TokenType.RIGHT_BRACKET) && !IsAtEnd()) Consume(TokenType.COMMA,"Expected ,");
            }
            Consume(TokenType.RIGHT_BRACKET,"Expected ']'");
            return onActivation;
        }
        OnActivationElements ParseOAE()
        {
            Consume(TokenType.LEFT_BRACE,"Expected '{'");
            OAEffect onActivationEffect = null!;
            Selector selector = null!;
            List<PostAction> postActions = new();
            ParseOnActivationElements(onActivationEffect,selector,postActions); 
            Consume(TokenType.RIGHT_BRACE,"Expected '}' after OnActivation declaration");
            return new OnActivationElements(onActivationEffect,selector,postActions);
        }
        void ParseOnActivationElements(OAEffect onActivationEffect,Selector selector , List<PostAction> postActions)
        {
            while(!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
            {
                if(Match(TokenType.ONACTIVATIONEFFECT))
                {
                    if(onActivationEffect==null)
                    {
                        Consume(TokenType.COLON,"Expected ':'");
                        onActivationEffect = ParseOAEffect();
                    }
                    else    throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Only one OAEffect per Bracks");
                }
                else if(Match(TokenType.SELECTOR))
                {
                    if(selector == null)
                    {
                        Consume(TokenType.COLON,"Expected ':'");
                        selector = ParseSelector();
                        if(selector.Source == null) throw new ParseException($"'{Peek().Lexeme}' in {Peek().Line}: Missing field");
                    }
                    else    throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Only one Selector per OAEffect");
                }
                else if(Match(TokenType.POSTACTION))
                {
                    if(postActions == null)
                    {
                        Consume(TokenType.COLON,"Expected ':'");
                        postActions.Add(ParsePostAction());
                    }
                }
                else    throw new ParseException($"'{Peek().Lexeme}' in {Peek().Line}: Invalid OnActivation field.");
            }
        }
        #endregion
        #region On Activation Effect Parser
        OAEffect ParseOAEffect()
        {
            string name = null!;
            List<Assignment> assignments = new();
            CheckOnActivationProperties(name,assignments);
            return new OAEffect(name,assignments);
        }
        void ParseIdentifier(List<Assignment> assignments)
        {
            Variable variable = ParseVariable();
            Token token = Peek();
            Consume(TokenType.COLON, "Expected ':'");
            Expression expression = ParseExpression();
            Assignment assignment = new Assignment(variable, token, expression);
            assignments.Add(assignment);
            if (!Check(TokenType.RIGHT_BRACE))  Consume(TokenType.COMMA, "Expected ','");
        }
        void ParseOnActivationName(string name)
        {
            name = Advance().Lexeme.Substring(1,Previous().Lexeme.Length-2);
            if(!Check(TokenType.RIGHT_BRACE)) Consume(TokenType.COMMA,"Expected ','");
        }
        void CheckOnActivationProperties(string name,List<Assignment>assignments)
        {
            if(Check(TokenType.STRING))
            {
                name = Advance().Lexeme.Substring(1,Previous().Lexeme.Length-2);
                if(!Check(TokenType.RIGHT_BRACE))Consume(TokenType.COMMA,"Expected ','");
                while(!Check(TokenType.SELECTOR) && !Check(TokenType.RIGHT_BRACE))
                {
                    if(Check(TokenType.IDENTIFIER))    ParseIdentifier(assignments);  
                    else  throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Invalid OAEffect field");
                }
            }
            else
            {
                Consume(TokenType.LEFT_BRACE,"Expected '{'");
                while(!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
                {
                    if(Match(TokenType.NAME))
                    {
                        if(Match(TokenType.COLON))
                        {
                            if(name == null)    ParseOnActivationName(name!);
                            else    throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Duplicate");
                        }
                        else
                        {
                            if(name == null)    ParseOnActivationName(name!);
                            else    throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Duplicate");
                        }
                    }
                    else if(Check(TokenType.IDENTIFIER))    ParseIdentifier(assignments); 
                    else    throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Invalid OAEffect field");
                }
                Consume(TokenType.RIGHT_BRACE,"Expected '}'");
            }
            if(name == null) throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: No name");
        }
        #endregion
        #region Selector Parser
        Selector ParseSelector()
        {
            Consume(TokenType.LEFT_BRACE,"Expected '{'");
            string source = null!;
            Single single = null!;
            Predicate predicate = null!;
            ParserSelectorProperties(source,single,predicate);
            return new Selector(source,single,predicate);
        }
        bool CheckSelectorSource()
        {
            return    Convert.ToString(Peek().Literal) == "deck"
                    ||Convert.ToString(Peek().Literal) == "otherDeck"
                    ||Convert.ToString(Peek().Literal) == "hand"
                    ||Convert.ToString(Peek().Literal) == "otherHand"
                    ||Convert.ToString(Peek().Literal) == "field"
                    ||Convert.ToString(Peek().Literal) == "otherField"
                    ||Convert.ToString(Peek().Literal) == "parent"
                    ||Convert.ToString(Peek().Literal) == "board";
        }  
        void ParserSelectorProperties(string source,Single single,Predicate predicate)
        {
            while(!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
            {
                if(Match(TokenType.SOURCE))
                {
                    Consume(TokenType.COLON,"Expected ':'");
                    if(source == null)
                    {
                        if(CheckSelectorSource())    source = Advance().Literal as string;
                        else    throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Invalid Source");
                    }
                    else    throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Source duplicate");
                    if(!Check(TokenType.RIGHT_BRACE)) Consume(TokenType.COMMA,"Expected ','");
                }
                else if(Match(TokenType.SINGLE))    ParseSingle(single);
                else if(Match(TokenType.PREDICATE))    CallPredicatePArse(predicate);           
                else    throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Invalid Selector field");
            }
            Consume(TokenType.RIGHT_BRACE,"Expected '}' after Selector declaration");
            if(single == null || predicate == null) throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Missing field");
        }
        void ParseSingle(Single single)
        {
            Consume(TokenType.COLON,"Expected ':'");
            if(single == null)    single = new Single(Advance());
            else    throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Single duplicate");
            if(!Check(TokenType.RIGHT_BRACE)) Consume(TokenType.COMMA,"Expected ','");
        }
        void CallPredicatePArse(Predicate predicate)
        {
            Consume(TokenType.COLON,"Expected ':'");
            if(predicate == null)    predicate = ParsePredicate();
            else    throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Predicate duplicate");
            if(!Check(TokenType.RIGHT_BRACE)) Consume(TokenType.COMMA,"Expected ','");
        }
        #endregion
        #region Post Action Parser
        PostAction ParsePostAction()
        {
            Consume(TokenType.LEFT_BRACE,"Expected '{'");
            Expression expression = null!;
            Selector selector = null!;
            List<Assignment> assignments = new();
            ParsePostActionElements(expression,selector,assignments);
            return new PostAction(expression,selector);
        }
        void ParsePostActionType(Expression expression)
        {
            Consume(TokenType.COLON,"Expected ':'");
            expression = ParseExpression();
            if(!Check(TokenType.RIGHT_BRACE)) Consume(TokenType.COMMA,"Expected ','");
        }
        void ParsePostActionSelector(Selector selector)
        {
            Consume(TokenType.COLON,"Expected ':'");
            selector = ParseSelector();
            if(selector.Source == null) selector.Source = "parent";
            if(!Check(TokenType.RIGHT_BRACE)) Consume(TokenType.COMMA,"Expected ','");
        }
        void ParsePostActionElements(Expression expression , Selector selector , List<Assignment> assignments)
        {
            while(!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
            {
                if(Match(TokenType.TYPE))    ParsePostActionType(expression);
                else if(Match(TokenType.SELECTOR)) ParsePostActionSelector(selector);
                else if(Check(TokenType.IDENTIFIER))    ParseIdentifier(assignments); //ParseIdentifier Method Was created in On Activation Parser Region ;)
                else throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Invalid PostAction field");    
            }
            Consume(TokenType.RIGHT_BRACE,"Expected '}'");
            if(expression == null) throw new Exception("Missing PostAction Type");
        }
        #endregion
        #region Predicate Parser
        Predicate ParsePredicate()
        {
            Consume(TokenType.LEFT_PAREN,"Expected '('");
            Variable unit = ParseVariable();
            unit.VariableType = Variable.Type.CARD;
            Consume(TokenType.RIGHT_PAREN,"Expected ')'");
            Consume(TokenType.LAMBDA,"Expected '=>'");
            Expression expression = ParseExpression();
            return new Predicate(unit,expression);
        }
        #endregion
        #region Action Parser
        Action ParseAction()
        {
            Consume(TokenType.LEFT_PAREN,"Expected '('");
            Variable target = ParseVariable();
            Consume(TokenType.COMMA,"Expected ','");
            Variable context = ParseVariable();
            Consume(TokenType.RIGHT_PAREN,"Expected ')'");
            Consume(TokenType.LAMBDA,"Expected '=>'");
            Consume(TokenType.LEFT_BRACE,"Expected '{'");
            StmsBlock stmsBlock = ParseStmsBlock();
            Consume(TokenType.RIGHT_BRACE,"Expected '}'");
            return new Action(target,context,stmsBlock);
        }
        #endregion
        #region Variable Parser
        Variable ParseVariable()
        {
            Variable variable = new Variable(Advance());
            VariableParser(variable); 
            return variable;
        }
        void VariableFunctionParser(Variable.Type varType,VariableComp variableComp)
        {
            Function function = ParseFunction(Previous().Literal as string);
            varType = function.Type;
            variableComp.args.Arguments.Add(function);
        }
        void VariableTypeParser(Variable.Type varType,VariableComp variableComp)
        {
            CardType type = new CardType(new String(Previous().Literal as string));
            varType = Variable.Type.STRING;
            variableComp.args.Arguments.Add(type);
        }
        void VariableNameParser(Variable.Type varType,VariableComp variableComp)
        {
            Name name = new Name(new String(Previous().Literal as string));
            varType = Variable.Type.STRING;
            variableComp.args.Arguments.Add(name);
        }
        void VariableFactionParser(Variable.Type varType,VariableComp variableComp)
        {
            Faction faction = new Faction(new String(Previous().Literal as string));
            varType = Variable.Type.STRING;
            variableComp.args.Arguments.Add(faction);
        }
        void VariablePOwerParser(Variable.Type varType,VariableComp variableComp)
        {
            PowerAsField power = new PowerAsField();
            varType = Variable.Type.INT;
            variableComp.args.Arguments.Add(power);
        }
        void VariableRangeParser(Variable.Type varType , VariableComp variableComp)
        {
            Range range = new Range(Previous().Literal as string);
            varType = Variable.Type.STRING;
            variableComp.args.Arguments.Add(range);
        }
        void VariablePointerParser(Variable.Type varType , VariableComp variableComp)
        {
            Pointer pointer = new Pointer(Previous().Literal as string);
            variableComp.args.Arguments.Add(pointer);
            if(Match(TokenType.LEFT_BRACKET))
            {
                Indexer indexer = new Indexer(Convert.ToInt32(Advance().Literal));
                Consume(TokenType.RIGHT_BRACKET,"Expected ']'");
                variableComp.args.Arguments.Add(indexer);
            }
        }
        void VariableOwnerParser(Variable.Type varType, VariableComp variableComp)
        {
            Owner owner = new Owner(Previous().Literal as string);
            varType = Variable.Type.STRING;
            variableComp.args.Arguments.Add(owner);
        }
        void VariableParser(Variable variable)
        {
            if(Check(TokenType.DOT))
            {
                VariableComp variableComp = new VariableComp(variable.Token);
                Variable.Type varType = Variable.Type.NULL;
                while(Match(TokenType.DOT) && !IsAtEnd())
                {
                    if(Match(TokenType.FUNCTION)) VariableFunctionParser(varType,variableComp);    
                    else
                    {
                        if (Match(TokenType.TYPE)) VariableTypeParser(varType,variableComp);
                        else if(Match(TokenType.NAME)) VariableNameParser(varType,variableComp);
                        else if(Match(TokenType.FACTION)) VariableFactionParser(varType,variableComp);
                        else if(Match(TokenType.POWER)) VariablePOwerParser(varType,variableComp);
                        else if(Match(TokenType.RANGE)) VariableRangeParser(varType,variableComp);
                        else if(Match(TokenType.POINTER)) VariablePointerParser(varType,variableComp);
                        else if(Match(TokenType.OWNER))  VariableOwnerParser(varType,variableComp);
                        else throw new Exception($"'{Peek().Lexeme}' in {Peek().Line}: Invalid variable");
                    }
                }
                variable = variableComp;
                variable.VariableType = varType;
            }
        }
        #endregion
        #region Statements Parser ( block , statement ,for and while)
        #region Statement Block Parser
        StmsBlock ParseStmsBlock()
        {
            StmsBlock block = new StmsBlock();
            while(!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())    block.statements.Add(ParseStm());
            return block;
        }
        #endregion
        #region Statement Parser
        Stmt ParseStm()
        {
            if(Match(TokenType.FOR)) return ParseForStm();
            else if(Match(TokenType.WHILE))  return ParseWhileStm();
            else if(Check(TokenType.IDENTIFIER))
            {
                Variable variable = ParseVariable();
                if(variable is VariableComp && Check(TokenType.SEMICOLON))
                {
                    VariableComp v = (variable as VariableComp)!;
                    if(v.args.Arguments[v.args.Arguments.Count-1].GetType() == typeof(Function))
                    {
                        Function function = (v.args.Arguments[v.args.Arguments.Count-1] as Function)!;
                    }
                    else    throw new Exception("A compound variable most end in a function");
                    Consume(TokenType.SEMICOLON,"Expected ';'");
                    return (variable as VariableComp)!;
                }
                else    return ParseAssignment(variable);
            }
            else if(Check(TokenType.FUNCTION)) return ParseFunction(Previous().Lexeme);
            else throw new System.Exception("Invalid statement");
        }
        #endregion
        #region For Statement Parser
        ForStatement ParseForStm()
        {
            Variable target = ParseVariable();
            Consume(TokenType.IN,"Expected 'in'");
            Variable targets = ParseVariable();
            Consume(TokenType.LEFT_BRACE,"Expected {");
            StmsBlock stms = ParseStmsBlock();
            Consume(TokenType.RIGHT_BRACE,"Expected }");
            Consume(TokenType.SEMICOLON,"Expected ';'");
            return new ForStatement(target,targets,stms);
        }
        #endregion
        #region While Statement Parser
        WhileStatement ParseWhileStm()
        {
            Consume(TokenType.LEFT_PAREN,"Expected '('");
            Expression expression = ParseExpression();
            Consume(TokenType.RIGHT_PAREN,"Expected ')'");
            StmsBlock stms = ParseStmsBlock();
            return new WhileStatement(expression,stms);
        }
        #endregion
        #endregion
        #region Assignement Parser
        Assignment ParseAssignment(Variable variable)
        {
            Token op = Advance();
            Expression expression = ParseExpression();
            Consume(TokenType.SEMICOLON,"Expected ';'");
            return new Assignment(variable,op,expression);
        }
        #endregion
        #region Function Parser
        Function ParseFunction(string name)
        {
            Consume(TokenType.LEFT_PAREN,"Expected '('");
            Args args = new Args();
            CheckFunctionData(args);
            Consume(TokenType.RIGHT_PAREN,"Expected ')'");
            Function function = new Function(name,args);
            return function;
        }
        void CheckFunctionData(Args args)
        {
            while(!Check(TokenType.RIGHT_PAREN) && !IsAtEnd())
            {
                if(Check(TokenType.IDENTIFIER))    args.Arguments.Add(ParseVariable());
                else if(Match(TokenType.LAMBDA))    LambdaFunction(args);
                else if(Check(TokenType.FUNCTION))    args.Arguments.Add(ParseFunction(Advance().Lexeme));
                else    args.Arguments.Add(ParseExpression());
                if(!Check(TokenType.RIGHT_PAREN)) Consume(TokenType.COMMA,"Expected ','");
            }
        }
        void LambdaFunction(Args args)
        {
            Predicate predicate = new Predicate(args.Arguments[args.Arguments.Count-1] as Variable,ParseExpression());
            args.Arguments.RemoveAt(args.Arguments.Count-1);
            args.Arguments.Add(predicate);
        }
        #endregion
        #region Get Argument Parmeters
        Args GetParams()
        {
            Consume(TokenType.LEFT_BRACE,"Expected '{' after Params");
            Args variables = new Args();
            CheckParams(variables);
            Consume(TokenType.RIGHT_BRACE,"Expected '}' after Params declaration");
            return variables;
        }
        void CheckParams(Args variables)
        {
            while(!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
            {
                var variable = ParseVariable();
                Consume(TokenType.COLON,"Expected ':' after parameter");
                if(Check(TokenType.STRINGTYPE)||Check(TokenType.NUMBERTYPE)||Check(TokenType.BOOLEANTYPE))
                {
                    variable.TypeParam(Advance().Type);
                    variables.Arguments.Add(variable);
                    if(!Check(TokenType.RIGHT_BRACE))    Consume(TokenType.COMMA,"Expected ','");
                }
                else    throw new Exception("Expected type after parameter name");
            }
        }
        #endregion
        #region Parse Expressions
        Expression ParseExpression()
        {
            var result = Equality();
            return result;
        }
        #region Types of Expressions (equality , comparison , term , factor , unary and prinary)
        #region Equality expressions Parser
        Expression Equality()
        {
            Expression expression = Comparison();
            EqualityExpressionsParser(expression);   
            return expression;
        }
        void EqualityExpressionsParser(Expression expression)
        {
            while(Match(TokenType.NOT_EQUAL)||Match(TokenType.EQUAL))
            {
                Token operators = Previous();
                Expression right = Comparison();
                expression = new BinaryExpression(expression,operators,right);
            }
        }
        #endregion
        #region Comparison Expressions Parser
        Expression Comparison()
        {
            Expression expression = Term();
            ComparisonExpressionsParser(expression);
            return expression;
        }
        void ComparisonExpressionsParser(Expression expression)
        {
            while(Match(TokenType.GREATER)||Match(TokenType.GREATER_EQUAL)||Match(TokenType.LESS)||Match(TokenType.LESS_EQUAL))
            {
                Token operators = Previous();
                Expression right = Term();
                expression = new BinaryExpression(expression,operators,right);
            }
        }
        #endregion
        #region Term Expressions Parser
        Expression Term()
        {
            Expression expression = Factor();
            CheckTermProperties(expression);
            return expression;
        }
        void TermExpressionParser(Expression expression)
        {
            Token operators = Previous();
            Expression right = Factor();
            expression = new BinaryExpression(expression,operators,right);
        }
        void CheckTermProperties(Expression expression)
        {
            if(Check(TokenType.PLUS) || Check(TokenType.MINUS))    while(Match(TokenType.PLUS)||Match(TokenType.MINUS))    TermExpressionParser(expression);
            else if(Check(TokenType.CONCAT) || Check(TokenType.CONCAT_CONCAT))    while(Match(TokenType.CONCAT)||Match(TokenType.CONCAT_CONCAT))  TermExpressionParser(expression);
        }
        #endregion
        #region Factor Expressions Parser
        Expression Factor()
        {
            Expression expression = Unary();
            FactorExpressionsParser(expression);
            return expression;
        }
        void FactorExpressionsParser(Expression expression)
        {
            while(Match(TokenType.DIVIDE)||Match(TokenType.MULTIPLY)||Match(TokenType.MODULUS))
            {
                Token operators = Previous();
                Expression right = Unary();
                expression = new BinaryExpression(expression,operators,right);
            }
        }
        #endregion
        #region Unary Expressions Parser
        Expression Unary()
        {
            if(Match(TokenType.MINUS) || Match(TokenType.PLUS_PLUS_LEFT)|| Match(TokenType.MINUS_MINUS_LEFT) )
            {
                Token operators = Previous();
                Expression right = Unary();
                return new UnaryExpression(operators,right);
            }
            else if(Match(TokenType.NOT))
            {
                Token operators = Previous();
                Expression right = Unary();
                return new UnaryExpression(operators,right);   
            }
            else if (Check(TokenType.IDENTIFIER) && (LookAhead(TokenType.PLUS_PLUS_LEFT)|| LookAhead(TokenType.MINUS_MINUS_LEFT)))
            {
                Expression left = ParseVariable();
                Token operatorToken = Advance();
                if(LookAhead(TokenType.PLUS_PLUS_LEFT))   operatorToken.Type = TokenType.PLUS_PLUS_RIGHT;
                if(LookAhead(TokenType.MINUS_MINUS_LEFT)) operatorToken.Type = TokenType.MINUS_MINUS_RIGHT;
                return new UnaryExpression(operatorToken, left);
            }
            return Primary();
        }
        #endregion
        #region Primary Expressions Parser
        Expression Primary()
        {
            if(Match(TokenType.FALSE))     return new Bool(false);
            if(Match(TokenType.TRUE))      return new Bool(true);
            if(Match(TokenType.NUMBER))    return new Number(Convert.ToInt32(Previous().Literal));
            if(Match(TokenType.STRING))    return new String(Previous().Lexeme.Substring(1,Previous().Lexeme.Length-2));
            if(Match(TokenType.LEFT_PAREN))
            {
                Expression expression = Equality();
                Consume(TokenType.RIGHT_PAREN,"Expect ')' after expression.");
                return new ExpressionGroup(expression);
            }
            if(Check(TokenType.IDENTIFIER))    return ParseVariable();
            return new Bool(false); 
            throw new System.Exception($"'{Peek().Lexeme}' in line {Peek().Line}: Unexpected token.");
        }
        #endregion
        #endregion
        #endregion
    }
}