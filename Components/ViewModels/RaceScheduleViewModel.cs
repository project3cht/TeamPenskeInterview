using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.Extensions.Logging;

public class RaceScheduleViewModel : INotifyPropertyChanged
{
    private readonly NascarDataService _dataService;
    private readonly ILogger<RaceScheduleViewModel> _logger; 
    private List<RaceSchedule> _raceSchedules;
    private string _errorMessage;
    private bool _isLoading;
    public Dictionary<string, string> seriesMapping = new Dictionary<string, string>
    {
        { "Cup Series", "series_1" },
        { "Xfinity Series", "series_2" },
        { "Truck Series", "series_3" }
    };
    public RaceScheduleViewModel(NascarDataService dataService, ILogger<RaceScheduleViewModel> logger)
    {
        //initialize the data service and race schedules
        _dataService = dataService;
        _logger = logger;
        _raceSchedules = new List<RaceSchedule>();
    }
    public async Task LoadSchedules(string year, string series)
    {
        _logger.LogInformation("Loading schedules for {Year} and {Series}", year, series);
        if (seriesMapping.TryGetValue(series, out string actualSeriesKey))
        {
            try
            {
                IsLoading = true;
                //ErrorMessage = null;
                
                // Fetch schedules based on year and series selected
                RaceSchedules = await _dataService.GetRaceSchedulesAsync(year, actualSeriesKey);
                _logger.LogInformation("Successfully loaded schedules for {SeriesKey}", actualSeriesKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading schedules for {Year} and {Series}", year, series);
                ErrorMessage = $"Failed to load Race Schedule Data: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
        else
        {
            ErrorMessage = "Invalid Series Selection";
            _logger.LogWarning("Invalid series selection attempted: {Series}", series);
        }
    }
    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }
    public void SetError(string message)
    {
        ErrorMessage = message;
    }

    public bool IsLoading
    {
        get => _isLoading;
        private set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }
    public List<RaceSchedule> RaceSchedules
    {
        get => _raceSchedules ??=new List<RaceSchedule>();
        private set
        {
            _raceSchedules = value;
            OnPropertyChanged();
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
