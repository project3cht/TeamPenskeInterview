using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;

public class NascarDataService
{
    private readonly HttpClient _httpClient;

    public NascarDataService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<RaceSchedule>> GetRaceSchedulesAsync(string year)
    {
        var response = await _httpClient.GetAsync($"https://cf.nascar.com/cacher/{year}/race_list_basic.json"); 
        response.EnsureSuccessStatusCode();
        var jsonResponse = await response.Content.ReadAsStringAsync();
        //Console.WriteLine(jsonResponse);
        
        var json = JsonSerializer.Deserialize<Dictionary<string, List<RaceSchedule>>>(jsonResponse);
        var RaceSchedules = json["series_1"];
        return RaceSchedules;
        // var RaceSchedules = JsonSerializer.Deserialize<List<RaceSchedule>>(jsonResponse);
        // return RaceSchedules;
    }

}

