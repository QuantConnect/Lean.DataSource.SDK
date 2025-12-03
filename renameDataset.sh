#!/bin/bash

# Get {vendorNameDatasetName} from the directory name (e.g., Lean.DataSource.VendorName -> VendorName)
vendorNameDatasetName=${PWD##*.}
vendorNameDatasetNameUniverse=${vendorNameDatasetName}Universe

# Rename the MyCustomDataType.cs file to {vendorNameDatasetName}.cs
mv MyCustomDataType.cs ${vendorNameDatasetName}.cs
mv MyCustomDataUniverse.cs ${vendorNameDatasetNameUniverse}.cs

# Rename the MyCustomDataProvider.cs file to {vendorNameDatasetName}Provider.cs
mv MyCustomDataProvider.cs ${vendorNameDatasetName}Provider.cs

# Rename the MyCustomDataDownloader.cs file to {vendorNameDatasetName}Downloader.cs
mv MyCustomDataDownloader.cs ${vendorNameDatasetName}Downloader.cs

# In the QuantConnect.DataSource.csproj file, rename the MyCustomDataType class to {vendorNameDatasetName}
sed -i "s/MyCustomDataType/$vendorNameDatasetName/g" QuantConnect.DataSource.csproj
sed -i "s/MyCustomDataUniverse/$vendorNameDatasetNameUniverse/g" QuantConnect.DataSource.csproj
sed -i "s/MyCustomDataProvider/${vendorNameDatasetName}Provider/g" QuantConnect.DataSource.csproj
sed -i "s/MyCustomDataDownloader/${vendorNameDatasetName}Downloader/g" QuantConnect.DataSource.csproj
sed -i "s/Demonstration.cs/${vendorNameDatasetName}Algorithm.cs/g" QuantConnect.DataSource.csproj
sed -i "s/DemonstrationUniverse.cs/${vendorNameDatasetNameUniverse}SelectionAlgorithm.cs/g" QuantConnect.DataSource.csproj

# In the {vendorNameDatasetName}.cs file, rename the MyCustomDataType class to {vendorNameDatasetName}
sed -i "s/MyCustomDataType/$vendorNameDatasetName/g" ${vendorNameDatasetName}.cs

# In the {vendorNameDatasetNameUniverse}.cs file, rename the MyCustomDataUniverse class to {vendorNameDatasetNameUniverse}
sed -i "s/MyCustomDataUniverse/$vendorNameDatasetNameUniverse/g" ${vendorNameDatasetNameUniverse}.cs

# In the {vendorNameDatasetName}Provider.cs file, rename the MyCustomDataProvider class to {vendorNameDatasetName}Provider
sed -i "s/MyCustomDataProvider/${vendorNameDatasetName}Provider/g" ${vendorNameDatasetName}Provider.cs
sed -i "s/QuantConnect.Lean.DataSource.MyCustom/QuantConnect.Lean.DataSource.${vendorNameDatasetName}/g" ${vendorNameDatasetName}Provider.cs

# In the {vendorNameDatasetName}Downloader.cs file, rename the MyCustomDataDownloader class to {vendorNameDatasetName}Downloader
sed -i "s/MyCustomDataDownloader/${vendorNameDatasetName}Downloader/g" ${vendorNameDatasetName}Downloader.cs
sed -i "s/MyCustomDataProvider/${vendorNameDatasetName}Provider/g" ${vendorNameDatasetName}Downloader.cs
sed -i "s/QuantConnect.Lean.DataSource.MyCustom/QuantConnect.Lean.DataSource.${vendorNameDatasetName}/g" ${vendorNameDatasetName}Downloader.cs

# In the {vendorNameDatasetName}Algorithm.cs file, rename the MyCustomDataType class to {vendorNameDatasetName}
sed -i "s/MyCustomDataType/$vendorNameDatasetName/g" Demonstration.cs
sed -i "s/MyCustomDataType/$vendorNameDatasetName/g" Demonstration.py

# In the {vendorNameDatasetName}Algorithm.cs file, rename the CustomDataAlgorithm class to {vendorNameDatasetName}Algorithm
sed -i "s/CustomDataAlgorithm/${vendorNameDatasetName}Algorithm/g" Demonstration.cs
sed -i "s/CustomDataAlgorithm/${vendorNameDatasetName}Algorithm/g" Demonstration.py

# In the {vendorNameDatasetNameUniverse}SelectionAlgorithm.cs file, rename the MyCustomDataUniverse class to {vendorNameDatasetNameUniverse}
sed -i "s/MyCustomDataUniverse/$vendorNameDatasetNameUniverse/g" DemonstrationUniverse.cs
sed -i "s/MyCustomDataUniverse/$vendorNameDatasetNameUniverse/g" DemonstrationUniverse.py

# In the {vendorNameDatasetNameUniverse}SelectionAlgorithm.cs file, rename the CustomDataUniverse class to {vendorNameDatasetNameUniverse}SelectionAlgorithm
sed -i "s/CustomDataUniverse/${vendorNameDatasetNameUniverse}SelectionAlgorithm/g" DemonstrationUniverse.cs
sed -i "s/CustomDataUniverse/${vendorNameDatasetNameUniverse}SelectionAlgorithm/g" DemonstrationUniverse.py

# Rename the Demonstration.cs/py file to {vendorNameDatasetName}Algorithm.cs/py
mv Demonstration.cs ${vendorNameDatasetName}Algorithm.cs
mv Demonstration.py ${vendorNameDatasetName}Algorithm.py

# Rename the DemonstrationUniverse.cs/py file to {vendorNameDatasetNameUniverse}SelectionAlgorithm.cs/py
mv DemonstrationUniverse.cs ${vendorNameDatasetNameUniverse}SelectionAlgorithm.cs
mv DemonstrationUniverse.py ${vendorNameDatasetNameUniverse}SelectionAlgorithm.py

# Rename the tests/MyCustomDataTypeTests.cs file to tests/{vendorNameDatasetName}Tests.cs
sed -i "s/MyCustomDataType/${vendorNameDatasetName}/g" tests/MyCustomDataTypeTests.cs
mv tests/MyCustomDataTypeTests.cs tests/${vendorNameDatasetName}Tests.cs

# In tests/MyCustomDataDownloaderTests.cs, rename the classes
sed -i "s/MyCustomDataDownloader/${vendorNameDatasetName}Downloader/g" tests/MyCustomDataDownloaderTests.cs
sed -i "s/MyCustomDataProviderHistoryTests/${vendorNameDatasetName}ProviderHistoryTests/g" tests/MyCustomDataDownloaderTests.cs
sed -i "s/QuantConnect.Lean.DataSource.MyCustom/QuantConnect.Lean.DataSource.${vendorNameDatasetName}/g" tests/MyCustomDataDownloaderTests.cs
mv tests/MyCustomDataDownloaderTests.cs tests/${vendorNameDatasetName}DownloaderTests.cs

# In tests/MyCustomDataProviderHistoryTests.cs, rename the classes
sed -i "s/MyCustomDataProvider/${vendorNameDatasetName}Provider/g" tests/MyCustomDataProviderHistoryTests.cs
sed -i "s/QuantConnect.Lean.DataSource.MyCustom/QuantConnect.Lean.DataSource.${vendorNameDatasetName}/g" tests/MyCustomDataProviderHistoryTests.cs
mv tests/MyCustomDataProviderHistoryTests.cs tests/${vendorNameDatasetName}ProviderHistoryTests.cs

# In tests/MyCustomDataQueueHandlerTests.cs, rename the classes
sed -i "s/MyCustomDataProvider/${vendorNameDatasetName}Provider/g" tests/MyCustomDataQueueHandlerTests.cs
sed -i "s/MyCustomDataQueueHandlerTests/${vendorNameDatasetName}QueueHandlerTests/g" tests/MyCustomDataQueueHandlerTests.cs
sed -i "s/QuantConnect.Lean.DataSource.MyCustom/QuantConnect.Lean.DataSource.${vendorNameDatasetName}/g" tests/MyCustomDataQueueHandlerTests.cs
mv tests/MyCustomDataQueueHandlerTests.cs tests/${vendorNameDatasetName}QueueHandlerTests.cs

# In tests/Tests.csproj, rename the Demonstration.cs and DemonstrationUniverse.cs
sed -i "s/Demonstration.cs/${vendorNameDatasetName}Algorithm.cs/g" tests/Tests.csproj
sed -i "s/DemonstrationUniverse.cs/${vendorNameDatasetNameUniverse}SelectionAlgorithm.cs/g" tests/Tests.csproj

# In the DataProcessing/MyCustomDataDownloader.cs and Program.cs files, rename MyCustomDataDownloader to {vendorNameDatasetName}DataDownloader
sed -i "s/MyCustomDataDownloader/${vendorNameDatasetName}DataDownloader/g" DataProcessing/Program.cs
sed -i "s/MyCustomDataDownloader/${vendorNameDatasetName}DataDownloader/g" DataProcessing/MyCustomDataDownloader.cs

# Rename the DataProcessing/MyCustomDataDownloader.cs file to DataProcessing/{vendorNameDatasetName}DataDownloader.cs
mv DataProcessing/MyCustomDataDownloader.cs DataProcessing/${vendorNameDatasetName}DataDownloader.cs

# Rename mycustomdata.json to {vendorNameDatasetName}.json (lowercase)
vendorNameLower=$(echo "$vendorNameDatasetName" | tr '[:upper:]' '[:lower:]')
mv mycustomdata.json ${vendorNameLower}.json