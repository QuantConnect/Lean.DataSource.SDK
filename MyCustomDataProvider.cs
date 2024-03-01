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
using NodaTime;
using QuantConnect.Data;
using QuantConnect.Util;
using QuantConnect.Interfaces;
using System.Collections.Generic;
using QuantConnect.Lean.Engine.DataFeeds;
using QuantConnect.Lean.Engine.HistoricalData;

namespace QuantConnect.Lean.DataSource.MyCustom
{
    /// <summary>
    /// Implementation of Custom Data Provider
    /// </summary>
    public class MyCustomDataProvider : SynchronizingHistoryProvider, IDataQueueHandler
    {
        /// <summary>
        /// <inheritdoc cref="IDataAggregator"/>
        /// </summary>
        private readonly IDataAggregator _dataAggregator;

        /// <summary>
        /// <inheritdoc cref="EventBasedDataQueueHandlerSubscriptionManager"/>
        /// </summary>
        private readonly EventBasedDataQueueHandlerSubscriptionManager _subscriptionManager;

        /// <summary>
        /// Returns true if we're currently connected to the Data Provider
        /// </summary>
        public bool IsConnected { get; }

        /// <inheritdoc cref="HistoryProviderBase.Initialize(HistoryProviderInitializeParameters)"/>
        public override void Initialize(HistoryProviderInitializeParameters parameters)
        { }

        /// <inheritdoc cref="HistoryProviderBase.GetHistory(IEnumerable{HistoryRequest}, DateTimeZone)"/>
        public override IEnumerable<Slice> GetHistory(IEnumerable<HistoryRequest> requests, DateTimeZone sliceTimeZone)
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

        /// <summary>
        /// Subscribe to the specified configuration
        /// </summary>
        /// <param name="dataConfig">defines the parameters to subscribe to a data feed</param>
        /// <param name="newDataAvailableHandler">handler to be fired on new data available</param>
        /// <returns>The new enumerator for this subscription request</returns>
        public IEnumerator<BaseData> Subscribe(SubscriptionDataConfig dataConfig, EventHandler newDataAvailableHandler)
        {
            if (!CanSubscribe(dataConfig.Symbol))
            {
                return null;
            }

            var enumerator = _dataAggregator.Add(dataConfig, newDataAvailableHandler);
            _subscriptionManager.Subscribe(dataConfig);

            return enumerator;
        }

        /// <summary>
        /// Removes the specified configuration
        /// </summary>
        /// <param name="dataConfig">Subscription config to be removed</param>
        public void Unsubscribe(SubscriptionDataConfig dataConfig)
        {
            _subscriptionManager.Unsubscribe(dataConfig);
            _dataAggregator.Remove(dataConfig);
        }

        /// <summary>
        /// Sets the job we're subscribing for
        /// </summary>
        /// <param name="job">Job we're subscribing for</param>
        /// <exception cref="NotImplementedException"></exception>
        public void SetJob(Packets.LiveNodePacket job)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Dispose of unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _dataAggregator?.DisposeSafely();
            _subscriptionManager?.DisposeSafely();
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the history for the requested security
        /// </summary>
        /// <param name="request">The historical data request</param>
        /// <returns>An enumerable of BaseData points</returns>
        private IEnumerable<BaseData> GetHistory(HistoryRequest request)
        {
            if (!CanSubscribe(request.Symbol))
            {
                return null;
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if this Data provider supports the specified symbol
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <returns>returns true if Data Provider supports the specified symbol; otherwise false</returns>
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
