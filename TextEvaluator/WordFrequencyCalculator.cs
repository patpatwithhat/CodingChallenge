using System.Text.RegularExpressions;

namespace TextEvaluator
{
    public class WordFrequencyCalculator
    {
        private string _filePath;
        public WordFrequencyCalculator(string filePath)
        {
            _filePath = filePath;
        }

        public IOrderedEnumerable<KeyValuePair<string, int>>? Calculate()
        {
            string text = string.Empty;
            try
            {
                text = File.ReadAllText(_filePath).ToLower();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

            MatchCollection matches = Regex.Matches(text, @"\b[\w']+\b"); // You don't have to know this.
            // Regex is a toughy. There are good tools for testing it: https://regexr.com/, https://regex-generator.olafneumann.org/, https://regex101.com/
            // But asking chatgpt made everything much easier here ;)

            var wordFrequency = new Dictionary<string, int>();

            foreach (Match match in matches)
            {
                string word = match.Value;
                if (wordFrequency.ContainsKey(word))
                {
                    wordFrequency[word]++;
                }
                else
                {
                    wordFrequency.Add(word, 1);
                }
            }

            var sortedWords = from entry in wordFrequency orderby entry.Value descending select entry;
            return sortedWords;
        }
    }
}
