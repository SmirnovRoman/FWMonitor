using System;
using System.Collections.Generic;

namespace fwmonitor.DB;

public partial class Componentschangeshistory
{
    public int Id { get; set; }

    public int? Uhistoryidcur { get; set; }

    public int? Uhistoryidprev { get; set; }

    public string Componentid { get; set; }

    public string Operationtype { get; set; }

    public virtual Uhistory UhistoryidcurNavigation { get; set; }

    public virtual Uhistory UhistoryidprevNavigation { get; set; }
}
