/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QuantConnect.Configuration;

namespace QuantConnect.DataSource
{
    /// <summary>
    /// Raw JSON client for FXMacroData's public read/data endpoints.
    /// </summary>
    public sealed class FxMacroDataClient : IDisposable
    {
        public const string DefaultBaseUrl = "https://fxmacrodata.com/api/v1";

        private readonly HttpClient _httpClient;
        private readonly bool _ownsClient;
        private readonly string _baseUrl;
        private readonly string _apiKey;

        public FxMacroDataClient(HttpClient httpClient = null, string baseUrl = DefaultBaseUrl, string apiKey = null)
        {
            _httpClient = httpClient ?? new HttpClient();
            _ownsClient = httpClient == null;
            _baseUrl = baseUrl.EndsWith("/") ? baseUrl.Substring(0, baseUrl.Length - 1) : baseUrl;
            _apiKey = string.IsNullOrWhiteSpace(apiKey) ? Config.Get("fxmacrodata-api-key") : apiKey;
        }

        public Task<string> GetDataCatalogueAsync(string currency = "usd", bool includeCapabilities = false,
            bool includeCoverage = false, string indicator = null)
        {
            return GetAsync($"data_catalogue/{currency.ToLowerInvariant()}", new Dictionary<string, object>
            {
                ["include_capabilities"] = includeCapabilities,
                ["include_coverage"] = includeCoverage,
                ["indicator"] = indicator
            });
        }

        public Task<string> GetAnnouncementsAsync(string currency, string indicator, DateTime? startDate = null,
            DateTime? endDate = null, int limit = 20, int offset = 0, string seriesMode = "raw",
            string revisions = "latest", bool officialOnly = true)
        {
            return GetAsync($"announcements/{currency.ToLowerInvariant()}/{indicator}", new Dictionary<string, object>
            {
                ["start_date"] = FormatDate(startDate),
                ["end_date"] = FormatDate(endDate),
                ["series_mode"] = seriesMode,
                ["limit"] = limit,
                ["offset"] = offset,
                ["revisions"] = revisions,
                ["official_only"] = officialOnly
            });
        }

        public Task<string> GetLatestAnnouncementsAsync(string currency = "usd")
        {
            return GetAsync($"announcements/{currency.ToLowerInvariant()}/latest");
        }

        public Task<string> GetAnnouncementChangesAsync(string currencies = null, string indicators = null,
            string since = null, int limit = 100, string payload = "compact")
        {
            return GetAsync("announcements/changes", new Dictionary<string, object>
            {
                ["currencies"] = currencies,
                ["indicators"] = indicators,
                ["since"] = since,
                ["limit"] = limit,
                ["payload"] = payload
            });
        }

        public Task<string> GetPredictionsAsync(string currency, string indicator, DateTime? startDate = null,
            DateTime? endDate = null, int limit = 20, int offset = 0, string predictionType = null,
            string predictionSource = null)
        {
            return GetAsync($"predictions/{currency.ToLowerInvariant()}/{indicator}", new Dictionary<string, object>
            {
                ["prediction_type"] = predictionType,
                ["prediction_source"] = predictionSource,
                ["start_date"] = FormatDate(startDate),
                ["end_date"] = FormatDate(endDate),
                ["limit"] = limit,
                ["offset"] = offset
            });
        }

        public Task<string> GetReleaseCalendarAsync(string currency = "usd", string indicator = null,
            DateTime? startDate = null, DateTime? endDate = null, string timezone = null)
        {
            return GetAsync($"calendar/{currency.ToLowerInvariant()}", new Dictionary<string, object>
            {
                ["indicator"] = indicator,
                ["start_date"] = FormatDate(startDate),
                ["end_date"] = FormatDate(endDate),
                ["timezone"] = timezone
            });
        }

        public Task<string> GetForexAsync(string baseCurrency, string quoteCurrency, DateTime? startDate = null,
            DateTime? endDate = null, int limit = 20, int offset = 0, string indicators = null)
        {
            return GetAsync($"forex/{baseCurrency.ToLowerInvariant()}/{quoteCurrency.ToLowerInvariant()}",
                new Dictionary<string, object>
                {
                    ["start_date"] = FormatDate(startDate),
                    ["end_date"] = FormatDate(endDate),
                    ["limit"] = limit,
                    ["offset"] = offset,
                    ["indicators"] = indicators
                });
        }

        public Task<string> GetCotAsync(string currency, DateTime? startDate = null, DateTime? endDate = null,
            int limit = 20, int offset = 0)
        {
            return GetAsync($"cot/{currency.ToLowerInvariant()}", new Dictionary<string, object>
            {
                ["start_date"] = FormatDate(startDate),
                ["end_date"] = FormatDate(endDate),
                ["limit"] = limit,
                ["offset"] = offset
            });
        }

        public Task<string> GetCommodityAsync(string indicator, DateTime? startDate = null, DateTime? endDate = null,
            int limit = 20, int offset = 0)
        {
            return GetAsync($"commodities/{indicator}", new Dictionary<string, object>
            {
                ["start_date"] = FormatDate(startDate),
                ["end_date"] = FormatDate(endDate),
                ["limit"] = limit,
                ["offset"] = offset
            });
        }

        public Task<string> GetLatestCommoditiesAsync()
        {
            return GetAsync("commodities/latest");
        }

