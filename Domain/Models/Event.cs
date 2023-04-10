using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class Event
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        [JsonPropertyName("start")]
        public Time Start { get; set; }

        [JsonPropertyName("end")]
        public Time End { get; set; }

        [JsonPropertyName("location")]
        public Location Location { get; set; }

        [JsonPropertyName("showAs")]
        public string ShowAs { get; set; }
    }

    public class Time
    {
        [JsonPropertyName("datetime")]
        public string DateTime { get; set; }

        [JsonPropertyName("timezone")]
        public string TimeZone { get; set; }
    }

    public class Location
    {
        [JsonPropertyName("address")]
        public PhysicalAddress Address  { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }
    }

    public class PhysicalAddress
    {
        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("countryOrRegion")]
        public string CountryOrRegion { get; set; }

        [JsonPropertyName("postalCode")]
        public string PostalCode { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("street")]
        public string Street { get; set; }
    }
}
