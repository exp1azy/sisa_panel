using AngleSharp.Dom;

namespace Sisa.Panel.Extensions
{
    internal static class AngleSharpExtensions
    {
        extension (IElement element)
        {
            public IElement? GetSteamProfileElement()
            {
                return element.QuerySelector("a[href^='http://steamcommunity.com/profiles/']");
            }

            public IHtmlCollection<IElement> GetTableRows()
            {
                return element.QuerySelectorAll("tbody tr");
            }

            public IHtmlCollection<IElement> GetTableCells()
            {
                return element.QuerySelectorAll("td");
            }

            public string ExtractImgAltAttribute()
            {
                var flagImg = element.QuerySelector("img");

                if (flagImg != null)
                {
                    var alt = flagImg.GetAttribute("alt");
                    return alt ?? "Unknown";
                }

                return "Unknown";
            }

            public string ExtractLinkText()
            {
                var a = element.QuerySelector("a");
                return a?.TextContent ?? element.TextContent;
            }
        }
    }
}
