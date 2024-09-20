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

    public async Task<List<RaceSchedule>> GetRaceSchedulesAsync(string year, string series)
    {
        try
        {
            var response = await _httpClient.GetAsync($"https://cf.nascar.com/cacher/{year}/race_list_basic.json");
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var raceSchedules = JsonSerializer.Deserialize<List<RaceSchedule>>(jsonResponse);

            return raceSchedules;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch race schedules for year {Year} and series {Series}", year, series);
            throw;
        }
    }
}