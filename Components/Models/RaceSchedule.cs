using System.Text.Json.Serialization;

public class RaceSchedule
{
    [JsonPropertyName("race_id")]
    public int RaceID {get; set;}
    [JsonPropertyName("series_id")]
    public int SeriesId {get; set;}
    [JsonPropertyName("race_season")]
    public int RaceSeason {get; set;}
    [JsonPropertyName("race_name")]
    public string RaceName {get; set;}
    [JsonPropertyName("race_date")]
    public DateTime RaceDate {get; set;}
    [JsonPropertyName("track_name")]
    public string TrackName {get; set;}
    [JsonPropertyName("scheduled_distance")]
    public double ScheduleDistance {get; set;}
    [JsonPropertyName("scheduled_laps")]
    public int ScheduledLaps {get; set;}
}