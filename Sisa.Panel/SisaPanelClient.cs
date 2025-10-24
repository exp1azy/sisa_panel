using AngleSharp;
using Sisa.Panel.Parsers;
using Sisa.Panel.Responses;

namespace Sisa.Panel
{
    public class SisaPanelClient
    {
        private readonly HttpClient _httpClient;
        private readonly IBrowsingContext _context;

        private readonly BanListParser _banListParser;
        private readonly ChatBanListParser _chatBanListParser;
        private readonly ChatLogParser _chatLogParser;
        private readonly AdminsListParser _adminsListParser;
        private readonly LiveStatusParser _liveStatusParser;
        private readonly ClansListParser _clansListParser;

        public SisaPanelClient(HttpClient? httpClient = null)
        {
            httpClient ??= new HttpClient();
            httpClient.BaseAddress = new Uri("https://panel.lan-game.com");
            _httpClient = httpClient;

            _context = BrowsingContext.New(Configuration.Default);

            _banListParser = new BanListParser(_context);
            _chatBanListParser = new ChatBanListParser(_context);
            _chatLogParser = new ChatLogParser(_context);
            _adminsListParser = new AdminsListParser(_context);
            _liveStatusParser = new LiveStatusParser(_context);
            _clansListParser = new ClansListParser(_context);
        }

        public async Task<BanList> GetBanListAsync(int page = 1, int view = 20, CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync($"/ban_list.php?view={view}&page={page}", cancellationToken);
            return await _banListParser.ParseAsync(html);
        }

        public async Task<ChatBanList> GetChatBanListAsync(int page = 1, int view = 20, CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync($"/chatban_list.php?view={view}&page={page}", cancellationToken);
            return await _chatBanListParser.ParseAsync(html);
        }

        public async Task<ChatLog> GetChatLogAsync(int view = 200, int page = 1, DateOnly date = default, CancellationToken cancellationToken = default)
        {
            if (date == default)
                date = DateOnly.FromDateTime(DateTime.Now);

            string dateStr = date.ToString("yyyy-MM-dd");

            var html = await _httpClient.GetStringAsync($"/chatlog.php?sid=0&view={view}&page={page}&date={dateStr}", cancellationToken);
            return await _chatLogParser.ParseAsync(html);
        }

        public async Task<AdminsList> GetAdminsListAsync(CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync("/admins_list.php", cancellationToken);
            return await _adminsListParser.ParseAsync(html);
        }

        public async Task<ServerLiveStatus> GetLiveStatusAsync(CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync("/live.php", cancellationToken);
            return await _liveStatusParser.ParseAsync(html);
        }

        public async Task<ClansList> GetClansListAsync(CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync("/clans.php", cancellationToken);
            return await _clansListParser.ParseAsync(html);
        }
    }
}