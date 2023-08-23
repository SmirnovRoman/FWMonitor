using System;
using System.Collections.Generic;

namespace fwmonitor.DB;

public partial class EntityAttributesHistory
{
    public long Id { get; set; }

    public int EntityId { get; set; }

    public int AttributeEntityId { get; set; }

    public int? SourceId { get; set; }

    public byte[] Value { get; set; }

    public string ValueStr { get; set; }

    public byte[] CurrentValue { get; set; }

    public string CurrentValueStr { get; set; }

    public DateTime OperationStamp { get; set; }
}
