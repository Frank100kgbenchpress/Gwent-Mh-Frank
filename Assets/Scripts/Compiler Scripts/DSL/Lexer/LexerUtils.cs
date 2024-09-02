namespace DSL
{   
    public static class LexerUtils
    {
        public static bool IsAlphabet(char c) => (char.IsLetter(c) || c == '_');
        public static bool IsAlphabetOrNum(char c) => (char.IsDigit(c) || IsAlphabet(c));
    }
}