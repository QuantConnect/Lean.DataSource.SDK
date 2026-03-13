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

### <summary>
### Example algorithm using the custom data type as a source of alpha
### </summary>
class CustomDataUniverse(QCAlgorithm):

    def initialize(self):
        ''' Initialise the data and resolution required, as well as the cash and start-end dates for your algorithm. All algorithms must initialized. '''

        # Data ADDED via universe selection is added with Daily resolution.
        self.universe_settings.resolution = Resolution.DAILY

        self.set_start_date(2022, 2, 14)
        self.set_end_date(2022, 2, 18)
        self.set_cash(100000)

        # add a custom universe data source (defaults to usa-equity)
        universe = self.add_universe(MyCustomDataUniverse, self._universe_selection)

        history = self.history(universe, 1)
        if len(history) != 1:
            raise ValueError(f"Unexpected history count {len(history)}! Expected 1")

        for data_for_date in history:
            if len(data_for_date) < 300:
                raise ValueError(f"Unexpected historical universe data!")

    def _universe_selection(self, data):
        ''' Selected the securities
        
        :param List of MyCustomUniverseType data: List of MyCustomUniverseType
        :return: List of Symbol objects '''

        for datum in data:
            self.log(f"{datum.symbol},{datum.some_numeric_property},{datum.some_custom_property}")
        
        # define our selection criteria
        return [d.symbol for d in data if d.some_custom_property == 'buy']

    def on_securities_changed(self, changes):
        ''' Event fired each time that we add/remove securities from the data feed
		
        :param SecurityChanges changes: Security additions/removals for this time step
        '''
        self.log(changes.to_string())