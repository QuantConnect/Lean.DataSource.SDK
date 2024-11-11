![LEAN Data Source SDK](http://cdn.quantconnect.com.s3.us-east-1.amazonaws.com/datasources/Github_LeanDataSourceSDK.png)

# Lean DataSource SDK

[![Build Status](https://github.com/QuantConnect/LeanDataSdk/workflows/Build%20%26%20Test/badge.svg)](https://github.com/QuantConnect/LeanDataSdk/actions?query=workflow%3A%22Build%20%26%20Test%22)

### Introduction

The Lean Data SDK is a cross-platform template repository for developing custom data types for Lean.
These data types will be consumed by [QuantConnect](https://www.quantconnect.com/) trading algorithms and research environment, locally or in the cloud.

It is composed by example .Net solution for the data type and converter scripts.

### Prerequisites

The solution targets dotnet 5, for installation instructions please follow [dotnet download](https://dotnet.microsoft.com/download).

The data downloader and converter script can be developed in different ways: C# executable, Python script, Python Jupyter notebook or even a bash script.
- The python script should be compatible with python 3.6.8
- Bash script will run on Ubuntu Bionic

Specifically, the enviroment where these scripts will be run is [quantconnect/research](https://hub.docker.com/repository/docker/quantconnect/research) based on [quantconnect/lean:foundation](https://hub.docker.com/repository/docker/quantconnect/lean).

### Installation

The "Use this template" feature should be used for each unique data source which requires its own data processing. Once it is cloned locally, you should be able to successfully build the solution, run all tests and execute the downloader and/or conveter scripts. The final version should pass all CI tests of GitHub Actions.

Once ready, please contact support@quantconnect.com and we will create a listing in the QuantConnect Data Market for your company and link to your public repository and commit hash. 

### Datasets Vendor Requirements

Key requirements for new vendors include:

 - A well-defined dataset with a clear and static vision for the data to minimize churn or changes as people will be building systems from it. This is easiest with "raw" data (e.g. sunshine hours vs a sentiment algorithm)
 - Suitable delievery frequency, e.g. daily, weekly, but not annually (unless it is a "research-base" or "regime-detection" dataset with sufficiently long history)
 - Robust ticker and security links to ensure the tickers are tracked well through time, or accurately point in time. ISIN, FIGI, or point in time ticker supported
 - Robust funding to ensure viable for at least 1 year
 - Robust API to ensure reliable up-time. No dead links on site or and 502 servers while using API
 - Consistent delivery schedule, on time and in time for market trading
 - Consistent data format with notifications and lead time on data format updates
 - At least 1 year of historical point in time data
 - Look-ahead bias free data
 - Survivorship bias free data
 - Good documentation for the dataset

### Checklist Before Development/Submission

Some key factors to check the validity of the data processing:

 - The timestamp of the data entry is instantenous, i.e. it is not roll back to add data to the previous date's data file.
   - Use a realistic timestamp for the data entry if the exact time of delivery is unknown
     - the processing time for the newly added entries
     - period offset, e.g. 1-week offset
     - etc.
 - Ensure that the processed data is time-named according to the representative date of the data, but not the end-time timestamp. 
   - For example, data of 2024-10-01 00:00 to 2024-10-02 00:00 should be named 20241001 but not 20241002.

### Tutorials

 - See [Tutorials](https://www.quantconnect.com/docs/v2/our-platform/datasets/contributing-datasets) for a step by step guide for creating a new LEAN Data Source.