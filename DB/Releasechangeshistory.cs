using System;
using System.Collections.Generic;

namespace fwmonitor.DB;

public partial class Releasechangeshistory
{
    public int Id { get; set; }

    public int? Uhistoryidcur { get; set; }

    public int? Uhistoryidprev { get; set; }

    public string Componentname { get; set; }

    public DateTime? Releasetimestamp { get; set; }

    public string Releaseversion { get; set; }

    public string Releasedescription { get; set; }

    public string Operationtype { get; set; }

    public virtual Uhistory UhistoryidcurNavigation { get; set; }

    public virtual Uhistory UhistoryidprevNavigation { get; set; }
}
