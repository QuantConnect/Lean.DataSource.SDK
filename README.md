![LEAN Data Source SDK](http://cdn.quantconnect.com.s3.us-east-1.amazonaws.com/datasources/Github_LeanDataSourceSDK.png)

# Lean DataSource SDK

[![Build Status](https://github.com/QuantConnect/LeanDataSdk/workflows/Build%20%26%20Test/badge.svg)](https://github.com/QuantConnect/LeanDataSdk/actions?query=workflow%3A%22Build%20%26%20Test%22)

#### Introduction

The Lean Data SDK is a cross-platform template repository for developing custom data types for Lean.
These data types will be consumed by [QuantConnect](https://www.quantconnect.com/) trading algorithms and research environment, locally or in the cloud.

It is composed by example .Net solution for the data type and converter scripts.

#### Prerequisites

The solution targets dotnet 5, for installation instructions please follow [dotnet download](https://dotnet.microsoft.com/download).

The data downloader and converter script can be developed in different ways: Python script, Python jupyter notebook or even a bash script.
- The python script should be compatible with python 3.6.8
- Bash script will run on Ubuntu Bionic

Specifically, the enviroment where these scripts will be run is [quantconnect/research](https://hub.docker.com/repository/docker/quantconnect/research) based on [quantconnect/lean:foundation](https://hub.docker.com/repository/docker/quantconnect/lean).

#### Installation

This repository should be forked for each unique data source which requires its own data processing. Once it is cloned locally, you should be able to successfully build the solution, run all tests and execute the conveter scripts.

Once ready, please contact support@quantconnect.com and we will create a listing in the QuantConnect Data Market for your company and link to your public repository and commit hash. 

#### Usage

### Tutorials

 - See [Tutorials](https://github.com/QuantConnect/Lean/blob/master/tutorials.md) for a step by step guide for creating a new LEAN Data Source.
 
