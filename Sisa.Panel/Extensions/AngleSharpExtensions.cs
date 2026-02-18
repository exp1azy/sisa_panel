using AngleSharp.Dom;
using Sisa.Panel.Parsers;

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

            public int ExtractUid()
            {
                var href = element.GetAttribute("href") ?? "";
                var uidMatch = ParserRegex.UidPattern.Match(href);

                if (uidMatch.Success && int.TryParse(uidMatch.Groups[1].Value, out int uid))
                    return uid;

                return 0;
            }

            public string ExtractAbsoluteImageUrl()
            {
                var imgElement = element.QuerySelector("img.img-circle");
                return imgElement?.GetAttribute("src") ?? string.Empty;
            }

            public string ExtractRelativeImageUrl()
            {
                var imageSrc = element.GetAttribute("src");
                if (!string.IsNullOrEmpty(imageSrc))
                {
                    if (imageSrc.StartsWith('/'))
                        return "https://panel.lan-game.com" + imageSrc;
                }

                return string.Empty;
            }
        }
    }
}
