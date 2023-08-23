using System;
using System.Collections.Generic;

namespace fwmonitor.DB;

public partial class Component
{
    public int Id { get; set; }

    public int? Uhistoryid { get; set; }

    public string Componentid { get; set; }

    public string Componenttype { get; set; }

    public string Componenteol { get; set; }

    public string Componentname { get; set; }

    public string Summary { get; set; }

    public string Url { get; set; }

    public string Projectlicense { get; set; }

    public string Developername { get; set; }

    public string[] Categories { get; set; }

    public virtual ICollection<Componentsrelease> Componentsreleases { get; set; } = new List<Componentsrelease>();

    public virtual ICollection<Downloadhistory> Downloadhistories { get; set; } = new List<Downloadhistory>();

    public virtual ICollection<Releasesfile> Releasesfiles { get; set; } = new List<Releasesfile>();

    public virtual Uhistory Uhistory { get; set; }
}
