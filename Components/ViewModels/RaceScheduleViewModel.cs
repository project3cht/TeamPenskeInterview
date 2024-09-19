using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class RaceScheduleViewModel : INotifyPropertyChanged
{
    private readonly NascarDataService _dataService;
    private List<RaceSchedule> _raceSchedules;
    private string _errorMessage;
    private bool _isLoading;

    public async Task LoadSchedules(string year)
    {
        //exceptions
        try
        {
            IsLoading = true;
            ErrorMessage = null;
            
            // Fetch schedules based on the year using the data service
            RaceSchedules = await _dataService.GetRaceSchedulesAsync(year);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load schedules: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }


    public string ErrorMessage
    {
        get => _errorMessage;
        private set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
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
        get => _raceSchedules;
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
