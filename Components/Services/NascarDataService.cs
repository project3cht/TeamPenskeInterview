using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;

public class NascarDataService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<NascarDataService> _logger;

    public NascarDataService(HttpClient httpClient, ILogger<NascarDataService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<RaceSchedule>> GetRaceSchedulesAsync(int year, string series)
    {
        try
        {
            string apiUrl = $"https://cf.nascar.com/cacher/{year}/race_list_basic.json";
            _logger.LogInformation($"Fetching race schedule for year: {year} from {apiUrl}");

            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var raceSeriesDict = JsonSerializer.Deserialize<Dictionary<string, List<RaceSchedule>>>(json);
                if (raceSeriesDict != null && raceSeriesDict.ContainsKey(series))
                {
                    return raceSeriesDict[series];
                }
                else
                {
                    _logger.LogWarning($"No races found for series: {series}");
                }
            }
            else
            {
                _logger.LogError($"Failed to fetch race schedule for {year}. Status Code: {response.StatusCode}");
                return new List<RaceSchedule>();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching race schedule");
            return new List<RaceSchedule>();
        }
        return new List<RaceSchedule>();
    }
}