# PowerTradeApp

## Description
PowerTradeApp is a tool designed to automate the extraction and downloading of power position data in CSV format. It connects to the PowerPosition service, retrieves aggregated data, and stores it locally for further use or processing. The tool supports configurable intervals for periodic downloads and can save the files to custom or default folders.

## Features
- **CSV Extraction**: Extracts power position data from a service and generates a CSV file.
- **Custom Folders**: Allows setting custom folder paths for saving the CSV files.
- **Periodical Extraction**: Supports scheduled extraction at defined intervals.
- **Logging**: Detailed logging for tracking the app's operations.
- **Configuration**: Configuration settings for default folder paths and interval times.

## Getting Started

### Prerequisites
To run this application, ensure you have the following installed on your machine:
- [.NET SDK 9.0 or higher](https://dotnet.microsoft.com/download)
- Access to the PowerPosition service

### Installation
1. Clone the repository:
    ```bash
    git clone https://github.com/yourusername/PowerTradeApp.git
    ```
   
2. Navigate to the project directory:
    ```bash
    cd PowerTradeApp
    ```

3. Restore dependencies:
    ```bash
    dotnet restore
    ```

4. Build the project:
    ```bash
    dotnet build
    ```

5. Run the application:
    ```bash
    dotnet run
    ```

### Configuration
The application reads configuration settings from the `appsettings.json` file, which is located in the root directory. You can modify the following properties:
- **IntervalMinutes**: The interval in minutes for periodic CSV extraction.
- **FolderPath**: The default folder path where CSV files are saved. Be mindful that his property is sensitive to absolute or relative paths

Example `appsettings.json`:
```json
{
  "IntervalMinutes": 30,
  "FolderPath": "C:/power-position-data/"
}
```

### Usage
Once you run the application, you'll be presented with a menu. The menu options are:
1. **Download CSV to the default folder**: Downloads CSV to the folder path specified in the configuration file.
2. **Download CSV to a custom folder**: Allows you to specify a custom folder to save the CSV.
3. **Change default folder path**: Updates the default folder path in the configuration file.
4. **Change interval of periodical download**: Updates the interval time (in minutes) for periodical downloads.
5. **Exit**: Closes the application.

Example of running the app:

```bash
Welcome to the PowerTradeApp.
============================================================
Select an option:
1 - Download CSV to the default folder (set in configuration file)
2 - Download CSV to a custom folder (this option also changes configuration file)
3 - Change default folder path
4 - Change interval of periodical download of CSV
5 - Exit
============================================================
```

When you choose an option, the application will either download the CSV, change settings, or exit based on your input. 

For example:

- **Option 1**: Download CSV to the default folder
  - The application will download the CSV to the default folder path specified in the configuration file (usually found in `appsettings.json`).
  - The path to the default folder is set under the `FolderPath` setting in the configuration file.

- **Option 2**: Download CSV to a custom folder
  - You will be prompted to enter a custom folder path.
  - The CSV will be saved to the folder you provide.
  - The default folder path in the configuration file will be updated to the new folder you specify.

- **Option 3**: Change the default folder path
  - You can enter a new folder path that will become the default folder for saving CSV files.
  - This change will be reflected in the configuration file, so the new path will be used for future CSV downloads.

- **Option 4**: Change the interval of the periodical download of CSV
  - You can update the interval (in minutes) for periodic CSV downloads. 
  - The interval determines how often the CSV file will be downloaded automatically.

- **Option 5**: Exit
  - Exits the application gracefully. No further actions will be taken, and the application will close.
