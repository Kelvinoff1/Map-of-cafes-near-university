using MapOfCafesNearUniversity.DTO;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace MapOfCafesNearUniversity.Settings
{
    public class OverpassApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly OverpassApiSettings _settings;
        private readonly ILogger<OverpassApiClient> _logger;

        public OverpassApiClient(HttpClient httpClient, IOptions<OverpassApiSettings> settings, ILogger<OverpassApiClient> logger)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _logger = logger;
            _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
        }

        public async Task<OverpassResponse?> GetCafesFromApiAsync()
        {
            var query = string.Format(_settings.QueryTemplate, _settings.BoundingBox);
            var requestUri = $"api/interpreter?data={Uri.EscapeDataString(query)}";

            try
            {
                var response = await _httpClient.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<OverpassResponse>(jsonString);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Помилка під час запиту до Overpass API. URI: {RequestUri}", requestUri);
                return null;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Помилка десеріалізації відповіді від Overpass API.");
                return null;
            }
        }
    }
}
