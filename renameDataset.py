import os
from subprocess import Popen
vendorNameDatasetName = os.path.basename(os.getcwd()).replace('Lean.DataSource.','')
vendorNameDatasetNameUniverse=f'{vendorNameDatasetName}Universe'

def RenameFile(src: str, dst: str) -> None:
    if os.path.isfile(dst):
        return
    if os.path.isfile(src):
        os.rename(src, dst)

def RenameFileContent(filename: str, old: str, new: str) -> None:
    file = f'{os.getcwd()}/{filename}'
    if not os.path.isfile(file):
        return
    with open(file, mode='r') as fp:
        content = fp.read()
    with open(file, mode='w') as fp:
        content = content.replace(old, new)
        fp.write(content)

def Build(csproj: str) -> None:
    cmd = f'dotnet build {csproj}'
    print(cmd)
    Popen(cmd).wait()

# Rename the MyCustomDataType.cs file to {vendorNameDatasetName}.cs
# In the {vendorNameDatasetName}.cs file, rename the MyCustomDataType class to {vendorNameDatasetName}
RenameFile('MyCustomDataType.cs', f'{vendorNameDatasetName}.cs')
RenameFileContent(f'{vendorNameDatasetName}.cs', 'MyCustomDataType', vendorNameDatasetName)
# Rename the MyCustomDataUniverseType.cs file to {vendorNameDatasetNameUniverse}.cs
# In the {vendorNameDatasetNameUniverse}.cs file, rename the MyCustomDataUniverseType class to {vendorNameDatasetNameUniverse}
RenameFile('MyCustomDataUniverseType.cs', f'{vendorNameDatasetNameUniverse}.cs')
RenameFileContent(f'{vendorNameDatasetNameUniverse}.cs', 'MyCustomDataUniverseType', vendorNameDatasetNameUniverse)

# In the QuantConnect.DataSource.csproj file, rename the MyCustomDataType class to {vendorNameDatasetName}
RenameFileContent('QuantConnect.DataSource.csproj', 'MyCustomDataType', vendorNameDatasetName)
RenameFileContent('QuantConnect.DataSource.csproj', 'Demonstration.cs', f'{vendorNameDatasetName}Algorithm.cs')
RenameFileContent('QuantConnect.DataSource.csproj', 'DemonstrationUniverse.cs', f'{vendorNameDatasetNameUniverse}SelectionAlgorithm.cs')

# In the {vendorNameDatasetName}Algorithm.cs file, rename the MyCustomDataType class to to {vendorNameDatasetName}
RenameFileContent('Demonstration.cs', 'MyCustomDataType', vendorNameDatasetName)
RenameFileContent('Demonstration.py', 'MyCustomDataType', vendorNameDatasetName)

# In the {vendorNameDatasetName}Algorithm.cs file, rename the CustomDataAlgorithm class to {vendorNameDatasetName}Algorithm
RenameFileContent('Demonstration.cs', 'CustomDataAlgorithm', f'{vendorNameDatasetName}Algorithm')
RenameFileContent('Demonstration.py', 'CustomDataAlgorithm', f'{vendorNameDatasetName}Algorithm')

# In the {vendorNameDatasetName}UniverseSelectionAlgorithm.cs file, rename the MyCustomDataUniverseType class to to {vendorNameDatasetName}Universe
RenameFileContent('DemonstrationUniverse.cs', 'MyCustomDataUniverseType', vendorNameDatasetNameUniverse)
RenameFileContent('DemonstrationUniverse.py', 'MyCustomDataUniverseType', vendorNameDatasetNameUniverse)

# In the {vendorNameDatasetNameUniverse}SelectionAlgorithm.cs file, rename the CustomDataAlgorithm class to {vendorNameDatasetNameUniverse}SelectionAlgorithm
RenameFileContent('DemonstrationUniverse.cs', 'CustomDataUniverse', f'{vendorNameDatasetNameUniverse}SelectionAlgorithm')
RenameFileContent('DemonstrationUniverse.py', 'CustomDataUniverse', f'{vendorNameDatasetNameUniverse}SelectionAlgorithm')

# Rename the Lean.DataSource.vendorNameDatasetName/Demonstration.cs/py file to {vendorNameDatasetName}Algorithm.cs/py
RenameFile('Demonstration.cs', f'{vendorNameDatasetName}Algorithm.cs')
RenameFile('Demonstration.py', f'{vendorNameDatasetName}Algorithm.py')

# Rename the Lean.DataSource.vendorNameDatasetName/DemonstrationUniverseSelectionAlgorithm.cs/py file to {vendorNameDatasetName}UniverseSelectionAlgorithm.cs/py
RenameFile('DemonstrationUniverse.cs', f'{vendorNameDatasetNameUniverse}SelectionAlgorithm.cs')
RenameFile('DemonstrationUniverse.py', f'{vendorNameDatasetNameUniverse}SelectionAlgorithm.py')

# Rename the tests/MyCustomDataTypeTests.cs file to tests/{vendorNameDatasetName}Tests.cs
RenameFileContent('tests/MyCustomDataTypeTests.cs', 'MyCustomDataType', vendorNameDatasetName)
RenameFile('tests/MyCustomDataTypeTests.cs', f'tests/{vendorNameDatasetName}Tests.cs')

# In tests/Tests.csproj, rename the Demonstration.cs and DemonstrationUniverse.cs to {vendorNameDatasetName}Algorithm.cs and {vendorNameDatasetNameUniverse}SelectionAlgorithm.cs
RenameFileContent('tests/Tests.csproj', 'Demonstration.cs', f'{vendorNameDatasetName}Algorithm.cs')
RenameFileContent('tests/Tests.csproj', 'DemonstrationUniverse.cs', f'{vendorNameDatasetNameUniverse}SelectionAlgorithm.cs')

# In the MyCustomDataDownloader.cs and Program.cs files, rename the MyCustomDataDownloader to {vendorNameDatasetNameUniverse}DataDownloader
RenameFileContent('DataProcessing/Program.cs', 'MyCustomDataDownloader', f'{vendorNameDatasetNameUniverse}DataDownloader')
RenameFileContent('DataProcessing/MyCustomDataDownloader.cs', 'MyCustomDataDownloader', f'{vendorNameDatasetNameUniverse}DataDownloader')

# Rename the DataProcessing/MyCustomDataDownloader.cs file to DataProcessing/{vendorNameDatasetName}DataDownloader.cs
RenameFile('DataProcessing/MyCustomDataDownloader.cs', f'DataProcessing/{vendorNameDatasetName}DataDownloader.cs')

# Build the projects
Build('.\QuantConnect.DataSource.csproj')
Build('.\tests\Tests.csproj')
Build('.\DataProcessing\DataProcessing.csproj')