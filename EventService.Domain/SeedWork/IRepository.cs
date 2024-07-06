﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventService.Domain.Interfaces {
    public interface IRepository<T> where T : IAggregateRoot {
        IUnitOfWork UnitOfWork { get; }
    }
}