using System.Text.Json.Serialization;

namespace MapOfCafesNearUniversity.DTO
{
    public class OverpassResponse
    {
        [JsonPropertyName("elements")]
        public List<Element>? Elements { get; set; }
    }

    public class Element
    {
        [JsonPropertyName("lat")]
        public double Latitude { get; set; }

        [JsonPropertyName("lon")]
        public double Longitude { get; set; }

        [JsonPropertyName("tags")]
        public Tags? Tags { get; set; }
    }

    public class Tags
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("addr:street")]
        public string? Street { get; set; }

        [JsonPropertyName("addr:housenumber")]
        public string? HouseNumber { get; set; }

        [JsonPropertyName("opening_hours")]
        public string? OpeningHours { get; set; }

        [JsonPropertyName("website")]
        public string? Website { get; set; }

        [JsonPropertyName("phone")]
        public string? Phone { get; set; }
    }
}
