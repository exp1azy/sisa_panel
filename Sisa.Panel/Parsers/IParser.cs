namespace Sisa.Panel.Parsers
{
    internal interface IParser<T> where T : class
    {
        public Task<T> ParseAsync(string html);
    }
}
