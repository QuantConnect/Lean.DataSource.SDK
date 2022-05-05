# This file is used to import the environment and classes/methods of LEAN.
# so that any python file could be using LEAN's classes/methods.
from clr_loader import get_coreclr
from pythonnet import set_runtime

# process.runtimeconfig.json is created when we build the DataProcessing Project:
# dotnet build .\DataProcessing\DataProcessing.csproj
set_runtime(get_coreclr('process.runtimeconfig.json'))

from AlgorithmImports import *
from QuantConnect.Lean.Engine.DataFeeds import *
AddReference("Fasterflect")