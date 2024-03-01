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
using NUnit.Framework;
using System.Collections.Generic;
using QuantConnect.Lean.DataSource.MyCustom;

namespace QuantConnect.DataLibrary.Tests
{
    [TestFixture]
    public class MyCustomDataDownloaderTests
    {
        private static IEnumerable<TestCaseData> DownloadTestParameters => MyCustomDataProviderHistoryTests.TestParameters;

        [TestCaseSource(nameof(DownloadTestParameters))]
        public void DownloadHistory(Symbol symbol, Resolution resolution, TickType tickType, TimeSpan period, bool isThrowNotImplementedException)
        {
            var myCustomDownloader = new MyCustomDataDownloader();

            var request = MyCustomDataProviderHistoryTests.GetHistoryRequest(resolution, tickType, symbol, period);

            var parameters = new DataDownloaderGetParameters(symbol, resolution, request.StartTimeUtc, request.EndTimeUtc, tickType);

            Assert.Throws<NotImplementedException>(() => myCustomDownloader.Get(parameters));
        }
    }
}
