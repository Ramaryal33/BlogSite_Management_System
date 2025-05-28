using System.Text.RegularExpressions;

namespace MurkyPluse.HelperMurky
{
    public  static class RemoveHtmlTagHelper
    {
        public static string RemoveHtmlTags(string input)
        {
            return Regex.Replace(input, "<.*?>|&.*?;", string.Empty); // This removes any HTML tags.
        }
    }
}
