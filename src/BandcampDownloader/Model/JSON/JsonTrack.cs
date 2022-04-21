using Newtonsoft.Json;

namespace BandcampDownloader
{
    internal class JsonTrack
    {
        [JsonProperty("duration")]
        public double Duration { get; set; }

        [JsonProperty("file")]
        public JsonMp3File File { get; set; }

        [JsonProperty("lyrics")]
        public string Lyrics { get; set; }

        [JsonProperty("track_num")]
        public int Number { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("license_type")]
        public int LicenseType { get; set; }

        public Track ToTrack(Album album)
        {
            var licenseLookup = new System.Collections.Generic.Dictionary<int, string>
            {
                [1] = "All Rights Reserved ©️",
                [2] = "CC BY-NC-ND 3.0",
                [3] = "CC BY-NC-SA 3.0",
                [4] = "CC BY-NC 3.0",
                [5] = "CC BY-ND 3.0",
                [6] = "CC BY 3.0",
                [8] = "CC BY-SA 3.0"
            };

            var mp3Url = (File.Url.StartsWith("//") ? "http:" : "") + File.Url; // "//example.com" Uri lacks protocol
            var number = Number == 0 ? 1 : Number; // For bandcamp track pages, Number will be 0. Set 1 instead

            var licenseString = "";
            try
            {
                licenseString = licenseLookup[LicenseType];
            }
            catch
            {
                licenseString = string.Format("Unknown License Type {0}", LicenseType);
            }

            return new Track(album, Duration, Lyrics, mp3Url, number, Title, licenseString);
        }
    }
}