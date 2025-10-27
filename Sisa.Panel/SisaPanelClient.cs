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
        private readonly AdminListParser _adminListParser;
        private readonly LiveStatusParser _liveStatusParser;
        private readonly ClanListParser _clanListParser;
        private readonly ClanInfoParser _clanInfoParser;
        private readonly PlayerStatsParser _playerStatsParser;
        private readonly WeaponStatsParser _weaponStatsParser;

        public SisaPanelClient(HttpClient? httpClient = null)
        {
            httpClient ??= new HttpClient();
            httpClient.BaseAddress = new Uri("https://panel.lan-game.com");
            _httpClient = httpClient;

            _context = BrowsingContext.New(Configuration.Default);

            _banListParser = new BanListParser(_context);
            _chatBanListParser = new ChatBanListParser(_context);
            _chatLogParser = new ChatLogParser(_context);
            _adminListParser = new AdminListParser(_context);
            _liveStatusParser = new LiveStatusParser(_context);
            _clanListParser = new ClanListParser(_context);
            _clanInfoParser = new ClanInfoParser(_context);
            _playerStatsParser = new PlayerStatsParser(_context);
            _weaponStatsParser = new WeaponStatsParser(_context);
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

        public async Task<AdminList> GetAdminListAsync(CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync("/admins_list.php", cancellationToken);
            return await _adminListParser.ParseAsync(html);
        }

        public async Task<ServerLiveStatus> GetLiveStatusAsync(CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync("/live.php", cancellationToken);
            return await _liveStatusParser.ParseAsync(html);
        }

        public async Task<ClanList> GetClanListAsync(CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync("/clans.php", cancellationToken);
            return await _clanListParser.ParseAsync(html);
        }

        public async Task<ClanInfo> GetClanInfoAsync(int id, CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync($"/clans.php?action=clan&sid=0&id={id}", cancellationToken);
            return await _clanInfoParser.ParseAsync(html);
        }

        public async Task<PlayerStats> GetPlayerStatsAsync(int view = 50, int page = 1, CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync($"/stat.php?sid=0&view={view}&page={page}", cancellationToken);
            return await _playerStatsParser.ParseAsync(html);
        }

        public async Task<WeaponStats> GetWeaponStatsAsync(CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync("/stat.php?sid=0&action=weapons", cancellationToken);
            return await _weaponStatsParser.ParseAsync(html);
        }
    }
}