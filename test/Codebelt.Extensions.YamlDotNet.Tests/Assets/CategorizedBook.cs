namespace Codebelt.Extensions.YamlDotNet.Assets
{
    public enum BookCategory
    {
        Fiction,
        NonFiction,
        Technical
    }

    public class CategorizedBook
    {
        public string Title { get; set; }

        public BookCategory Category { get; set; }
    }
}
