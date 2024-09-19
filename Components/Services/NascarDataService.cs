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
    return JsonSerializer.Deserialize<List<RaceSchedule>>(jsonResponse);
}

}

