using AngleSharp;
using AngleSharp.Dom;
using Sisa.Panel.Models;
using Sisa.Panel.Responses;

namespace Sisa.Panel.Parsers
{
    internal class AdminsListParser(IBrowsingContext context) : BaseParser<AdminsList>(context)
    {
        public override async Task<AdminsList> ParseAsync(string html)
        {
            var document = await context.OpenAsync(req => req.Content(html));

            var adminsList = new AdminsList
            {
                Admins = ParseAdminInfos(document)
            };

            return adminsList;
        }

        private static List<AdminInfo> ParseAdminInfos(IDocument document)
        {
            var adminsList = new List<AdminInfo>();
            var adminBlocks = document.QuerySelectorAll("div.smallstat.box");

            foreach (var block in adminBlocks)
            {
                var adminInfo = new AdminInfo();

                var nameLink = block.QuerySelector("a[href^='http://steamcommunity.com/profiles/']");
                if (nameLink != null)
                {
                    adminInfo.AdminName = nameLink.TextContent.Trim();
                    adminInfo.SteamProfile = nameLink.GetAttribute("href") ?? string.Empty;
                }

                var statusElement = block.QuerySelector("span.label");

                if (statusElement != null)
                    adminInfo.Status = statusElement.TextContent.Trim();

                adminsList.Add(adminInfo);
            }

            return adminsList;
        } 
    }
}
