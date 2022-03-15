# CLRImports is required to handle Lean C# objects for Mapped Datasets (Single asset and Universe Selection)
from CLRImports import *

# To generate the Security Identifier, we need to create and initialize the Map File Provider
# and call the SecurityIdentifier.GenerateEquity method
mapFileProvider = LocalZipMapFileProvider()
mapFileProvider.Initialize(DefaultDataProvider())
sid = SecurityIdentifier.GenerateEquity("BAC", Market.USA, True, mapFileProvider, datetime(2022, 3, 1))