# QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
# Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.

from AlgorithmImports import *
from QuantConnect.DataSource import *


class FxMacroDataReleaseCalendarAlgorithm(QCAlgorithm):
    def initialize(self):
        self.set_start_date(2026, 1, 1)
        self.set_end_date(2026, 12, 31)
        self.set_cash(100000)

        self.spy = self.add_equity("SPY", Resolution.DAILY).symbol
        self.usd_calendar = self.add_data(
            FxMacroDataReleaseCalendar, "USD", Resolution.DAILY
        ).symbol

    def on_data(self, data: Slice):
        event = data.get(FxMacroDataReleaseCalendar, self.usd_calendar)
        if event is None:
            return

        self.debug(
            f"{self.time.date()} {event.name} tier={event.market_tier} "
            f"source={event.source}"
        )
        if event.market_tier == 1:
            self.set_holdings(self.spy, 0.25)