        public Task<string> GetCurvesAsync(string currency, string curveFamily = "government_nominal",
            string metric = "spot", DateTime? date = null)
        {
            return GetAsync($"curves/{currency.ToLowerInvariant()}", new Dictionary<string, object>
            {
                ["curve_family"] = curveFamily,
                ["metric"] = metric,
                ["date"] = FormatDate(date)
            });
        }

        public Task<string> GetCurveProxiesAsync(string currency, string curveFamily = "government_nominal",
            DateTime? date = null)
        {
            return GetAsync($"curve_proxies/{currency.ToLowerInvariant()}", new Dictionary<string, object>
            {
                ["curve_family"] = curveFamily,
                ["date"] = FormatDate(date)
            });
        }

        public Task<string> GetForwardCurvesAsync(string currency, string curveFamily = "government_nominal",
            string method = "derived_from_spot_nodes", DateTime? date = null)
        {
            return GetAsync($"forward_curves/{currency.ToLowerInvariant()}", new Dictionary<string, object>
            {
                ["curve_family"] = curveFamily,
                ["method"] = method,
                ["date"] = FormatDate(date)
            });
        }

        public Task<string> GetRateDifferentialsAsync(string baseCurrency, string quoteCurrency, string measure = "auto",
            DateTime? startDate = null, DateTime? endDate = null, int limit = 20, int offset = 0)
        {
            return GetAsync($"rate_differentials/{baseCurrency.ToLowerInvariant()}/{quoteCurrency.ToLowerInvariant()}",
                new Dictionary<string, object>
                {
                    ["measure"] = measure,
                    ["start_date"] = FormatDate(startDate),
                    ["end_date"] = FormatDate(endDate),
                    ["limit"] = limit,
                    ["offset"] = offset
                });
        }

        public Task<string> GetForwardDifferentialsAsync(string baseCurrency, string quoteCurrency,
            string curveFamily = "government_nominal", decimal startTenorYears = 2m, decimal endTenorYears = 5m,
            DateTime? startDate = null, DateTime? endDate = null, int limit = 20, int offset = 0)
        {
            return GetAsync($"forward_differentials/{baseCurrency.ToLowerInvariant()}/{quoteCurrency.ToLowerInvariant()}",
                new Dictionary<string, object>
                {
                    ["curve_family"] = curveFamily,
                    ["start_tenor_years"] = startTenorYears,
                    ["end_tenor_years"] = endTenorYears,
                    ["start_date"] = FormatDate(startDate),
                    ["end_date"] = FormatDate(endDate),
                    ["limit"] = limit,
                    ["offset"] = offset
                });
        }

        public Task<string> GetMarketSessionsAsync(string at = null)
        {
            return GetAsync("market_sessions", new Dictionary<string, object> { ["at"] = at });
        }

        public Task<string> GetRiskSentimentAsync(DateTime? startDate = null, DateTime? endDate = null,
            int limit = 20, int offset = 0)
        {
            return GetAsync("risk_sentiment", new Dictionary<string, object>
            {
                ["start_date"] = FormatDate(startDate),
                ["end_date"] = FormatDate(endDate),
                ["limit"] = limit,
                ["offset"] = offset
            });
        }

        public Task<string> GetNewsAsync(string currency, int limit = 20, int offset = 0)
        {
            return GetAsync($"news/{currency.ToLowerInvariant()}", new Dictionary<string, object>
            {
                ["limit"] = limit,
                ["offset"] = offset
            });
        }

        public Task<string> GetPressReleasesAsync(string currency, int limit = 20, int offset = 0)
        {
            return GetAsync($"press-releases/{currency.ToLowerInvariant()}", new Dictionary<string, object>
            {
                ["limit"] = limit,
                ["offset"] = offset
            });
        }

        public async Task<string> GraphQlAsync(string query, object variables = null)
        {
            var payload = JsonConvert.SerializeObject(new
            {
                query,
                variables = variables ?? new { }
            });
            using var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}/graphql", content).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        public void Dispose()
        {
            if (_ownsClient)
            {
                _httpClient.Dispose();
            }
        }

        private Task<string> GetAsync(string path, IDictionary<string, object> query = null)
        {
            return _httpClient.GetStringAsync(BuildUri(path, query));
        }

        private Uri BuildUri(string path, IDictionary<string, object> query)
        {
            var parameters = new Dictionary<string, object>(query ?? new Dictionary<string, object>());
            if (!string.IsNullOrWhiteSpace(_apiKey))
            {
                parameters["api_key"] = _apiKey;
            }
            var url = $"{_baseUrl}/{path.TrimStart('/')}";
            var queryString = string.Join("&", parameters
                .Where(pair => pair.Value != null)
                .Select(pair => $"{Uri.EscapeDataString(pair.Key)}={Uri.EscapeDataString(FormatValue(pair.Value))}"));
            return new Uri(string.IsNullOrWhiteSpace(queryString) ? url : $"{url}?{queryString}");
        }

        private static string FormatDate(DateTime? value)
        {
            return value?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private static string FormatValue(object value)
        {
            return value switch
            {
                bool boolean => boolean ? "true" : "false",
                DateTime dateTime => dateTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                IFormattable formattable => formattable.ToString(null, CultureInfo.InvariantCulture),
                _ => value.ToString()
            };
        }
    }
}

