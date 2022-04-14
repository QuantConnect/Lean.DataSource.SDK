# Get {vendorNameDatasetName}
vendorNameDatasetName=${PWD##*.}
vendorNameDatasetNameUniverse=${vendorNameDatasetName}Universe

# Rename the MyCustomDataType.cs file to {vendorNameDatasetName}.cs
mv MyCustomDataType.cs ${vendorNameDatasetName}.cs
mv MyCustomDataUniverseType.cs ${vendorNameDatasetNameUniverse}.cs

# In the QuantConnect.DataSource.csproj file, rename the MyCustomDataType class to {vendorNameDatasetName}
sed -i "s/MyCustomDataType/$vendorNameDatasetName/g" QuantConnect.DataSource.csproj
sed -i "s/Demonstration.cs/${vendorNameDatasetName}Algorithm.cs/g" QuantConnect.DataSource.csproj
sed -i "s/DemonstrationUniverse.cs/${vendorNameDatasetNameUniverse}SelectionAlgorithm.cs/g" QuantConnect.DataSource.csproj

# In the {vendorNameDatasetName}.cs file, rename the MyCustomDataType class to {vendorNameDatasetName}
sed -i "s/MyCustomDataType/$vendorNameDatasetName/g" ${vendorNameDatasetName}.cs

# In the {vendorNameDatasetNameUniverse}.cs file, rename the MyCustomDataUniverseType class to {vendorNameDatasetNameUniverse}
sed -i "s/MyCustomDataUniverseType/$vendorNameDatasetNameUniverse/g" ${vendorNameDatasetNameUniverse}.cs

# In the {vendorNameDatasetName}Algorithm.cs file, rename the MyCustomDataType class to to {vendorNameDatasetName}
sed -i "s/MyCustomDataType/$vendorNameDatasetName/g" Demonstration.cs
sed -i "s/MyCustomDataType/$vendorNameDatasetName/g" Demonstration.py

# In the {vendorNameDatasetName}Algorithm.cs file, rename the CustomDataAlgorithm class to {vendorNameDatasetName}Algorithm
sed -i "s/CustomDataAlgorithm/${vendorNameDatasetName}Algorithm/g" Demonstration.cs
sed -i "s/CustomDataAlgorithm/${vendorNameDatasetName}Algorithm/g" Demonstration.py

# In the {vendorNameDatasetName}UniverseSelectionAlgorithm.cs file, rename the MyCustomDataUniverseType class to to {vendorNameDatasetName}Universe
sed -i "s/MyCustomDataUniverseType/$vendorNameDatasetNameUniverse/g" DemonstrationUniverse.cs
sed -i "s/MyCustomDataUniverseType/$vendorNameDatasetNameUniverse/g" DemonstrationUniverse.py

# In the {vendorNameDatasetNameUniverse}SelectionAlgorithm.cs file, rename the CustomDataAlgorithm class to {vendorNameDatasetNameUniverse}SelectionAlgorithm
sed -i "s/CustomDataUniverse/${vendorNameDatasetNameUniverse}SelectionAlgorithm/g" DemonstrationUniverse.cs
sed -i "s/CustomDataUniverse/${vendorNameDatasetNameUniverse}SelectionAlgorithm/g" DemonstrationUniverse.py

# Rename the Lean.DataSource.vendorNameDatasetName/Demonstration.cs/py file to {vendorNameDatasetName}Algorithm.cs/py
mv Demonstration.cs ${vendorNameDatasetName}Algorithm.cs
mv Demonstration.py ${vendorNameDatasetName}Algorithm.py

# Rename the Lean.DataSource.vendorNameDatasetName/DemonstrationUniverseSelectionAlgorithm.cs/py file to {vendorNameDatasetName}UniverseSelectionAlgorithm.cs/py
mv DemonstrationUniverse.cs ${vendorNameDatasetNameUniverse}SelectionAlgorithm.cs
mv DemonstrationUniverse.py ${vendorNameDatasetNameUniverse}SelectionAlgorithm.py

# Rename the tests/MyCustomDataTypeTests.cs file to tests/{vendorNameDatasetName}Tests.cs
sed -i "s/MyCustomDataType/${vendorNameDatasetName}/g" tests/MyCustomDataTypeTests.cs
mv tests/MyCustomDataTypeTests.cs tests/${vendorNameDatasetName}Tests.cs

# In tests/Tests.csproj, rename the Demonstration.cs and DemonstrationUniverse.cs to {vendorNameDatasetName}Algorithm.cs and {vendorNameDatasetNameUniverse}SelectionAlgorithm.cs
sed -i "s/Demonstration.cs/${vendorNameDatasetName}Algorithm.cs/g" tests/Tests.csproj
sed -i "s/DemonstrationUniverse.cs/${vendorNameDatasetNameUniverse}SelectionAlgorithm.cs/g" tests/Tests.csproj

# In the MyCustomDataDownloader.cs and Program.cs files, rename the MyCustomDataDownloader to {vendorNameDatasetNameUniverse}DataDownloader
sed -i "s/MyCustomDataDownloader/${vendorNameDatasetNameUniverse}DataDownloader/g" DataProcessing/Program.cs
sed -i "s/MyCustomDataDownloader/${vendorNameDatasetNameUniverse}DataDownloader/g" DataProcessing/MyCustomDataDownloader.cs

# Rename the DataProcessing/MyCustomDataDownloader.cs file to DataProcessing/{vendorNameDatasetName}DataDownloader.cs
mv DataProcessing/MyCustomDataDownloader.cs DataProcessing/${vendorNameDatasetName}DataDownloader.cs