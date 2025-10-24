using AngleSharp;

namespace Sisa.Panel.Parsers
{
    internal abstract class BaseParser<T>(IBrowsingContext context)
    {
        protected IBrowsingContext Context => context;

        public abstract Task<T> ParseAsync(string html);
    }
}
