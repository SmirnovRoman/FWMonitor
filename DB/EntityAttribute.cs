using System;
using System.Collections.Generic;

namespace fwmonitor.DB;

public partial class EntityAttribute
{
    public long Id { get; set; }

    public int EntityId { get; set; }

    public int AttributeEntityId { get; set; }

    public int? SourceId { get; set; }

    public byte[] Value { get; set; }

    public string ValueStr { get; set; }

    public byte[] ValueBlob { get; set; }

    public DateTime? ModifyStamp { get; set; }
}
