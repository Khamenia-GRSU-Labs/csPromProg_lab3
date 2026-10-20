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
        var matches = Regex.Matches(sentenceStr, @"([\p{L}\p{N}]+)|([^\s\p{L}\p{N}])");

        foreach (Match match in matches)
        {
            if (string.IsNullOrEmpty(match.Value)) continue;

            if (Regex.IsMatch(match.Value, @"^[\p{L}\p{N}]+$"))
            {
                sentence.Tokens.Add(new Word(match.Value));
            }
            else
            {
                sentence.Tokens.Add(new Punctuation(match.Value));
            }
        }
        return sentence;
    }
}
