using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Models
{
    public class Repsonse
    {
        public string statusCode { get; set; }
        public string message { get; set; }
        public object? developerMessage { get; set; }
        public object? data { get; set; }
    }
}