using AngleSharp;
using Sisa.Panel.Models.AdminList;
using Sisa.Panel.Models.Chatlog;
using Sisa.Panel.Models.Clans;
using Sisa.Panel.Models.Contest;
using Sisa.Panel.Models.Stat;
using Sisa.Panel.Parsers;
using Sisa.Panel.Responses;

namespace Sisa.Panel
{
    public class SisaPanelClient : IDisposable
    {
        private bool _disposed;

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
        private readonly GeneralWeaponStatsParser _generalWeaponStatsParser;
        private readonly WeaponStatsParser _weaponStatsParser;
        private readonly HumanTopPlayersParser _humanBestPlayersParser;
        private readonly ZombieTopPlayersParser _zombieBestPlayersParser;
        private readonly MapStatsParser _mapStatsParser;
        private readonly PlayerInfoParser _playerInfoParser;
        private readonly PlayerSearchParser _playerSearchParser;
        private readonly ContestParticipantsParser _contestParticipantsParser;
        private readonly ContestHistoryParser _contestHistoryParser;

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
            _generalWeaponStatsParser = new GeneralWeaponStatsParser(_context);
            _weaponStatsParser = new WeaponStatsParser(_context);
            _humanBestPlayersParser = new HumanTopPlayersParser(_context);
            _zombieBestPlayersParser = new ZombieTopPlayersParser(_context);
            _mapStatsParser = new MapStatsParser(_context);
            _playerInfoParser = new PlayerInfoParser(_context);
            _playerSearchParser = new PlayerSearchParser(_context);
            _contestParticipantsParser = new ContestParticipantsParser(_context);
            _contestHistoryParser = new ContestHistoryParser(_context);
        }

        public async Task<IReadOnlyList<ContestParticipant>> GetContestParticipantsAsync(CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync("/free.php", cancellationToken);
            return await _contestParticipantsParser.ParseAsync(html);
        }

        public async Task<IReadOnlyList<ContestHistoryEntry>> GetContestHistoryAsync(int page = 1, int view = 20, CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync($"/free.php?sid=0&action=history&view={view}&page={page}", cancellationToken);
            return await _contestHistoryParser.ParseAsync(html);
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

        public async Task<IReadOnlyList<ChatLogEntry>> GetChatLogAsync(int page = 1, int view = 200,  DateOnly date = default, CancellationToken cancellationToken = default)
        {
            if (date == default)
                date = DateOnly.FromDateTime(DateTime.Now);

            string dateStr = date.ToString("yyyy-MM-dd");

            var html = await _httpClient.GetStringAsync($"/chatlog.php?sid=0&view={view}&page={page}&date={dateStr}", cancellationToken);
            return await _chatLogParser.ParseAsync(html);
        }

        public async Task<IReadOnlyList<AdminInfo>> GetAdminListAsync(CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync("/admins_list.php", cancellationToken);
            return await _adminListParser.ParseAsync(html);
        }

        public async Task<ServerLiveStatus> GetLiveStatusAsync(CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync("/live.php", cancellationToken);
            return await _liveStatusParser.ParseAsync(html);
        }

        public async Task<IReadOnlyList<ClanEntry>> GetClanListAsync(CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync("/clans.php", cancellationToken);
            return await _clanListParser.ParseAsync(html);
        }

        public async Task<ClanInfo> GetClanInfoAsync(int id, CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync($"/clans.php?action=clan&sid=0&id={id}", cancellationToken);
            return await _clanInfoParser.ParseAsync(html);
        }

        public async Task<IReadOnlyList<PlayerStatEntry>> GetPlayerStatsAsync(int page = 1, int view = 50, CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync($"/stat.php?sid=0&view={view}&page={page}", cancellationToken);
            return await _playerStatsParser.ParseAsync(html);
        }

        public async Task<WeaponStats> GetWeaponStatsAsync(CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync("/stat.php?sid=0&action=weapons", cancellationToken);
            return await _generalWeaponStatsParser.ParseAsync(html);
        }

        public async Task<IReadOnlyList<WeaponStatsEntry>> GetWeaponStatsAsync(int wid, CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync($"/stat.php?sid=0&action=weapon&wid={wid}", cancellationToken);
            return await _weaponStatsParser.ParseAsync(html);
        }

        public async Task<HumanTopPlayersStats> GetHumanTopPlayersAsync(CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync("/stat.php?sid=0&action=hmcl", cancellationToken);
            return await _humanBestPlayersParser.ParseAsync(html);
        }

        public async Task<ZombieTopPlayersStats> GetZombieTopPlayersAsync(CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync("/stat.php?sid=0&action=zmcl", cancellationToken);
            return await _zombieBestPlayersParser.ParseAsync(html);
        }

        public async Task<IReadOnlyList<MapEntry>> GetMapStatsAsync(CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync("/stat.php?sid=0&action=maps", cancellationToken);
            return await _mapStatsParser.ParseAsync(html);
        }

        public async Task<PlayerInfo> GetPlayerInfo(int uid, CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync($"/stat.php?sid=0&action=player&uid={uid}", cancellationToken);
            return await _playerInfoParser.ParseAsync(html);
        }

        public async Task<IReadOnlyList<PlayerSearchEntry>> SearchAsync(string query, bool searchByName = true, CancellationToken cancellationToken = default)
        {
            var parameters = new Dictionary<string, string>
            {
                ["radiosearch"] = searchByName ? "pnick" : "psteam",
                ["action"] = "insert",
                ["mysearch"] = query,
                ["submit"] = "Поиск"
            };

            var formData = new FormUrlEncodedContent(parameters);
            var response = await _httpClient.PostAsync("/stat.php?sid=0&action=search", formData, cancellationToken);
            var html = await response.Content.ReadAsStringAsync(cancellationToken);

            return await _playerSearchParser.ParseAsync(html);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _httpClient.Dispose();
                _context.Dispose();
                _disposed = true;
            }

            GC.SuppressFinalize(this);
        }
    }
}