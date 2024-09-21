using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

public class RaceScheduleViewModel : INotifyPropertyChanged
{
    private readonly NascarDataService _dataService;
    private readonly ILogger<RaceScheduleViewModel> _logger;

    public bool IsLoading {get; set;}
    public string? ErrorMessage {get; set;}

    // Selected year
    public int SelectedYear {get; set;}
    public List<int> AvailableYears {get; set;} = new List<int>();

    // Race schedules
    public List<RaceSchedule> CupSeriesSchedules {get; set;} = new List<RaceSchedule>();
    public List<RaceSchedule> XfinitySeriesSchedules {get; set;} = new List<RaceSchedule>();
    public List<RaceSchedule> TruckSeriesSchedules {get; set;} = new List<RaceSchedule>();

    public Dictionary<string, string> seriesMapping = new Dictionary<string, string>
    {
        {"Cup Series", "series_1"},
        {"Xfinity Series", "series_2"},
        {"Truck Series", "series_3"}
    };

    public RaceScheduleViewModel(NascarDataService dataService, ILogger<RaceScheduleViewModel> logger)
    {
        _dataService = dataService;
        _logger = logger;
        SelectedYear = DateTime.Now.Year;  // Default to current year
        ErrorMessage = string.Empty;
    }

    // Load schedules for all series
    public async Task LoadAllSchedules()
    {
        IsLoading = true;
        ErrorMessage = string.Empty;

        try
        {
            CupSeriesSchedules = await _dataService.GetRaceSchedulesAsync(SelectedYear, "Cup Series");
            XfinitySeriesSchedules = await _dataService.GetRaceSchedulesAsync(SelectedYear, "Xfinity Series");
            TruckSeriesSchedules = await _dataService.GetRaceSchedulesAsync(SelectedYear, "Truck Series");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading race schedules");
            ErrorMessage = "Failed to load race schedules.";
        }
        finally
        {
            IsLoading = false;
            OnPropertyChanged(nameof(CupSeriesSchedules));
            OnPropertyChanged(nameof(XfinitySeriesSchedules));
            OnPropertyChanged(nameof(TruckSeriesSchedules));
        }
    }

    // Load available years for a series
    public async Task LoadAvailableYears(string series)
    {
        IsLoading = true;
        ErrorMessage = string.Empty;

        try
        {
            AvailableYears = await _dataService.GetAvailableYearsAsync(series);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading available years");
            ErrorMessage = "Failed to load available years.";
        }
        finally
        {
            IsLoading = false;
            OnPropertyChanged(nameof(AvailableYears));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using System.ComponentModel;
// using System.Runtime.CompilerServices;
// using System.Text.Json;
// using Microsoft.Extensions.Logging;

// public class RaceScheduleViewModel : INotifyPropertyChanged
// {
//     private readonly NascarDataService _dataService;
//     private readonly ILogger<RaceScheduleViewModel> _logger; 
//     public List<RaceSchedule> CupSeriesSchedules {get; set;} = new List<RaceSchedule>();
//     public List<RaceSchedule> XfinitySeriesSchedules {get; set;} = new List<RaceSchedule>();
//     public List<RaceSchedule> TruckSeriesSchedules {get; set;} = new List<RaceSchedule>();
//     private List<RaceSchedule> _raceSchedules;
//     private string _errorMessage;
//     private bool _isLoading;
   
//     public RaceScheduleViewModel(NascarDataService dataService, ILogger<RaceScheduleViewModel> logger)
//     {
//         //initialize the data service
//         _dataService = dataService;
//         _logger = logger;
//     }
//     public async Task LoadAllSchedules()
//     {
//         IsLoading = true;
//         ErrorMessage = null;
//         try
//         {
//             CupSeriesSchedules = await _dataService.GetRaceSchedulesAsync("2024", "series_1");
//             XfinitySeriesSchedules = await _dataService.GetRaceSchedulesAsync("2024", "series_2");
//             TruckSeriesSchedules = await _dataService.GetRaceSchedulesAsync("2024", "series_3");
//         }
//         catch (Exception ex)
//         {
//             _logger.LogError(ex, "Error loading all schedules");
//             ErrorMessage = "Failed to load all Race Schedule Data";
//         }
//         finally
//         {
//             IsLoading = false;
//         }
//     }
//     public string ErrorMessage
//     {
//         get => _errorMessage;
//         set
//         {
//             _errorMessage = value;
//             OnPropertyChanged();
//         }
//     }
//     public void SetError(string message)
//     {
//         ErrorMessage = message;
//     }

//     public bool IsLoading
//     {
//         get => _isLoading;
//         private set
//         {
//             _isLoading = value;
//             OnPropertyChanged();
//         }
//     }
//     public List<RaceSchedule> RaceSchedules
//     {
//         get => _raceSchedules ??=new List<RaceSchedule>();
//         private set
//         {
//             _raceSchedules = value;
//             OnPropertyChanged();
//         }
//     }
//     public event PropertyChangedEventHandler PropertyChanged;
//     protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
//     {
//         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//     }
// }
