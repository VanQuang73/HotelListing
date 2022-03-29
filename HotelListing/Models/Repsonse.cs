using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Models
{
    public class Repsonse
    {
        public bool statusCode { get; set; }
        public string message { get; set; }
        public List<string>? developerMessage { get; set; }
        public object? data { get; set; }
    }
}
