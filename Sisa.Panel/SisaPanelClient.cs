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
    /// <summary>
    /// Клиент для <c>https://panel.lan-game.com</c>.
    /// </summary>
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
        private readonly ContestParser _contestParticipantsParser;
        private readonly ContestHistoryParser _contestHistoryParser;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="httpClient">Клиент HTTP (необязательно).</param>
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
            _contestParticipantsParser = new ContestParser(_context);
            _contestHistoryParser = new ContestHistoryParser(_context);
        }

        /// <summary>
        /// Страница конкурса.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список участников конкурса, доступный только для чтения.</returns>
        public async Task<ContestInfo> GetContestAsync(CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync("/free.php", cancellationToken);
            return await _contestParticipantsParser.ParseAsync(html);
        }

        /// <summary>
        /// История конкурса.
        /// </summary>
        /// <param name="page">Страница.</param>
        /// <param name="view">Отображаемое количество записей.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список проводимых конкурсов, доступный только для чтения.</returns>
        public async Task<IReadOnlyList<ContestHistoryEntry>> GetContestHistoryAsync(int page = 1, int view = 20, CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync($"/free.php?sid=0&action=history&view={view}&page={page}", cancellationToken);
            return await _contestHistoryParser.ParseAsync(html);
        }

        /// <summary>
        /// Список банов.
        /// </summary>
        /// <param name="page">Страница.</param>
        /// <param name="view">Отображаемое количество записей.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список банов с общей статистикой.</returns>
        public async Task<BanList> GetBansAsync(int page = 1, int view = 20, CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync($"/ban_list.php?view={view}&page={page}", cancellationToken);
            return await _banListParser.ParseAsync(html);
        }

        /// <summary>
        /// Список банов чата.
        /// </summary>
        /// <param name="page">Страница.</param>
        /// <param name="view">Отображаемое количество записей.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список банов с общей статистикой.</returns>
        public async Task<ChatBanList> GetChatBansAsync(int page = 1, int view = 20, CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync($"/chatban_list.php?view={view}&page={page}", cancellationToken);
            return await _chatBanListParser.ParseAsync(html);
        }

        /// <summary>
        /// Журнал чата.
        /// </summary>
        /// <param name="page">Страница.</param>
        /// <param name="view">Отображаемое количество записей.</param>
        /// <param name="date">Дата.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список сообщений чата, доступный только для чтения.</returns>
        public async Task<IReadOnlyList<ChatLogEntry>> GetChatLogAsync(int page = 1, int view = 200,  DateOnly date = default, CancellationToken cancellationToken = default)
        {
            if (date == default)
                date = DateOnly.FromDateTime(DateTime.Now);

            string dateStr = date.ToString("yyyy-MM-dd");

            var html = await _httpClient.GetStringAsync($"/chatlog.php?sid=0&view={view}&page={page}&date={dateStr}", cancellationToken);
            return await _chatLogParser.ParseAsync(html);
        }

        /// <summary>
        /// Список админов.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список админов, доступный только для чтения.</returns>
        public async Task<IReadOnlyList<AdminInfo>> GetAdminsAsync(CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync("/admins_list.php", cancellationToken);
            return await _adminListParser.ParseAsync(html);
        }

        /// <summary>
        /// Статус сервера.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Подробная информация о статусе сервера.</returns>
        public async Task<ServerLiveStatus> GetLiveStatusAsync(CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync("/live.php", cancellationToken);
            return await _liveStatusParser.ParseAsync(html);
        }

        /// <summary>
        /// Список кланов.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список кланов, доступный только для чтения.</returns>
        public async Task<IReadOnlyList<ClanEntry>> GetClansAsync(CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync("/clans.php", cancellationToken);
            return await _clanListParser.ParseAsync(html);
        }

        /// <summary>
        /// Информация о клане.
        /// </summary>
        /// <param name="id">Идентификатор клана.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Подробная информация о клане.</returns>
        public async Task<ClanInfo> GetClanAsync(int id, CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync($"/clans.php?action=clan&sid=0&id={id}", cancellationToken);
            return await _clanInfoParser.ParseAsync(html);
        }

        /// <summary>
        /// Статистика по игрокам.
        /// </summary>
        /// <param name="page">Страница.</param>
        /// <param name="view">Отображаемое количество записей.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список игроков, доступный только для чтения.</returns>
        public async Task<IReadOnlyList<PlayerStatEntry>> GetPlayerStatsAsync(int page = 1, int view = 50, CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync($"/stat.php?sid=0&view={view}&page={page}", cancellationToken);
            return await _playerStatsParser.ParseAsync(html);
        }

        /// <summary>
        /// Статистика по оружиям.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Подробная информация о статистике по оружиям.</returns>
        public async Task<WeaponStats> GetWeaponStatsAsync(CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync("/stat.php?sid=0&action=weapons", cancellationToken);
            return await _generalWeaponStatsParser.ParseAsync(html);
        }

        /// <summary>
        /// Статистика по указанному оружию.
        /// </summary>
        /// <param name="wid">Идентификатор оружия.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список статистики игроков по указанному оружию, доступный только для чтения.</returns>
        public async Task<IReadOnlyList<WeaponStatsEntry>> GetWeaponStatsAsync(int wid, CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync($"/stat.php?sid=0&action=weapon&wid={wid}", cancellationToken);
            return await _weaponStatsParser.ParseAsync(html);
        }

        /// <summary>
        /// Статистика по лучшим игрокам за людей.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Подробная информация о статистике по лучшим игрокам за людей.</returns>
        public async Task<HumanTopPlayersStats> GetHumanTopPlayersAsync(CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync("/stat.php?sid=0&action=hmcl", cancellationToken);
            return await _humanBestPlayersParser.ParseAsync(html);
        }

        /// <summary>
        /// Статистика по лучшим игрокам за зомби.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Подробная информация о статистике по лучшим игрокам за зомби.</returns>
        public async Task<ZombieTopPlayersStats> GetZombieTopPlayersAsync(CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync("/stat.php?sid=0&action=zmcl", cancellationToken);
            return await _zombieBestPlayersParser.ParseAsync(html);
        }

        /// <summary>
        /// Статистика по картам.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список карт, доступный только для чтения.</returns>
        public async Task<IReadOnlyList<MapEntry>> GetMapStatsAsync(CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync("/stat.php?sid=0&action=maps", cancellationToken);
            return await _mapStatsParser.ParseAsync(html);
        }

        /// <summary>
        /// Информация об игроке.
        /// </summary>
        /// <param name="uid">Идентификатор игрока.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Подробная информация об игроке.</returns>
        public async Task<PlayerInfo> GetPlayerAsync(int uid, CancellationToken cancellationToken = default)
        {
            var html = await _httpClient.GetStringAsync($"/stat.php?sid=0&action=player&uid={uid}", cancellationToken);
            return await _playerInfoParser.ParseAsync(html);
        }

        /// <summary>
        /// Поиск.
        /// </summary>
        /// <param name="query">Запрос.</param>
        /// <param name="searchByName">Осуществить запрос по имени (true) или по STEAM_ID (false).</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список записей по запросу, доступный только для чтения.</returns>
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

        /// <summary>
        /// Освобождение ресурсов.
        /// </summary>
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