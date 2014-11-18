namespace Teller.Web.Infrastructure.UrlGenerators
{
    using System;
    using System.Text;

    public class UrlGenerator : IUrlGenerator
    {
        public string GenerateUrlId(int id, string title)
        {
            return string.Format("{0}-{1}", this.ToUrl(title.ToLower()), id);
        }

        private string ToUrl(string uglyString)
        {
            ////var url = string.Join("-", uglyString.ToLower().Substring(0, Math.Min(20, uglyString.Length)).Split(new char[] { ' ', '.', '-', ',', ':', ';', '?', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '+', '=', '[', ']', '{', '}', '<', '>', '\\', '/', '|', '\'', '"' }, StringSplitOptions.RemoveEmptyEntries));

            var resultString = new StringBuilder(uglyString.Length);
            bool isLastCharacterDash = false;

            foreach (var character in uglyString)
            {
                if (char.IsLetterOrDigit(character))
                {
                    resultString.Append(character);
                    isLastCharacterDash = false;
                }
                else if (!isLastCharacterDash)
                {
                    resultString.Append('-');
                    isLastCharacterDash = true;
                }
            }

            return resultString.ToString().Trim('-');
        }
    }
}