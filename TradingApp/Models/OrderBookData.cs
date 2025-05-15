using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingApp.Models
{
    public class OrderBookData
    {
        public List<OrderBookItem> Bids { get; set; } = new();
        public List<OrderBookItem> Asks { get; set; } = new();
    }

    public class OrderBookItem
    {
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
    }
}
