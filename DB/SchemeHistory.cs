using System;
using System.Collections.Generic;

namespace fwmonitor.DB;

public partial class SchemeHistory
{
    public long Id { get; set; }

    public byte[] Scheme { get; set; }

    public DateTime? CreationStamp { get; set; }
}
