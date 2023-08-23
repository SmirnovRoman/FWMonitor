using System;
using System.Collections.Generic;

namespace fwmonitor.DB;

public partial class Releasesfile
{
    public int Id { get; set; }

    public int? Uhistoryid { get; set; }

    public int? Componentid { get; set; }

    public int? Releaseid { get; set; }

    public string Checksumtype { get; set; }

    public string Checksumfilename { get; set; }

    public string Checksumtarget { get; set; }

    public string Checksumvalue { get; set; }

    public virtual Component Component { get; set; }

    public virtual Componentsrelease Release { get; set; }

    public virtual Uhistory Uhistory { get; set; }
}
