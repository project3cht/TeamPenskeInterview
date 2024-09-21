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
    public string? RaceName {get; set;}
    [JsonPropertyName("race_date")]
    public DateTime RaceDate {get; set;}
    [JsonPropertyName("track_name")]
    public string? TrackName {get; set;}
    [JsonPropertyName("scheduled_distance")]
    public double ScheduleDistance {get; set;}
    [JsonPropertyName("scheduled_laps")]
    public int ScheduledLaps {get; set;}
}
public class RaceSeries
{
    public List<RaceSchedule> RaceSchedule {get; set;} = new List<RaceSchedule>();}
public class ParentObject
{
    [JsonPropertyName("series_1")]
    public List<RaceSchedule> CupSeries {get; set;} = new List<RaceSchedule>();

    [JsonPropertyName("series_2")]
    public List<RaceSchedule> XfinitySeries {get; set;} = new List<RaceSchedule>();

    [JsonPropertyName("series_3")]
    public List<RaceSchedule> TruckSeries {get; set;} = new List<RaceSchedule>();
}