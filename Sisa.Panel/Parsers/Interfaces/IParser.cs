namespace Sisa.Panel.Parsers.Interfaces
{
    internal interface IParser<T> where T : class
    {
        public Task<T> ParseAsync(string html);
    }
}
