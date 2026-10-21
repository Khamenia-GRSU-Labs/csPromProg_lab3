using System.Text.RegularExpressions;

namespace lab3.TextProcessing;

static class TextParser
{
    public static Text Parse(string rawText)
    {
        var text = new Text();
        if (string.IsNullOrWhiteSpace(rawText)) return text;

        var sentenceMatches = Regex.Matches(rawText, @"[^.!?]+[.!?]+");
        if (sentenceMatches.Count == 0)
        {
            text.Sentences.Add(ParseSentence(rawText));
            return text;
        }
        foreach (Match match in sentenceMatches)
        {
            text.Sentences.Add(ParseSentence(match.Value));
        }
        return text;
    }

    private static Sentence ParseSentence(string sentenceStr)
    {
        var sentence = new Sentence();
        var matches = Regex.Matches(sentenceStr, @"([\p{L}\p{N}]+)|(\r\n|\r|\n)|([^\s\p{C}\p{L}\p{N}])");

        foreach (Match match in matches)
        {
            if (match.Groups[1].Success)
            {
                sentence.Tokens.Add(new Word(match.Groups[1].Value));
            }
            else if (match.Groups[2].Success)
            {
                sentence.Tokens.Add(new NewLine(match.Groups[2].Value));
            }
            else if (match.Groups[3].Success)
            {
                sentence.Tokens.Add(new Punctuation(match.Groups[3].Value));
            }
        }
        return sentence;
    }
}
