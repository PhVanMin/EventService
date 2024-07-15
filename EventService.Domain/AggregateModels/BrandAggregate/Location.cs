using EventService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventService.Domain.AggregateModels.BrandAggregate
{
    public class Location : ValueObject
    {
        public Location(string address, string gps)
        {
            Address = address;
            Gps = gps;
        }

        public string Address { get; private set; } = null!;
        public string Gps { get; private set; } = null!;
    }
}
