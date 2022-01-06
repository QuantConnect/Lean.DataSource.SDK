### Requesting Data
To add *`datasetName`* dataset by *`vendorName`* to your algorithm, use the AddData method to request the data. As with all datasets, you should save a reference to your symbol for easy use later in your algorithm. For detailed documentation on using custom data, see [Importing Custom Data](https://www.quantconnect.com/docs/algorithm-reference/importing-custom-data).

Python:
```
# pythonCodeToRequestData
```

C#:
```
// cSharpCodeToRequestData
```

### Accessing Data
Data can be accessed via Slice events. Slice delivers unique events to your algorithm as they happen. We recommend saving the symbol object when you add the data for easy access to slice later. Data is available in *`resolution`* resolution. You can see an example of the slice accessor in the code below.

Python:
```
# pythonCodeToAccessData
```

C#:
```
// cSharpCodeToAccessData
```


### Historical Data
You can request historical custom data in your algorithm using the custom data Symbol object. To learn more about historical data requests, please visit the [Historical Data](https://www.quantconnect.com/docs/algorithm-reference/historical-data) documentation. If there is no custom data in the period you request, the history result will be empty. The following example gets the historical data for *`datasetName`* *`datasetProperties`* by using the History API.

Python:
```
# pythonCodeToGetHistoricalData
```

C#:
```
// cSharpCodeToGetHistoricalData
```