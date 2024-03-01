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
 *
*/

using System;
using QuantConnect.Data;
using System.Collections.Generic;
using QuantConnect.Util;

namespace QuantConnect.Lean.DataSource.MyCustom
{
    /// <summary>
    /// Data downloader class for pulling data from Data Provider
    /// </summary>
    public class MyCustomDataDownloader : IDataDownloader, IDisposable
    {
        /// <inheritdoc cref="MyCustomDataProvider"/>
        private readonly MyCustomDataProvider _myCustomDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyCustomDataDownloader"/>
        /// </summary>
        public MyCustomDataDownloader()
        {
            _myCustomDataProvider = new MyCustomDataProvider();
        }

        /// <summary>
        /// Get historical data enumerable for a single symbol, type and resolution given this start and end time (in UTC).
        /// </summary>
        /// <param name="dataDownloaderGetParameters">Parameters for the historical data request</param>
        /// <returns>Enumerable of base data for this symbol</returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<BaseData> Get(DataDownloaderGetParameters dataDownloaderGetParameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _myCustomDataProvider?.DisposeSafely();
        }
    }
}
