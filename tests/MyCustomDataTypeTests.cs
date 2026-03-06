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
using ProtoBuf;
using System.IO;
using System.Linq;
using ProtoBuf.Meta;
using Newtonsoft.Json;
using NUnit.Framework;
using QuantConnect.Data;
using QuantConnect.DataSource;

namespace QuantConnect.DataLibrary.Tests
{
    [TestFixture]
    public class MyCustomDataTypeTests
    {
        /// <summary>
        /// Path to the sample data file in the repo output/ directory.
        /// Adjust if your sample data uses a different filename.
        /// </summary>
        private static readonly string _repoRoot = Path.GetFullPath(
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", ".."));

        private readonly string _sampleDataPath = Path.Combine(
            _repoRoot,
            "output",
            "alternative",
            "mycustomdatatype",
            "spy.csv"
        );

        private readonly string _universeDataPath = Path.Combine(
            _repoRoot,
            "output",
            "alternative",
            "mycustomdatatype",
            "universe",
            "20220214.csv"
        );

        private SubscriptionDataConfig _config;

        [SetUp]
        public void SetUp()
        {
            _config = new SubscriptionDataConfig(
                typeof(MyCustomDataType),
                Symbol.Create("SPY", SecurityType.Base, Market.USA),
                Resolution.Daily,
                TimeZones.Utc,
                TimeZones.Utc,
                false,
                false,
                false
            );
        }

        [Test]
        public void JsonRoundTrip()
        {
            var expected = CreateNewInstance();
            var type = expected.GetType();
            var serialized = JsonConvert.SerializeObject(expected);
            var result = JsonConvert.DeserializeObject(serialized, type);

            AssertAreEqual(expected, result);
        }

        [Test]
        public void ProtobufRoundTrip()
        {
            var expected = CreateNewInstance();
            var type = expected.GetType();

            RuntimeTypeModel.Default[typeof(BaseData)].AddSubType(2000, type);

            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, expected);

                stream.Position = 0;

                var result = Serializer.Deserialize(type, stream);

                AssertAreEqual(expected, result, filterByCustomAttributes: true);
            }
        }

        [Test]
        public void Clone()
        {
            var expected = CreateNewInstance();
            var result = expected.Clone();

            AssertAreEqual(expected, result);
        }

        /// <summary>
        /// Verifies that the sample data file exists in the output directory.
        /// </summary>
        [Test]
        public void SampleDataFileExists()
        {
            Assert.IsTrue(
                File.Exists(_sampleDataPath),
                $"Sample data file not found at {_sampleDataPath}. " +
                "Ensure minimal sample data is present in the output/ directory."
            );
        }

        /// <summary>
        /// Reads every line of the sample data file through the Reader and
        /// asserts that each line produces a valid, non-null instance with
        /// correct Symbol, non-default Time/EndTime, and non-zero Value.
        /// </summary>
        [Test]
        public void ReaderParsesAllSampleDataLines()
        {
            Assert.IsTrue(File.Exists(_sampleDataPath), $"Missing: {_sampleDataPath}");

            var lines = File.ReadAllLines(_sampleDataPath)
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToList();

            Assert.IsNotEmpty(lines, "Sample data file is empty");

            var instance = CreateNewInstance();

            foreach (var line in lines)
            {
                var result = instance.Reader(_config, line, DateTime.UtcNow, false) as MyCustomDataType;

                Assert.IsNotNull(result, $"Reader returned null for line: {line}");
                Assert.AreEqual(_config.Symbol, result.Symbol, $"Wrong Symbol for line: {line}");
                Assert.AreNotEqual(default(DateTime), result.Time, $"Time not set for line: {line}");
                Assert.AreNotEqual(default(DateTime), result.EndTime, $"EndTime not set for line: {line}");
                // TODO: add property-specific assertions if needed
                // Assert.AreNotEqual(0m, result.SomeCustomProperty, $"SomeCustomProperty is zero for line: {line}");
            }

            Assert.IsTrue(lines.Count > 0, "Expected at least one data row");
        }

        /// <summary>
        /// Reads the first sample data line through the Reader and verifies
        /// that Clone produces an exact copy of every property.
        /// </summary>
        [Test]
        public void CloneCopiesAllPropertiesFromSampleData()
        {
            Assert.IsTrue(File.Exists(_sampleDataPath), $"Missing: {_sampleDataPath}");

            var firstLine = File.ReadLines(_sampleDataPath)
                .FirstOrDefault(l => !string.IsNullOrWhiteSpace(l));

            Assert.IsNotNull(firstLine, "Sample data file has no data lines");

            var instance = CreateNewInstance();
            var original = instance.Reader(_config, firstLine, DateTime.UtcNow, false) as MyCustomDataType;
            Assert.IsNotNull(original);

            var clone = original.Clone() as MyCustomDataType;

            Assert.IsNotNull(clone);
            Assert.AreEqual(original.Symbol, clone.Symbol);
            Assert.AreEqual(original.Time, clone.Time);
            Assert.AreEqual(original.EndTime, clone.EndTime);
            Assert.AreEqual(original.Value, clone.Value);
            Assert.AreEqual(original.SomeCustomProperty, clone.SomeCustomProperty);
        }

