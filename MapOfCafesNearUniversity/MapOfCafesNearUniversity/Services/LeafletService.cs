using MapOfCafesNearUniversity.DTO;
using MapOfCafesNearUniversity.Models;
using MapOfCafesNearUniversity.ServiceContracts;
using MapOfCafesNearUniversity.Settings;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using System.Text.Json;

namespace MapOfCafesNearUniversity.Services
{
    public class LeafletService : ILeafletService
    {
        private readonly OverpassApiClient _apiClient;
        private readonly IMemoryCache _cache;
        private readonly ILogger<LeafletService> _logger;
        private const string CafesCacheKey = "ListOfCafes";

        public LeafletService(OverpassApiClient apiClient, IMemoryCache cache, ILogger<LeafletService> logger)
        {
            _apiClient = apiClient;
            _cache = cache;
            _logger = logger;
        }

        public async Task<List<Cafe>> GetCafes()
        {
            if (_cache.TryGetValue(CafesCacheKey, out List<Cafe> cafes))
            {
                _logger.LogInformation("Дані про кафе отримано з кешу.");
                return cafes;
            }

            _logger.LogInformation("Кеш порожній. Запитуємо дані з Overpass API.");
            var overpassResponse = await _apiClient.GetCafesFromApiAsync();

            if (overpassResponse?.Elements == null)
            {
                return new List<Cafe>();
            }

            cafes = MapToCafeModel(overpassResponse.Elements);

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                .SetAbsoluteExpiration(TimeSpan.FromHours(2));

            _cache.Set(CafesCacheKey, cafes, cacheEntryOptions);

            return cafes;
        }

        private List<Cafe> MapToCafeModel(List<Element> elements)
        {
            return elements
                .Where(el => !string.IsNullOrEmpty(el.Tags?.Name))
                .Select(el =>
                {
                    var popupBuilder = new StringBuilder();
                    popupBuilder.Append($"<b>{el.Tags.Name}</b>");

                    string address = $"{el.Tags.Street}, {el.Tags.HouseNumber}".Trim(new char[] { ' ', ',' });
                    if (!string.IsNullOrWhiteSpace(address)) popupBuilder.Append($"<br>📍 {address}");

                    string openingHours = el.Tags.OpeningHours;
                    if (!string.IsNullOrWhiteSpace(openingHours)) popupBuilder.Append($"<br>🕒 {openingHours}");

                    string phone = el.Tags.Phone;
                    if (!string.IsNullOrWhiteSpace(phone)) popupBuilder.Append($"<br>📞 {phone}");

                    string website = el.Tags.Website;
                    if (!string.IsNullOrWhiteSpace(website)) popupBuilder.Append($"<br>🌐 <a href=\"{website}\" target=\"_blank\">Веб-сайт</a>");

                    return new Cafe
                    {
                        Name = el.Tags.Name,
                        Latitude = el.Latitude,
                        Longitude = el.Longitude,
                        PopupContent = popupBuilder.ToString(),
                        Address = string.IsNullOrWhiteSpace(address) ? "Адреса невідома" : address,
                        OpeningHours = string.IsNullOrWhiteSpace(openingHours) ? "Години роботи невідомі" : openingHours,
                        Website = website,
                        Phone = phone
                    };
                }).ToList();
        }
    }
}