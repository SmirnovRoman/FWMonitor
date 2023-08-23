using System;
using System.Collections.Generic;

namespace fwmonitor.DB;

public partial class EntityDatum
{
    public int EntityId { get; set; }

    public int ParameterId { get; set; }

    public DateTime TimeStamp { get; set; }

    public int SourceId { get; set; }

    public byte[] Value { get; set; }

    public byte[] StatusesId { get; set; }

    public byte[] RcvStamp { get; set; }
}
