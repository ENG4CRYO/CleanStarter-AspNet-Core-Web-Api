using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStarter.Core.Entities.BaseEntity
{
    public interface IEntity<TId>
    {
        TId Id { get; set; }
    }
}
