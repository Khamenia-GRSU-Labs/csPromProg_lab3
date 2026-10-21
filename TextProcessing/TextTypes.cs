using System.Text;
using System.Xml.Serialization;

namespace lab3.TextProcessing;

[XmlInclude(typeof(Word))]
[XmlInclude(typeof(Punctuation))]
public class Token
{
    public string Value { get; set; } = string.Empty;

    public Token() { }

    public Token(string value) { Value = value; }
}

public class Word : Token
{
    public Word() { }
    public Word(string value) : base(value) { }
}

public class Punctuation : Token
{
    public Punctuation() { }
    public Punctuation(string value) : base(value) { }
}

public class Sentence
{
    public List<Token> Tokens { get; set; } = [];

    public Sentence() { }

    public int GetWordCount()
    {
        return Tokens.Count(t => t is Word);
    }

    public bool IsQuestion()
    {
        return Tokens.Any(t => t is Punctuation && t.Value.Contains('?'));
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        foreach (var token in Tokens)
        {
            if (token is Word && sb.Length > 0) sb.Append(' ');
            sb.Append(token.Value);
        }
        return sb.ToString();
    }
}

public class Text
{
    public List<Sentence> Sentences { get; set; } = [];

    public Text() { }

    public override string ToString()
    {
        return string.Join(' ', Sentences);
    }
}
