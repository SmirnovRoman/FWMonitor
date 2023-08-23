using System;
using System.Collections.Generic;

namespace fwmonitor.DB;

public partial class Entity
{
    public int Id { get; set; }

    public Guid Guid { get; set; }

    public int? SourceId { get; set; }

    public DateTime? CreationStamp { get; set; }
}
