using System;
using System.Collections.Generic;

namespace fwmonitor.DB;

public partial class EntityEvent
{
    public long Id { get; set; }

    public int EntityId { get; set; }

    public int EventId { get; set; }

    public DateTime TimeStamp { get; set; }

    public int SourceId { get; set; }

    public byte[] Parameters { get; set; }

    public int ParametersHash { get; set; }

    public DateTime? RcvStamp { get; set; }
}
