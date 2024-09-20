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
        var response = await _httpClient.GetAsync($"https://cf.nascar.com/cacher/{year}/race_list_basic.json"); 
        response.EnsureSuccessStatusCode();
        var jsonResponse = await response.Content.ReadAsStringAsync();
        //Console.WriteLine(jsonResponse);
        
        var json = JsonSerializer.Deserialize<Dictionary<string, List<RaceSchedule>>>(jsonResponse);
        var RaceSchedules = json[series];
        return RaceSchedules;
    }
}