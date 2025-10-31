using AngleSharp.Dom;

namespace Sisa.Panel.Extensions
{
    internal static class IElementExtensions
    {
        public static IElement? GetSteamProfileElement(this IElement element)
        {
            return element.QuerySelector("a[href^='http://steamcommunity.com/profiles/']");
        }

        public static string GetTextContent(this IElement element)
        {
            return element.TextContent.Trim();
        }

        public static IHtmlCollection<IElement> GetTableRows(this IElement element)
        {
            return element.QuerySelectorAll("tbody tr");
        }

        public static IHtmlCollection<IElement> GetTableCells(this IElement element)
        {
            return element.QuerySelectorAll("td");
        }
    }
}
