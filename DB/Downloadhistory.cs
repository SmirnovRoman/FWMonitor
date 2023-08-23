using System;
using System.Collections.Generic;

namespace fwmonitor.DB;

public partial class Downloadhistory
{
    public int Id { get; set; }

    public int? Uhistoryid { get; set; }

    public int? Componentid { get; set; }

    public string Releaseid { get; set; }

    public string Releaseversion { get; set; }

    public DateTime? Releasetimestamp { get; set; }

    public string Urgency { get; set; }

    public string Releaselocation { get; set; }

    public string Releasedescription { get; set; }

    public int? Sizeinstalled { get; set; }

    public int? Sizedownload { get; set; }

    public DateTime? Downloaded { get; set; }

    public string Localfilename { get; set; }

    public virtual Component Component { get; set; }

    public virtual Uhistory Uhistory { get; set; }
}
