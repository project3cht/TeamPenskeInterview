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
                _logger.LogDebug($"Received JSON: {json}");

                var parentObject = JsonSerializer.Deserialize<ParentObject>(json);

                if (parentObject != null)
                {
                    List<RaceSchedule> raceSchedules = series switch
                    {
                        "series_1" => parentObject.CupSeries ?? new List<RaceSchedule>(),
                        "series_2" => parentObject.XfinitySeries ?? new List<RaceSchedule>(),
                        "series_3" => parentObject.TruckSeries ?? new List<RaceSchedule>(),
                        _ => new List<RaceSchedule>()
                    };

                    _logger.LogInformation($"Found {raceSchedules.Count} races for series: {series}");
                    return raceSchedules;
                }
                else
                {
                    _logger.LogWarning("Deserialized ParentObject is null");
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning($"No data found for year {year}");
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

        return new List<RaceSchedule>();
    }

    public async Task<List<int>> GetAvailableYearsAsync(string series, int startYear = 2020, int endYear = 2025)
    {
        var availableYears = new List<int>();

        for (int year = startYear; year <= endYear; year++)
        {
            var schedules = await GetRaceSchedulesAsync(year, series);
            if (schedules.Count > 0)
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