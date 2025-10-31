using AngleSharp;
using Sisa.Panel.Extensions;
using Sisa.Panel.Models.AdminList;

namespace Sisa.Panel.Parsers
{
    internal class AdminListParser(IBrowsingContext context) : IParser<IReadOnlyList<AdminInfo>>
    {
        public async Task<IReadOnlyList<AdminInfo>> ParseAsync(string html)
        {
            var document = await context.OpenAsync(req => req.Content(html));
            var adminsList = new List<AdminInfo>();

            foreach (var block in document.QuerySelectorAll("div.smallstat.box"))
            {
                var adminInfo = new AdminInfo();

                var nameLink = block.GetSteamProfileElement();
                if (nameLink != null)
                {
                    adminInfo.AdminName = nameLink.TextContent;
                    adminInfo.SteamProfile = nameLink.GetAttribute("href") ?? string.Empty;
                }

                var statusElement = block.QuerySelector("span.label");
                if (statusElement != null)
                    adminInfo.Status = statusElement.TextContent;

                adminsList.Add(adminInfo);
            }

            return adminsList;
        }
    }
}
