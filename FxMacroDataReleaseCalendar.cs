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
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NodaTime;
using QuantConnect.Configuration;
using QuantConnect.Data;

namespace QuantConnect.DataSource
{
    /// <summary>
    /// FXMacroData economic release calendar data. Subscribe with a currency symbol
    /// such as "USD", "EUR", or "JPY" to receive scheduled macro and central-bank
    /// events for event-aware algorithms.
    /// </summary>
    public class FxMacroDataReleaseCalendar : BaseData
    {
        /// <summary>
        /// Time when the release became available.
        /// </summary>
        public override DateTime EndTime { get; set; }

        public string Release { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public int? MarketTier { get; set; }
        public bool TopTierForCurrency { get; set; }
        public string Source { get; set; }
        public string SourceUrl { get; set; }

        public override SubscriptionDataSource GetSource(SubscriptionDataConfig config, DateTime date, bool isLiveMode)
        {
            var currency = config.Symbol.Value.ToLowerInvariant();
            var source = $"https://fxmacrodata.com/api/v1/calendar/{currency}?limit=100";
            var apiKey = Config.Get("fxmacrodata-api-key");
            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                source += $"&api_key={Uri.EscapeDataString(apiKey)}";
            }
            return new SubscriptionDataSource(source, SubscriptionTransportMedium.RemoteFile, FileFormat.UnfoldingCollection);
        }

        public override BaseData Reader(SubscriptionDataConfig config, string line, DateTime date, bool isLiveMode)
        {
            var payload = JsonConvert.DeserializeObject<CalendarPayload>(line);
            var entries = (payload?.Data ?? new List<CalendarRow>()).Select(row => new FxMacroDataReleaseCalendar
            {
                Symbol = config.Symbol,
                Time = row.Date.Date,
                EndTime = ResolveEndTime(row),
                Release = row.Release,
                Name = row.Name,
                Currency = row.Currency,
                MarketTier = row.MarketTier,
                TopTierForCurrency = row.TopTierForCurrency,
                Source = row.Source,
                SourceUrl = row.SourceUrl,
                Value = row.MarketTier ?? 0
            });

            return new BaseDataCollection(date, config.Symbol, entries);
        }

        public override BaseData Clone()
        {
            return new FxMacroDataReleaseCalendar
            {
                Symbol = Symbol,
                Time = Time,
                EndTime = EndTime,
                Release = Release,
                Name = Name,
                Currency = Currency,
                MarketTier = MarketTier,
                TopTierForCurrency = TopTierForCurrency,
                Source = Source,
                SourceUrl = SourceUrl,
                Value = Value
            };
        }

        public override bool RequiresMapping()
        {
            return false;
        }

        public override bool IsSparseData()
        {
            return true;
        }

        public override Resolution DefaultResolution()
        {
            return Resolution.Daily;
        }

        public override List<Resolution> SupportedResolutions()
        {
            return DailyResolution;
        }

        public override DateTimeZone DataTimeZone()
        {
            return DateTimeZone.Utc;
        }

        private static DateTime ResolveEndTime(CalendarRow row)
        {
            if (row.AnnouncementDateTimeUtc.HasValue)
            {
                return DateTime.SpecifyKind(row.AnnouncementDateTimeUtc.Value, DateTimeKind.Utc);
            }
            if (row.AnnouncementUnix.HasValue)
            {
                return DateTimeOffset.FromUnixTimeSeconds(row.AnnouncementUnix.Value).UtcDateTime;
            }
            return DateTime.SpecifyKind(row.Date.Date, DateTimeKind.Utc);
        }

        private sealed class CalendarPayload
        {
            [JsonProperty("data")]
            public List<CalendarRow> Data { get; set; } = new List<CalendarRow>();
        }

        private sealed class CalendarRow
        {
            [JsonProperty("date")]
            public DateTime Date { get; set; }

            [JsonProperty("release")]
            public string Release { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("currency")]
            public string Currency { get; set; }

            [JsonProperty("market_tier")]
            public int? MarketTier { get; set; }

            [JsonProperty("top_tier_for_currency")]
            public bool TopTierForCurrency { get; set; }

            [JsonProperty("announcement_datetime")]
            public long? AnnouncementUnix { get; set; }

            [JsonProperty("announcement_datetime_utc")]
            public DateTime? AnnouncementDateTimeUtc { get; set; }

            [JsonProperty("source")]
            public string Source { get; set; }

            [JsonProperty("source_url")]
            public string SourceUrl { get; set; }
        }
    }
}
