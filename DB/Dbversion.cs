using System;
using System.Collections.Generic;

namespace fwmonitor.DB;

public partial class Dbversion
{
    public int Value { get; set; }

    public long? ReferenceOperationStamp { get; set; }

    public byte[] ShardingConfiguration { get; set; }

    public Guid? ShardId { get; set; }

    public Guid? ShardingId { get; set; }
}
