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
class CustomDataAlgorithm(QCAlgorithm):
    
    def initialize(self):
        ''' Initialise the data and resolution required, as well as the cash and start-end dates for your algorithm. All algorithms must initialized.'''
        self.set_start_date(2013, 10, 7)
        self.set_end_date(2013, 10, 11)
        self._equity_symbol = self.add_equity("SPY", Resolution.DAILY).symbol
        self._custom_data_symbol = self.add_data(MyCustomDataType, self._equity_symbol).symbol

    def on_data(self, slice):
        ''' OnData event is the primary entry point for your algorithm. Each new data point will be pumped in here.

        :param Slice slice: Slice object keyed by symbol containing the stock data
        '''
        custom_data = slice.get(MyCustomDataType)
        if custom_data:
            point = custom_data[self._custom_data_symbol]
            if point.some_custom_property == "buy":
                self.set_holdings(self._equity_symbol, 1)
            elif point.some_custom_property == "sell":
                self.set_holdings(self._equity_symbol, -1)

    def on_order_event(self, order_event):
        ''' Order fill event handler. On an order fill update the resulting information is passed to this method.

        :param OrderEvent order_event: Order event details containing details of the events
        '''
        if order_event.status == OrderStatus.FILLED:
            self.debug(f'Purchased Stock: {order_event.symbol}')
    
