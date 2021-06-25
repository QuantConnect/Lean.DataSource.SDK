# Tutorial - Create Your Own Data Source

Implementing data sources is split into three parts:
  1. Creating the data source class ([`MyCustomDataType.cs`](https://github.com/QuantConnect/Lean.DataSource.SDK/blob/master/MyCustomDataType.cs))
  2. Creating data downloader/processor (`process.*`)
  3. Creating tests and a demonstration algorithm

## Prerequisites
  1. Fork this repository to your own GitHub profile
  2. Install [.NET 5.0 SDK](https://dotnet.microsoft.com/download/dotnet/5.0)

## Part 1: Setup C# Data Source Class
  1. Open the `MyCustomDataType.cs` file for editing
  2. Rename the class name `MyCustomDataType` to the data you'll be offering, starting with your vendor name (e.g. __`MyCompany`__`FlightData`)
  3. Remove the `SomeCustomProperty` property
  4. Add your dataset's fields/properties.
  * Add `[ProtoMember(n)]` to each field/property you add, where `n` starts at `10` and increments by `1` per field/property added
  5. Implement `GetSource(...)` to point to where your data lives
  * Replace `mycustomdatatype` with your vendor name (all lowercase), followed by the directory name where your data is in
  * Specify the file where your data is expected to be in
  * Use the `date` variable to get the date of data being requested
  * Use `config.Symbol.Value` to get the current ticker. Make sure that the ticker capitalization is correct. Default is uppercase.
  6. Implement `Reader(...)` to parse your data
  * Set `Symbol = config.Symbol` when creating the instance of the class
  * Set `EndTime` equal to the time the data first became available for consumption
  7. Implement `Clone()` to allow Lean to create copies of your data
  8. If your dataset is __NOT__ for equities data, Make `RequiresMapping()` return `false`, otherwise return `true` 
  * See the [data sources related to equities](#subsection---data-sources-related-to-equities) section for more details
  9. Make `IsSparseData()` return `true`
  10. Make `DefaultResolution()` return the resolution of your data if the user does not specify a resolution
  11. Make `SupportedResolutions()` return the resolutions that your data supports
  12. Set the timezone that your data is saved as in `DataTimeZone()`
  13. (Optional) Implement `ToString()` to return pretty output
  14. Rename the file `MyCustomDataType.cs` to the name of the class contained within
  15. Open the `QuantConnect.DataSource.csproj` file for editing
  16. Add `<AssemblyName>QuantConnect.DataSource.{{dataSourceClassName}}</AssemblyName>` below `<RootNamespace>QuantConnect.DataSource</RootNamespace>`
  * Replace `{{dataSourceClassName}}` with the name of the class you implemented

## Part 2: Setup Downloading/Processing Script
  1. Create one of the following files to download/process your data:
  * Python: [`process.py`](https://github.com/QuantConnect/Lean.DataSource.SDK/blob/master/process.sample.py)
  * Bash: [`process.sh`](https://github.com/QuantConnect/Lean.DataSource.SDK/blob/master/process.sample.sh)
  * Jupyter Notebook: [`process.ipynb`](https://github.com/QuantConnect/Lean.DataSource.SDK/blob/master/process.sample.ipynb)

  2. In `process.*`, output your processed/final data to: `/temp-output-directory/alternative/{{vendorName}}/{{dataSourceName}}/`
  * Replace `{{vendorName}}` with your vendor name (e.g. `quantconnect`)
  * Replace `{{dataSourceType}}` with the name of your data (e.g. `corporate-flights`)
  * Path should be completely lowercase, unless absolutely required
  * Do not use special characters in your output path (prefer `-` over `_` in directories, and `_` over `-` for file names)
  * __Output should be in CSV format__ (comma delimited)
  * Example output directory: `/temp-output-directory/alternative/quantconnect/fred`
  * Example output file: `/temp-output-directory/alternative/quantconnect/fred/oecdrecd.csv`

  3. If you are processing data that is associated with stocks/equities, review the [data sources related to equities](#subsection---data-sources-related-to-equities) section

## Part 3: Setup Testing and Demonstration Algorithm
  1. Edit [`Demonstration.cs`](https://github.com/QuantConnect/Lean.DataSource.SDK/blob/master/Demonstration.cs) and create an example of how to load and use your data
  * Rename the algorithm class name to the name of the class created in part 1
  * The algorithm should be very simple and minimal
  2. Open the [`tests/MyCustomDataTypeTests.cs`](https://github.com/QuantConnect/Lean.DataSource.SDK/blob/master/tests/MyCustomDataTypeTests.cs) file for editing
  3. Scroll to the bottom of the code and make `CreateNewInstance()` return your new data type
  * Data can be fake data, it doesn't have to be real
  * Set all fields/properties of your class when creating your new data type

  4. Ensure that tests are passing. Run the following commands in order to check for test status:
  * `dotnet build tests/Tests.csproj`
  * `dotnet test tests/bin/Debug/net5.0/Tests.dll`

  5. Rename `tests/MyCustomDataTypeTests.cs` to the name of the class you created in part 1, ending with "Tests.cs"

# Subsection - Data Sources Related to Equities

Your data source is related to equities whenever the following is true:
  * The data source describes data about a specific equity Symbol, e.g. AAPL
  * The data source is directly linked to the equity, i.e. if my data source describes data for AAPL, then this data __only__ applies to the AAPL equity Symbol

For equity related data sources, update `RequiresMapping()` to return `true` in the data source class you created in part 1

(Note: ticker `WW` is used for example purposes)

If your source/raw data is "point in time", then no further special handling is required. Example:
  * Ticker name as of today (2021-06-24) is `WW`
  * Ticker `WTW` was renamed to `WW` on 2019-04-19
  * Data before 2019-04-19 has ticker `WTW`, not `WW`

Otherwise, you'll need to use QuantConnect data to get the ticker's previous name at a given point in time.

To do so, follow the steps below (Python/Jupyter Notebooks only):

  1. Import required classes:
  * `from QuantConnect.Data.Auxiliary import *`
  * `from QuantConnect import *`
  2. Create a MapFileResolver instance: 
  * `resolver = MapFileResolver.Create(Globals.DataFolder, Market.USA)`
  3. For each ticker you encounter, resolve the map file, and provide the current time:
  * `map_file = resolver.ResolveMapFile('WW', datetime.now())`
  4. Get the ticker symbol for the date provided. Provide the time of the data you're processing that contains the ticker
  * `data_time = datetime(2018, 1, 1)`
  * `ticker = map_file.GetMappedSymbol(data_time)`
  5. (Optional) If you need a Symbol, you can create one:
  * `first_date = map_file.FirstDate`
  * `symbol = Symbol(SecurityIdentifier.GenerateEquity(first_date, ticker, Market.USA), ticker)`
  * `symbol` should now represent `WTW`