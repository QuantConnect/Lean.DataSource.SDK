/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using QuantConnect.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using QuantConnect.DataLibrary;

namespace QuantConnect.DataProcessing
{
    /// <summary>
    /// Data downloader/converter class example
    /// </summary>
    public class VendorDataDownloaderConverter : IDisposable
    {
        /// <summary>
        /// Name of the vendor. Only alphanumeric characters, all lowercase and no underscores/spaces. 
        /// </summary>
        public const string VendorName = "gaussgodel";
        
        /// <summary>
        /// Type of data that we are downloading/converting. Only alphanumeric characters, all lowercase and no underscores/spaces. 
        /// </summary>
        public const string VendorDataName = "flights";
        
        /// <summary>
        /// API key to download data with
        /// </summary>
        private readonly string _apiKey;
        
        /// <summary>
        /// Directory that data will be outputted to
        /// </summary>
        private readonly string _destinationDirectory;

        /// <summary>
        /// Has the class been cleaned up yet
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Creates a new instance of the data downloader/converter
        /// </summary>
        /// <param name="destinationDataFolder">The path to the data folder we want to save data to</param>
        /// <param name="apiKey">The API key to download data with</param>
        public VendorDataDownloaderConverter(string destinationDataFolder, string apiKey = null)
        {
            _apiKey = apiKey;
            _destinationDirectory = Path.Combine(
                destinationDataFolder,
                "alternative",
                VendorName,
                VendorDataName);

            // Create the directory ahead of time so that we don't get
            // errors when trying to write to this output directory.
            Directory.CreateDirectory(_destinationDirectory);
        }

        /// <summary>
        /// Begins running the downloader/converter
        /// </summary>
        /// <returns>True if downloading/converting was successful, false otherwise</returns>
        public bool Run()
        {
            try
            {
                // Your data downloading/processing code goes here. The lines below
                // can be deleted since they are only meant to be an example.
                // ================================================================
                var underlying = Symbol.Create("SPY", SecurityType.Equity, Market.USA);
                var symbol = Symbol.CreateBase(
                    typeof(MyCustomDataType),
                    underlying,
                    Market.USA);

                var lines = new[]
                {
                    "20131001,buy",
                    "20131003,buy",
                    "20131006,buy",
                    "20131007,sell",
                    "20131009,buy",
                    "20131011,sell"
                };
                
                var instances = lines.Select(x => ParseLine(symbol, x));
                var csvLines = instances.Select(x => ToCsvLine(x));
                
                SaveContentsToFile(symbol, csvLines);
            }
            catch (Exception e)
            {
                Log.Error(e, $"Failed to download/convert {VendorName} {VendorDataName} data");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Example method to parse and create an instance from a line of CSV
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="line">Line of raw data</param>
        /// <returns>Instance of <see cref="MyCustomDataType"/></returns>
        private MyCustomDataType ParseLine(Symbol symbol, string line)
        {
            var csv = line.Split(',');
            return new MyCustomDataType
            {
                Time = Parse.DateTimeExact(csv[0], "yyyyMMdd"),
                Symbol = symbol,
                
                SomeCustomProperty = csv[1]
            };
        }

        /// <summary>
        /// Example method to convert an instance to a CSV line
        /// </summary>
        /// <param name="instance">Custom Data instance</param>
        /// <returns>CSV line</returns>
        private string ToCsvLine(MyCustomDataType instance)
        {
            return string.Join(",",
                $"{instance.Time:yyyyMMdd}",
                instance.SomeCustomProperty);
        }

        /// <summary>
        /// Example method to save CSV lines to disk
        /// </summary>
        /// <param name="symbol">Symbol of the data</param>
        /// <param name="csvLines">CSV lines to write</param>
        private void SaveContentsToFile(Symbol symbol, IEnumerable<string> csvLines)
        {
            var ticker = symbol.Value.ToLowerInvariant();
            
            var tempPath = new FileInfo(Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}-{ticker}.csv"));
            var finalPath = Path.Combine(_destinationDirectory, $"{ticker}.csv");

            File.WriteAllLines(tempPath.FullName, csvLines);
            tempPath.MoveTo(finalPath, true);
        }

        /// <summary>
        /// If you need to shut down things like database connections, threads, or other
        /// resources that require manual shutdown, do it here. This will be called after
        /// we're done with the <see cref="Run"/> method.
        ///
        /// You don't have to implement this if you don't need to cleanup resources after we're done downloading/processing.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            // Your cleanup goes here. You don't have to implement it
            // if you have nothing that needs to be cleaned up.
            _disposed = true;
        }
    }
}
