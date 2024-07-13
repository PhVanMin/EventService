using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventService.Domain.Exceptions {
    public class AddEventException : Exception {
        public AddEventException() { }
        public AddEventException(string message) : base(message) {}
    }
}
