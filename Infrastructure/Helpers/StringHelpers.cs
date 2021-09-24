namespace Infrastructure.Helpers
{
    public static class StringHelpers
    {
        public static string RemoveNumbersFromMultiWord(this string input)
        {
            string output = "";
            var words = input.Split(' ');

            int junk = 0;
            foreach (var word in words)
            {
                if (!int.TryParse(word, out junk))
                    output += word + " ";
            }

            return output;
        }
    }
}
