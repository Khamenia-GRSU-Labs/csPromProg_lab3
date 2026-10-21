using lab3.TextProcessing;

namespace lab3;

class Program
{
    public static void Main()
    {
        string rawText = File.ReadAllText(@"examples/1984_short.txt");

        Text text = TextParser.Parse(rawText);

        Console.WriteLine(HighLight("Original text:"));
        Console.WriteLine(text);
        PrintDelim();

        Console.WriteLine(HighLight("Sorted by sentence word count:"));
        foreach (var s in TextProcessor.OrderByWordCount(text)) Console.WriteLine($"{s.GetWordCount(),5} | {s}");
        PrintDelim();

        Console.WriteLine(HighLight("Sorted by sentence string len:"));
        foreach (var s in TextProcessor.OrderByLength(text)) Console.WriteLine($"{s.ToString().Length,5} | {s}");
        PrintDelim();

        int len = 5;
        var questionWords = TextProcessor.FindWordsInQuestions(text, len);
        Console.WriteLine(HighLight($"Unique words in questions of len {len}:"));
        Console.WriteLine(string.Join(", ", questionWords));
        PrintDelim();

        int sentenceIdx = 1;
        TextProcessor.ReplaceWordsInSentence(text, sentenceIdx, 4, "[REDACTED]");
        Console.WriteLine(HighLight($"Sentence of idx #{sentenceIdx} after replacing words of len {4}:"));
        Console.WriteLine(text.Sentences[sentenceIdx]);
        PrintDelim();

        int remLen = 6;
        TextProcessor.RemoveWordsByLengthAndConsonant(text, remLen);
        Console.WriteLine(HighLight($"Text after deleting words of len {remLen}, started with consonant:"));
        Console.WriteLine(text);
        PrintDelim();

        HashSet<string> stopWords = LoadStopWords(@"examples/stopwords_ru.txt", @"examples/stopwords_en.txt");
        TextProcessor.RemoveStopWords(text, stopWords);
        Console.WriteLine(HighLight("Text after deleting stop-words:"));
        Console.WriteLine(text);
        PrintDelim();

        string exportPath = "text_export.xml";
        XmlHelper.ExportToXml(text, exportPath);
        Console.WriteLine($"Object successfuly exported to {exportPath}");
    }

    private static void PrintDelim()
    {
        Console.WriteLine("\n" + new string('-', 20) + "\n");
    }
    private static string HighLight(in string str)
    {
        return $"\e[33m{str}\e[0m";
    }

    private static HashSet<string> LoadStopWords(params string[] filePaths)
    {
        var stopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var path in filePaths)
        {
            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
                stopWords.Add(line.Trim().ToLower());
        }
        return stopWords;
    }
}
