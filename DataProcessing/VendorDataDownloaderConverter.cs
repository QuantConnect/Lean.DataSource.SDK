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
using System.IO;

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
                // Your data downloading/processing code goes here
            }
            catch (Exception e)
            {
                Log.Error(e, $"Failed to download/convert {VendorName} {VendorDataName} data");
                return false;
            }

            return true;
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
