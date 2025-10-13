using MapOfCafesNearUniversity.DTO;
using MapOfCafesNearUniversity.Models;
using MapOfCafesNearUniversity.ServiceContracts;
using System.Text.Json;

namespace MapOfCafesNearUniversity.Services
{
    public class LeafletService : ILeafletService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LeafletService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<Cafe>> GetCafes()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var boundingBox = "50.36,30.39,50.53,30.65";
            var overpassQuery = $"[out:json];node[\"amenity\"=\"cafe\"]({boundingBox});out;";
            var response = await httpClient.GetAsync($"https://overpass-api.de/api/interpreter?data={Uri.EscapeDataString(overpassQuery)}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var overpassResponse = JsonSerializer.Deserialize<OverpassResponse>(jsonString);

                if (overpassResponse?.Elements != null)
                {

                    return overpassResponse.Elements
                        .Where(el => !string.IsNullOrEmpty(el.Tags?.Name))
                        .Select(el => new Cafe
                        {
                            Name = el.Tags.Name,
                            Latitude = el.Latitude,
                            Longitude = el.Longitude,
                            Address = $"{el.Tags.Street}, {el.Tags.HouseNumber}".Trim(new char[] { ' ', ',' }),
                            OpeningHours = el.Tags.OpeningHours,
                            Website = el.Tags.Website,
                            Phone = el.Tags.Phone
                        }).ToList();
                }
            }
            return new List<Cafe>();
        }
    }
}