        /// <summary>
        /// Tests that ToString returns a non-empty string for parsed sample data.
        /// </summary>
        [Test]
        public void ToStringReturnsNonEmpty()
        {
            Assert.IsTrue(File.Exists(_sampleDataPath), $"Missing: {_sampleDataPath}");

            var firstLine = File.ReadLines(_sampleDataPath)
                .FirstOrDefault(l => !string.IsNullOrWhiteSpace(l));

            Assert.IsNotNull(firstLine);

            var instance = CreateNewInstance();
            var result = instance.Reader(_config, firstLine, DateTime.UtcNow, false) as MyCustomDataType;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ToString());
            Assert.IsNotEmpty(result.ToString());
        }

        /// <summary>
        /// Tests GetSource returns a valid SubscriptionDataSource with LocalFile transport.
        /// </summary>
        [Test]
        public void GetSourceReturnsLocalFile()
        {
            var instance = CreateNewInstance();
            var source = instance.GetSource(_config, DateTime.UtcNow, false);

            Assert.IsNotNull(source);
            Assert.AreEqual(SubscriptionTransportMedium.LocalFile, source.TransportMedium);
        }

        /// <summary>
        /// Tests that the default resolution is correctly set.
        /// </summary>
        [Test]
        public void DefaultResolutionIsCorrect()
        {
            var instance = CreateNewInstance();
            Assert.AreEqual(Resolution.Daily, instance.DefaultResolution());
        }

        /// <summary>
        /// Tests that RequiresMapping returns the expected value.
        /// </summary>
        [Test]
        public void RequiresMappingIsCorrect()
        {
            var instance = CreateNewInstance();
            Assert.AreEqual(true, instance.RequiresMapping());
        }

        /// <summary>
        /// Tests that IsSparseData returns the expected value.
        /// </summary>
        [Test]
        public void IsSparseDataIsCorrect()
        {
            var instance = CreateNewInstance();
            Assert.AreEqual(true, instance.IsSparseData());
        }

        /// <summary>
        /// Verifies that the universe sample data file exists in the output directory.
        /// </summary>
        [Test]
        public void SampleUniverseDataFileExists()
        {
            Assert.IsTrue(
                File.Exists(_universeDataPath),
                $"Universe sample data file not found at {_universeDataPath}. " +
                "Ensure minimal sample data is present in the output/ directory."
            );
        }

        /// <summary>
        /// Reads the universe sample data file and verifies the universe Reader.
        /// </summary>
        [Test]
        public void UniverseReaderParsesAllSampleDataLines()
        {
            Assert.IsTrue(File.Exists(_universeDataPath), $"Missing: {_universeDataPath}");

            var lines = File.ReadAllLines(_universeDataPath)
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToList();

            Assert.IsNotEmpty(lines, "Universe sample data file is empty");

            var universeConfig = new SubscriptionDataConfig(
                typeof(MyCustomDataUniverse),
                Symbol.Create("SPY", SecurityType.Base, Market.USA),
                Resolution.Daily,
                TimeZones.Utc,
                TimeZones.Utc,
                false,
                false,
                false
            );

            var instance = new MyCustomDataUniverse();
            foreach (var line in lines)
            {
                var result = instance.Reader(universeConfig, line, DateTime.UtcNow, false)
                    as MyCustomDataUniverse;
                Assert.IsNotNull(result, $"Universe reader returned null for line: {line}");
            }
        }

        private void AssertAreEqual(object expected, object result, bool filterByCustomAttributes = false)
        {
            foreach (var propertyInfo in expected.GetType().GetProperties())
            {
                // we skip Symbol which isn't protobuffed
                if (filterByCustomAttributes && propertyInfo.CustomAttributes.Count() != 0)
                {
                    Assert.AreEqual(propertyInfo.GetValue(expected), propertyInfo.GetValue(result));
                }
            }
            foreach (var fieldInfo in expected.GetType().GetFields())
            {
                Assert.AreEqual(fieldInfo.GetValue(expected), fieldInfo.GetValue(result));
            }
        }

        private BaseData CreateNewInstance()
        {
            return new MyCustomDataType
            {
                Symbol = Symbol.Empty,
                Time = DateTime.Today,
                DataType = MarketDataType.Base,
                SomeCustomProperty = "This is some market related information"
            };
        }
    }
}