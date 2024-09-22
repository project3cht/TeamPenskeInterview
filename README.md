# NASCAR Race Schedule Viewer



## Project Overview

The NASCAR Race Schedule Viewer is a Blazor WebAssembly application designed to display and manage NASCAR race schedules for different series (Cup Series, Xfinity Series, and Truck Series). It fetches data from the NASCAR API and presents it in an interactive, user-friendly interface.

## Features

-   View race schedules for Cup Series, Xfinity Series, and Truck Series
-   Filter schedules by year
-   Responsive design for various screen sizes
-   Error handling and user feedback
-   Logging for debugging and monitoring

## Technology Stack
-   .NET 6.0 or later
-   Blazor WebAssembly
-   C# for backend logic
-   HTML, CSS for frontend
-   NASCAR API for data retrieval

## Project Structure
```
NASCAR-Race-Schedule-Viewer/
├ Components/
│   └── NascarSeriesData.razor
│   └── NascarSeriesData.razor.cs
├ Models/
│   └── RaceSchedule.cs
├ Services/
│   └── NascarDataService.cs
├ ViewModels/
│   └── RaceScheduleViewModel.cs
├ Pages/
│   └── Home.razor
├ wwwroot/
│   └──  index.html
│   └── css/
│       └── app.css
└── Program.cs
```
Microsoft C# dev kit was used to provide me with an intial blazor applicaiton set up. 
`https://github.com/CommunityToolkit/MVVM-Samples.git` was used as a refernce to other open-source project files. This was useful to better understand the MVVM design structure. The null-coalescing operator '??' and nullable value types gretaly helped me work through various edge cases. 

## System Requirments

-   .NET 6.0 SDK or later
-   A modern web browser
-   Internet connection for API access

# Installation


1. Clone the repository:
- `git clone https://github.com/project3cht/TeamPenskeInterview.git`
2. Navigate to the Project Directory
 -if on Mac, the folder will be in your downloads directory
-   `cd Downloads/TeamPensekInterview` 
3. Restore project packages
- `cd dotnet restore`
4. Build the project
- `dotnet build` - if that did not work, try `dotnet clean` then re run `dotnet build`
5. Run the application
- `dotnet watch`


## Usage
1.  Open a web browser and navigate to  `https://localhost:5001`  (or the port specified in the console output).
2.  Use the year dropdown to select the desired year for race schedules.
3.  View the schedules for Cup Series, Xfinity Series, and Truck Series.
4.  Click on column headers to sort the data.
5.  Use the "Reload" button to refresh the data.


## API

The application integrates with the NASCAR API to fetch race schedule data. The API endpoint used is:

Copy

`https://cf.nascar.com/cacher/{year}/race_list_basic.json`

The  `NascarDataService`  class handles all API interactions.

## Component Breakdown

1.  **NascarSeriesData.razor**: The main component that orchestrates the display of race schedules.
2.  **RaceScheduleViewModel**: Manages the application state and business logic.
3.  **NascarDataService**: Handles API requests and data parsing.
4.  **RaceSchedule**: Model class representing a single race schedule entry.

## Error Handling

-   API request errors are caught and logged in  `NascarDataService`.
-   Data processing errors are handled in  `RaceScheduleViewModel`.
-   UI-level error messages are displayed to users when issues occur.

## Logging

The application uses the built-in .NET logging framework. Logs are generated for:
-   API requests and responses
-   Data processing steps
-   Errors and exceptions


## Future Enhancements

- Have the program properly fetch available years
- Have the reload button funtion as intended
- Add more detailed Race Information
- Implement sorting functionality across all data colunms
- Instead of having all three schedules on one page, have each race schedule have their own tab/page
