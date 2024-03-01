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
using System.Threading;
using QuantConnect.Data;
using QuantConnect.Tests;
using QuantConnect.Logging;
using System.Threading.Tasks;
using QuantConnect.Data.Market;
using System.Collections.Generic;
using QuantConnect.Lean.DataSource.MyCustom;

namespace QuantConnect.DataLibrary.Tests
{
    [TestFixture]
    public class MyCustomDataQueueHandlerTests
    {
        private static IEnumerable<TestCaseData> TestParameters
        {
            get
            {
                yield return new TestCaseData(Symbols.AAPL, Resolution.Tick);
                yield return new TestCaseData(Symbols.SPY, Resolution.Second);
                yield return new TestCaseData(Symbols.BTCUSD, Resolution.Minute);
            }
        }

        [Test, TestCaseSource(nameof(TestParameters))]
        public void StreamsData(Symbol symbol, Resolution resolution)
        {
            Assert.Pass();

            var dataQueueHandlerProvider = new MyCustomDataProvider();

            var configs = GetSubscriptionDataConfigs(symbol, resolution).ToList();

            foreach (var config in configs)
            {
                ProcessFeed(
                    dataQueueHandlerProvider.Subscribe(config, (s, e) => { }),
                    (baseData) =>
                    {
                        if (baseData != null)
                        {
                            Log.Trace($"{baseData}");
                        }
                    });
            }

            Thread.Sleep(20_000);

            foreach (var config in configs)
            {
                dataQueueHandlerProvider.Unsubscribe(config);
            }

            Thread.Sleep(1_000);
        }

        private IEnumerable<SubscriptionDataConfig> GetSubscriptionDataConfigs(Symbol symbol, Resolution resolution)
        {
            if (resolution == Resolution.Tick)
            {
                yield return new SubscriptionDataConfig(GetSubscriptionDataConfig<Tick>(symbol, resolution), tickType: TickType.Trade);
                yield return new SubscriptionDataConfig(GetSubscriptionDataConfig<Tick>(symbol, resolution), tickType: TickType.Quote);
            }
            else
            {
                yield return GetSubscriptionDataConfig<QuoteBar>(symbol, resolution);
                yield return GetSubscriptionDataConfig<TradeBar>(symbol, resolution);
            }
        }

        private static SubscriptionDataConfig GetSubscriptionDataConfig<T>(Symbol symbol, Resolution resolution)
        {
            return new SubscriptionDataConfig(
                typeof(T),
                symbol,
                resolution,
                TimeZones.Utc,
                TimeZones.Utc,
                true,
                extendedHours: false,
                false);
        }

        private Task ProcessFeed(IEnumerator<BaseData> enumerator, Action<BaseData> callback = null)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    while (enumerator.MoveNext())
                    {
                        BaseData tick = enumerator.Current;

                        if (tick != null)
                        {
                            callback?.Invoke(tick);
                        }
                    }
                }
                catch
                {
                    throw;
                }
            });
        }
    }
}
