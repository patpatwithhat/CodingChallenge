
using TextEvaluator;

var filePath = @"Kafka.txt";

var calculator = new WordFrequencyCalculator(filePath);
var sortedWords = calculator.Calculate();

if (sortedWords == null)
    return;

foreach (var word in sortedWords)
{
    Console.WriteLine($"{word.Key}: {word.Value}");
}