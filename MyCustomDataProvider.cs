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

using NodaTime;
using QuantConnect.Data;
using System.Collections.Generic;
using QuantConnect.Lean.Engine.HistoricalData;
using QuantConnect.Lean.Engine.DataFeeds;
using System;

namespace QuantConnect.DataSource
{
    /// <summary>
    /// Implementation of Custom Data Provider
    /// </summary>
    public class MyCustomDataProvider : SynchronizingHistoryProvider
    {
        /// <inheritdoc cref="HistoryProviderBase.Initialize(HistoryProviderInitializeParameters)"/>
        public override void Initialize(HistoryProviderInitializeParameters parameters)
        { }

        /// <inheritdoc cref="HistoryProviderBase.GetHistory(IEnumerable{HistoryRequest}, DateTimeZone)"/>
        public override IEnumerable<Slice>? GetHistory(IEnumerable<HistoryRequest> requests, DateTimeZone sliceTimeZone)
        {
            // Create subscription objects from the configs
            var subscriptions = new List<Subscription>();
            foreach (var request in requests)
            {
                // Retrieve the history for the current request
                var history = GetHistory(request);

                if (history == null)
                {
                    // If history is null, it indicates that the request contains wrong parameters
                    // Handle the case where the request parameters are incorrect
                    continue;
                }

                var subscription = CreateSubscription(request, history);
                subscriptions.Add(subscription);
            }

            // Validate that at least one subscription is valid; otherwise, return null
            if (subscriptions.Count == 0)
            {
                return null;
            }

            return CreateSliceEnumerableFromSubscriptions(subscriptions, sliceTimeZone);
        }

        private IEnumerable<BaseData> GetHistory(HistoryRequest request)
        {
            if (!CanSubscribe(request.Symbol))
            {
                return null;
            }

            throw new NotImplementedException();
        }

        private bool CanSubscribe(Symbol symbol)
        {
            if (symbol.Value.IndexOfInvariant("universe", true) != -1 || symbol.IsCanonical())
            {
                return false;
            }

            throw new NotImplementedException();
        }
    }
}
