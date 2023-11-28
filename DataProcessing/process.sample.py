# CLRImports is required to handle Lean C# objects for Mapped Datasets (Single asset and Universe Selection)
# Requirements:
# python -m pip install clr-loader==0.1.7
# python -m pip install pythonnet==3.0.0a2
# This script must be executed in ./bin/Debug/net6.0 after the follwing command is executed
# dotnet build .\DataProcessing\
import os
from CLRImports import *

# To use QuantBook, we need to set its internal handlers
# We download LEAN confif with the default settings 
with open("quantbook.json", 'w') as fp:
    from requests import get
    response = get("https://raw.githubusercontent.com/QuantConnect/Lean/master/Launcher/config.json")
    fp.write(response.text)

Config.SetConfigurationFile("quantbook.json")
Config.Set("composer-dll-directory", os.path.dirname(os.path.realpath(__file__)))

# Set the data folder
Config.Set("data-folder", '<path-to-data-folder>')

# To generate the Security Identifier, we need to create and initialize the Map File Provider
# and call the SecurityIdentifier.GenerateEquity method
mapFileProvider = LocalZipMapFileProvider()
mapFileProvider.Initialize(DefaultDataProvider())
sid = SecurityIdentifier.GenerateEquity("SPY", Market.USA, True, mapFileProvider, datetime(2022, 3, 1))

qb = QuantBook()
symbol = Symbol(sid, "SPY")
history = qb.History(symbol, 3600, Resolution.Daily)
print(history)