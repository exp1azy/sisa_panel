namespace Sisa.Panel.Parsers
{
    internal interface IParsable<T> where T : class
    {
        public Task<T> ParseAsync(string html);
    }
}
