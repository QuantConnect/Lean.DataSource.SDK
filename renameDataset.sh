# Get {vendorNameDatasetName}
vendorNameDatasetName=${PWD##*.}

# Rename the MyCustomDataType.cs file to {vendorNameDatasetName}.cs
mv MyCustomDataType.cs ${vendorNameDatasetName}.cs

# In the {vendorNameDatasetName}.cs file, rename the MyCustomDataType class to {vendorNameDatasetName}
sed -i "s/MyCustomDataType/$vendorNameDatasetName/g" ${vendorNameDatasetName}.cs

# Rename the Lean.DataSource.vendorNameDatasetName/Demonstration.cs file to {vendorNameDatasetName}Algorithm.cs
mv Demonstration.cs ${vendorNameDatasetName}Algorithm.cs

# In the {vendorNameDatasetName}Algorithm.cs file, rename the CustomDataAlgorithm class to {vendorNameDatasetName}Algorithm
sed -i "s/CustomDataAlgorithm/${vendorNameDatasetName}Algorithm/g" ${vendorNameDatasetName}Algorithm.cs

# In the {vendorNameDatasetName}Algorithm.cs file, rename the MyCustomDataType class to to {vendorNameDatasetName}
sed -i "s/MyCustomDataType/$vendorNameDatasetName/g" ${vendorNameDatasetName}Algorithm.cs

# Rename the tests/MyCustomDataTypeTests.cs file to tests/{vendorNameDatasetName}Tests.cs
mv tests/MyCustomDataTypeTests.cs tests/${vendorNameDatasetName}Tests.cs
