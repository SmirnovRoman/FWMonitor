using System;
using System.Collections.Generic;

namespace fwmonitor.DB;

public partial class Uhistory
{
    public int Id { get; set; }

    public DateTime? D2 { get; set; }

    public string Status { get; set; }

    public string Ipused { get; set; }

    public string[] Ipavailable { get; set; }

    public int? Sizeooffile { get; set; }

    public string Source { get; set; }

    public string Info { get; set; }

    public virtual ICollection<Component> Components { get; set; } = new List<Component>();

    public virtual ICollection<Componentschangeshistory> ComponentschangeshistoryUhistoryidcurNavigations { get; set; } = new List<Componentschangeshistory>();

    public virtual ICollection<Componentschangeshistory> ComponentschangeshistoryUhistoryidprevNavigations { get; set; } = new List<Componentschangeshistory>();

    public virtual ICollection<Componentsrelease> Componentsreleases { get; set; } = new List<Componentsrelease>();

    public virtual ICollection<Downloadhistory> Downloadhistories { get; set; } = new List<Downloadhistory>();

    public virtual ICollection<Releasechangeshistory> ReleasechangeshistoryUhistoryidcurNavigations { get; set; } = new List<Releasechangeshistory>();

    public virtual ICollection<Releasechangeshistory> ReleasechangeshistoryUhistoryidprevNavigations { get; set; } = new List<Releasechangeshistory>();

    public virtual ICollection<Releasesfile> Releasesfiles { get; set; } = new List<Releasesfile>();
}
