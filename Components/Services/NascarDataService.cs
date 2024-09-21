using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;

public class NascarDataService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<NascarDataService> _logger;
    //initialize the HTTPClient and Logger
    public NascarDataService(HttpClient httpClient, ILogger<NascarDataService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
    //gathers race schedules for selected year and each series
    public async Task<List<RaceSchedule>> GetRaceSchedulesAsync(int year, string series)
    {
        try
        {
            //set API url
            string apiUrl = $"https://cf.nascar.com/cacher/{year}/race_list_basic.json";
            _logger.LogInformation($"Fetching race schedule for year: {year} from {apiUrl}");

            //attempt HTTP request
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                //Read in and serialize JSON data
                var json = await response.Content.ReadAsStringAsync();
                _logger.LogDebug($"Received JSON: {json}");

                var parentObject = JsonSerializer.Deserialize<ParentObject>(json);

                if (parentObject != null)
                {
                    //set series based on the input
                    List<RaceSchedule> raceSchedules = series switch
                    {
                        // If series is "series_1", return Cup Series or an empty list if null
                        "series_1" => parentObject.CupSeries ?? new List<RaceSchedule>(),
                        // If series is "series_2", return Xfinity Series or an empty list if null
                        "series_2" => parentObject.XfinitySeries ?? new List<RaceSchedule>(),
                        // If series is "series_3", return Truck Series or an empty list if null
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

        //Resturns an empty list if no data was collecxted or an exception was caught/error was caught
        return new List<RaceSchedule>();
    }

    //Attempts to gather avalible years 
    public async Task<List<int>> GetAvailableYearsAsync(string series)
    {
        var availableYears = new List<int>();
        for (int year = 1900; year <= 2024; year++)
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