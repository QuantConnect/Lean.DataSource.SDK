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
using System.Linq;
using NUnit.Framework;
using QuantConnect.Data;
using QuantConnect.Util;
using QuantConnect.Tests;
using QuantConnect.DataSource;
using QuantConnect.Securities;
using System.Collections.Generic;

namespace QuantConnect.DataLibrary.Tests
{
    [TestFixture]
    public class MyCustomDataProviderTests
    {
        private static IEnumerable<TestCaseData> TestParameters
        {
            get
            {
                TestGlobals.Initialize();
                var equity = Symbol.Create("SPY", SecurityType.Equity, Market.USA);
                var option = Symbol.Create("SPY", SecurityType.Option, Market.USA);

                yield return new TestCaseData(equity, Resolution.Daily, TickType.Trade, TimeSpan.FromDays(15), true)
                    .SetDescription("Valid parameters - Daily resolution, 15 days period.")
                    .SetCategory("Valid");

                yield return new TestCaseData(equity, Resolution.Hour, TickType.Quote, TimeSpan.FromDays(2), true)
                    .SetDescription("Valid parameters - Hour resolution, 2 days period.")
                    .SetCategory("Valid");

                yield return new TestCaseData(option, Resolution.Second, TickType.Trade, TimeSpan.FromMinutes(60), false)
                    .SetDescription("Invalid Symbol - Canonical doesn't support")
                    .SetCategory("Invalid");
            }
        }

        [Test, TestCaseSource(nameof(TestParameters))]
        public void GetsHistory(Symbol symbol, Resolution resolution, TickType tickType, TimeSpan period, bool isThrowNotImplementedException)
        {
            var historyDataProvider = new MyCustomDataProvider();

            var utcNow = DateTime.UtcNow;
            var dataType = LeanData.GetDataType(resolution, tickType);
            var marketHoursDatabase = MarketHoursDatabase.FromDataFolder();

            var exchangeHours = marketHoursDatabase.GetExchangeHours(symbol.ID.Market, symbol, symbol.SecurityType);
            var dataTimeZone = marketHoursDatabase.GetDataTimeZone(symbol.ID.Market, symbol, symbol.SecurityType);

            var requests = new[] {
                new HistoryRequest(
                startTimeUtc: utcNow.Add(-period),
                endTimeUtc: utcNow,
                dataType: dataType,
                symbol: symbol,
                resolution: resolution,
                exchangeHours: exchangeHours,
                dataTimeZone: dataTimeZone,
                fillForwardResolution: resolution,
                includeExtendedMarketHours: true,
                isCustomData: false,
                DataNormalizationMode.Raw,
                tickType: tickType
                )
            };

            try
            {
                IEnumerable<Slice> slices = historyDataProvider.GetHistory(requests, TimeZones.Utc)?.ToList();
                Assert.IsNull(slices);
            }
            catch (NotImplementedException)
            {
                Assert.IsTrue(isThrowNotImplementedException);
            }
        }
    }
}
