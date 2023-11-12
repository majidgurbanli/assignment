using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment
{
    internal class Record
    {
        public DateTime PickupDateTime { get; set; }
        public DateTime DropoffDateTime { get; set; }
        public int PassengerCount { get; set; }
        public double TripDistance { get; set; }
        public string StoreAndForwardFlag { get; set; }
        public int PULocationID { get; set; }
        public int DOLocationID { get; set; }
        public decimal FareAmount { get; set; }
        public decimal TipAmount { get; set; }
    }
}
