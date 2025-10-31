using AngleSharp;
using Sisa.Panel.Models.AdminList;
using Sisa.Panel.Models.Chatlog;
using Sisa.Panel.Models.Clans;
using Sisa.Panel.Models.Stat;
using Sisa.Panel.Parsers;
using Sisa.Panel.Responses;

namespace Sisa.Panel
{
    public class SisaPanelClient
    {
        private readonly HttpClient _httpClient;

        private readonly BanListParser _banListParser;
        private readonly ChatBanListParser _chatBanListParser;
        private readonly ChatLogParser _chatLogParser;
        private readonly AdminListParser _adminListParser;
        private readonly LiveStatusParser _liveStatusParser;
        private readonly ClanListParser _clanListParser;
        private readonly ClanInfoParser _clanInfoParser;
        private readonly PlayerStatsParser _playerStatsParser;
        private readonly WeaponStatsParser _weaponStatsParser;
        private readonly HumanTopPlayersParser _humanBestPlayersParser;
        private readonly ZombieTopPlayersParser _zombieBestPlayersParser;
        private readonly MapStatsParser _mapStatsParser;
        private readonly PlayerInfoParser _playerInfoParser;

        public SisaPanelClient(HttpClient? httpClient = null)
        {
            httpClient ??= new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.BaseAddress = new Uri("https://panel.lan-game.com");
            _httpClient = httpClient;

            var context = BrowsingContext.New(Configuration.Default);

            _banListParser = new BanListParser(context);
            _chatBanListParser = new ChatBanListParser(context);
            _chatLogParser = new ChatLogParser(context);
            _adminListParser = new AdminListParser(context);
            _liveStatusParser = new LiveStatusParser(context);
            _clanListParser = new ClanListParser(context);
            _clanInfoParser = new ClanInfoParser(context);
            _playerStatsParser = new PlayerStatsParser(context);
            _weaponStatsParser = new WeaponStatsParser(context);
            _humanBestPlayersParser = new HumanTopPlayersParser(context);
            _zombieBestPlayersParser = new ZombieTopPlayersParser(context);
            _mapStatsParser = new MapStatsParser(context);
            _playerInfoParser = new PlayerInfoParser(context);
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

        public async Task<IReadOnlyList<ChatLogEntry>> GetChatLogAsync(int view = 200, int page = 1, DateOnly date = default, CancellationToken cancellationToken = default)
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

        public async Task<IReadOnlyList<PlayerStatEntry>> GetPlayerStatsAsync(int view = 50, int page = 1, CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync($"/stat.php?sid=0&view={view}&page={page}", cancellationToken);
            return await _playerStatsParser.ParseAsync(html);
        }

        public async Task<WeaponStats> GetWeaponStatsAsync(CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync("/stat.php?sid=0&action=weapons", cancellationToken);
            return await _weaponStatsParser.ParseAsync(html);
        }

        public async Task<HumanTopPlayersStat> GetHumanTopPlayersAsync(CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync("/stat.php?sid=0&action=hmcl", cancellationToken);
            return await _humanBestPlayersParser.ParseAsync(html);
        }

        public async Task<ZombieTopPlayersStat> GetZombieTopPlayersAsync(CancellationToken cancellationToken = default)
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
    }
}