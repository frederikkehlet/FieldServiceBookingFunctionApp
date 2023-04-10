using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SendMessageToSBQPlugin.Models
{
    public class Reservation
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string WorkOrderNumber { get; set; }
        public Guid ResourceUserId { get; set; }
        public Address Address { get; set; }
    }

    public class Address
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
    }
}
