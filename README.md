# LeanDataSdk

[![Build Status](https://github.com/QuantConnect/LeanDataSdk/workflows/Build%20%26%20Test/badge.svg)](https://github.com/QuantConnect/LeanDataSdk/actions?query=workflow%3A%22Build%20%26%20Test%22)

### Getting started

#### Introduction

The Lean Data SDK is a cross-platform template repository for developing custom data types for Lean.
These data types will be consumed by [QuantConnect](https://www.quantconnect.com/) trading algorithms and research environment, locally or in the cloud.

It is composed by example .Net solution for the data type and converter scripts.

#### Prerequisites

The solution targets dotnet 5, for installation instructions please follow [dotnet download](https://dotnet.microsoft.com/download).

The data downloader and converter script can be developed in different ways: Python script, C# or Python jupyter notebook or even a bash script.
- The python script should be compatible with python 3.6.8
- C# notebook will run using [dotnet interactive](https://github.com/dotnet/interactive)
- Bash script will run on Ubuntu Bionic

Specifically, the enviroment where these scripts will be run is [quantconnect/research](https://hub.docker.com/repository/docker/quantconnect/research) based on [quantconnect/lean:foundation](https://hub.docker.com/repository/docker/quantconnect/lean).

#### Installation

This repository should be forked by each new data provider.

Once it is cloned locally, should be able to successfully build the solution, run all tests and execute the conveter scripts.

#### Usage

- Once the repository is forked, the existing example implementation should be adjusted to create a new data type for a particular data set.
	- The Assembly name and data type have to be changed since they should be unique.
- Converter and downloader scripts should be developed following the [examples in this repository](https://github.com/QuantConnect/LeanDataSdk/tree/master/DataConverterScript). These script should be provided to `QuantConnect` as well as the fork repository at a particual commit.

### User guide

TODO:

### Tutorials

#### Create Data Type

##### Introduction

This tutorial we will create a new custom C# data type that will allow Lean algorithms or research environment to consume a particular data set.

##### New Lean Data Type

In [Lean](https://github.com/QuantConnect/Lean) each data type inherits from [BaseData](https://github.com/QuantConnect/Lean/blob/master/Common/Data/BaseData.cs), overrides a set of methods and incoporates any specific property this data set has.
The `DataLibrary` project holds an example custom data type [MyCustomDataType](https://github.com/QuantConnect/LeanDataSdk/blob/master/DataLibrary/MyCustomDataType.cs).

- `GetSource()` method returns an instance of `SubscriptionDataSource` which will tell Lean from where should it source data for a particular given date, ticker, and configuration.
- `Reader()` method should return a new instance of this data type for a given line of data
- `Clone()` Clones the data
- `RequiresMapping()` Indicates whether the data source is tied to an underlying symbol and requires that corporate events be applied to it as well, such as renames and delistings
- `IsSparseData()` Indicates whether the data is sparse. If true, we disable logging for missing files
- `ToString()` converts the instance to string format
- `DefaultResolution()` gets the default resolution for this data and security type if the user provided none
- `SupportedResolutions()` gets the supported resolution for this data and security type
- `DataTimeZone()` specifies the data time zone for this data type

##### Tests

It will be a requisite that each data type has a json and protobuf round trip serialization and deserialization, as well as a clone unit test. Examples provided at [MyCustomDataTypeTests](https://github.com/QuantConnect/LeanDataSdk/blob/master/Tests/MyCustomDataTypeTests.cs)

The only adjusment `MyCustomDataTypeTests` test suite requires for a new data type should be the `CreateNewInstance()` method. Which should returned a fully initialized data point.

#### Create Algorithm

##### Introduction

Creating an example `QCAlgorithm` will allow quants to understand how to consume a data set and what value could it provide to their trading strategy.

##### Developing Algorithm

A [sample](https://github.com/QuantConnect/LeanDataSdk/blob/master/Tests/CustomDataAlgorithm.cs) algorithm is provided in this repository for the defined custom data type.

- `Initialize()` Specifies the **data** and resolution required, as well as the cash and start-end dates for the algorithm. This is where the custom data should be added.
- `OnData(Slice slice)` is the primary entry point for the algorithm. Each new data point will be pumped through it. This should be where the custom data is retrieved from the `slice` object and used.

#### Create Data Converters

##### Introduction

Data converter scripts will be in charge of fetching new data and processing it into a format that Lean and the [new data type](https://github.com/QuantConnect/LeanDataSdk#create-data-type) will be able to read.

TODO:

##### Python Notebook
##### CSharp Notebook
##### Python Script
##### Bash Script

### Api reference

TODO: