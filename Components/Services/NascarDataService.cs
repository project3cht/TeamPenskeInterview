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

    // Method to fetch the race schedules for a specific year and series
    public async Task<List<RaceSchedule>?> GetRaceSchedulesAsync(int year, string series)
    {
        try
        {
            string apiUrl = $"https://cf.nascar.com/cacher/{year}/race_list_basic.json";
            _logger.LogInformation($"Fetching race schedule for year: {year} from {apiUrl}");

            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var parentObject = JsonSerializer.Deserialize<ParentObject>(json);
                if (parentObject != null && parentObject.Series.ContainsKey(series))
                {
                    return parentObject.Series[series].RaceSchedule;
                }
                else
                {
                    _logger.LogWarning($"No races found for series: {series}");
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning($"No data found for year {year}");
                return null;  // No data for this year
            }
            else
            {
                _logger.LogError($"Failed to fetch race schedule for {year}. Status Code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching race schedule");
        }

        return null;
    }

    // Method to fetch available years by testing which years have data
    public async Task<List<int>> GetAvailableYearsAsync(string series, int startYear = 2020, int endYear = 2025)
    {
        var availableYears = new List<int>();

        for (int year = startYear; year <= endYear; year++)
        {
            var schedules = await GetRaceSchedulesAsync(year, series);
            if (schedules != null && schedules.Count > 0)
            {
                availableYears.Add(year);
                _logger.LogInformation($"Found schedules for year: {year}");
            }
            else
            {
                _logger.LogWarning($"No schedules found for year: {year}");
            }
        }

        return availableYears;
    }
}
