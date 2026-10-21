namespace lab3.TextProcessing;

static class TextProcessor
{
    public static IEnumerable<Sentence> OrderByWordCount(Text text)
    {
        return text.Sentences.OrderBy(s => s.GetWordCount());
    }

    public static IEnumerable<Sentence> OrderByLength(Text text)
    {
        return text.Sentences.OrderBy(s => s.ToString().Length);
    }

    public static HashSet<string> FindWordsInQuestions(Text text, int length)
    {
        HashSet<string> result = new(StringComparer.OrdinalIgnoreCase);

        foreach (var sentence in text.Sentences)
        {
            if (sentence.IsQuestion())
            {
                var words = sentence.Tokens.OfType<Word>().Where(w => w.Value.Length == length);
                foreach (var word in words)
                {
                    result.Add(word.Value);
                }
            }
        }
        return result;
    }

    public static void RemoveWordsByLengthAndConsonant(Text text, int length)
    {
        char[] vowels = { 'а', 'е', 'ё', 'и', 'о', 'у', 'ы', 'э', 'ю', 'я', 'a', 'e', 'i', 'o', 'u', 'y' };

        foreach (var sentence in text.Sentences)
        {
            sentence.Tokens.RemoveAll(t =>
                t is Word w &&
                w.Value.Length == length &&
                char.IsLetter(w.Value[0]) &&
                !vowels.Contains(char.ToLower(w.Value[0])));
        }
    }

    public static void ReplaceWordsInSentence(Text text, int sentenceIndex, int length, string replacement)
    {
        if (sentenceIndex < 0 || sentenceIndex >= text.Sentences.Count) return;

        var sentence = text.Sentences[sentenceIndex];
        foreach (var token in sentence.Tokens)
        {
            if (token is Word w && w.Value.Length == length)
            {
                w.Value = replacement;
            }
        }
    }

    public static void RemoveStopWords(Text text, HashSet<string> stopWords)
    {
        foreach (var sentence in text.Sentences)
        {
            sentence.Tokens.RemoveAll(t => t is Word w && stopWords.Contains(w.Value.ToLower()));
        }
    }

    public static SortedDictionary<string, (int TotalCount, SortedSet<int> Lines)> BuildConcordance(Text text)
    {
        var concordance = new SortedDictionary<string, (int TotalCount, SortedSet<int> Lines)>(StringComparer.Ordinal);

        int currentLine = 1;

        foreach (var sentence in text.Sentences)
        {
            foreach (var token in sentence.Tokens)
            {
                if (token is Word word)
                {
                    string key = word.Value.ToLower();

                    if (!concordance.TryGetValue(key, out var entry))
                    {
                        entry = (0, new SortedSet<int>());
                    }

                    entry.TotalCount++;
                    entry.Lines.Add(currentLine);
                    concordance[key] = entry;
                }
                else if (token is NewLine)
                {
                    currentLine++;
                }
            }
        }
        return concordance;
    }
}
