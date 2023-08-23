using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace fwmonitor.DB;

public partial class uiContext : DbContext
{
    public uiContext()
    {
    }

    public uiContext(DbContextOptions<uiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Component> Components { get; set; }

    public virtual DbSet<Componentschangeshistory> Componentschangeshistories { get; set; }

    public virtual DbSet<Componentsrelease> Componentsreleases { get; set; }

    public virtual DbSet<Dbversion> Dbversions { get; set; }

    public virtual DbSet<Downloadhistory> Downloadhistories { get; set; }

    public virtual DbSet<Entity> Entities { get; set; }

    public virtual DbSet<EntityAttribute> EntityAttributes { get; set; }

    public virtual DbSet<EntityAttributesHistory> EntityAttributesHistories { get; set; }

    public virtual DbSet<EntityDatum> EntityData { get; set; }

    public virtual DbSet<EntityEvent> EntityEvents { get; set; }

    public virtual DbSet<Releasechangeshistory> Releasechangeshistories { get; set; }

    public virtual DbSet<Releasesfile> Releasesfiles { get; set; }

    public virtual DbSet<SchemeHistory> SchemeHistories { get; set; }

    public virtual DbSet<Uhistory> Uhistories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql(Program.S.ConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Component>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("components_pkey");

            entity.ToTable("components");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Categories).HasColumnName("categories");
            entity.Property(e => e.Componenteol).HasColumnName("componenteol");
            entity.Property(e => e.Componentid).HasColumnName("componentid");
            entity.Property(e => e.Componentname).HasColumnName("componentname");
            entity.Property(e => e.Componenttype).HasColumnName("componenttype");
            entity.Property(e => e.Developername).HasColumnName("developername");
            entity.Property(e => e.Projectlicense).HasColumnName("projectlicense");
            entity.Property(e => e.Summary).HasColumnName("summary");
            entity.Property(e => e.Uhistoryid).HasColumnName("uhistoryid");
            entity.Property(e => e.Url).HasColumnName("url");

            entity.HasOne(d => d.Uhistory).WithMany(p => p.Components)
                .HasForeignKey(d => d.Uhistoryid)
                .HasConstraintName("components_uhistoryid_fkey");
        });

        modelBuilder.Entity<Componentschangeshistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("componentschangeshistory_pkey");

            entity.ToTable("componentschangeshistory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Componentid).HasColumnName("componentid");
            entity.Property(e => e.Operationtype).HasColumnName("operationtype");
            entity.Property(e => e.Uhistoryidcur).HasColumnName("uhistoryidcur");
            entity.Property(e => e.Uhistoryidprev).HasColumnName("uhistoryidprev");

            entity.HasOne(d => d.UhistoryidcurNavigation).WithMany(p => p.ComponentschangeshistoryUhistoryidcurNavigations)
                .HasForeignKey(d => d.Uhistoryidcur)
                .HasConstraintName("componentschangeshistory_uhistoryidcur_fkey");

            entity.HasOne(d => d.UhistoryidprevNavigation).WithMany(p => p.ComponentschangeshistoryUhistoryidprevNavigations)
                .HasForeignKey(d => d.Uhistoryidprev)
                .HasConstraintName("componentschangeshistory_uhistoryidprev_fkey");
        });

        modelBuilder.Entity<Componentsrelease>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("componentsreleases_pkey");

            entity.ToTable("componentsreleases");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Componentid).HasColumnName("componentid");
            entity.Property(e => e.Downloaded).HasColumnName("downloaded");
            entity.Property(e => e.Localfilename).HasColumnName("localfilename");
            entity.Property(e => e.Releasedescription).HasColumnName("releasedescription");
            entity.Property(e => e.Releaseid).HasColumnName("releaseid");
            entity.Property(e => e.Releaselocation).HasColumnName("releaselocation");
            entity.Property(e => e.Releasetimestamp)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("releasetimestamp");
            entity.Property(e => e.Releaseversion).HasColumnName("releaseversion");
            entity.Property(e => e.Sizedownload).HasColumnName("sizedownload");
            entity.Property(e => e.Sizeinstalled).HasColumnName("sizeinstalled");
            entity.Property(e => e.Uhistoryid).HasColumnName("uhistoryid");
            entity.Property(e => e.Urgency).HasColumnName("urgency");

            entity.HasOne(d => d.Component).WithMany(p => p.Componentsreleases)
                .HasForeignKey(d => d.Componentid)
                .HasConstraintName("componentsreleases_componentid_fkey");

            entity.HasOne(d => d.Uhistory).WithMany(p => p.Componentsreleases)
                .HasForeignKey(d => d.Uhistoryid)
                .HasConstraintName("componentsreleases_uhistoryid_fkey");
        });

        modelBuilder.Entity<Dbversion>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("dbversion");

            entity.Property(e => e.ReferenceOperationStamp).HasColumnName("reference_operation_stamp");
            entity.Property(e => e.ShardId).HasColumnName("shard_id");
            entity.Property(e => e.ShardingConfiguration).HasColumnName("sharding_configuration");
            entity.Property(e => e.ShardingId).HasColumnName("sharding_id");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        modelBuilder.Entity<Downloadhistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("downloadhistory_pkey");

            entity.ToTable("downloadhistory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Componentid).HasColumnName("componentid");
            entity.Property(e => e.Downloaded)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("downloaded");
            entity.Property(e => e.Localfilename).HasColumnName("localfilename");
            entity.Property(e => e.Releasedescription).HasColumnName("releasedescription");
            entity.Property(e => e.Releaseid).HasColumnName("releaseid");
            entity.Property(e => e.Releaselocation).HasColumnName("releaselocation");
            entity.Property(e => e.Releasetimestamp)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("releasetimestamp");
            entity.Property(e => e.Releaseversion).HasColumnName("releaseversion");
            entity.Property(e => e.Sizedownload).HasColumnName("sizedownload");
            entity.Property(e => e.Sizeinstalled).HasColumnName("sizeinstalled");
            entity.Property(e => e.Uhistoryid).HasColumnName("uhistoryid");
            entity.Property(e => e.Urgency).HasColumnName("urgency");

            entity.HasOne(d => d.Component).WithMany(p => p.Downloadhistories)
                .HasForeignKey(d => d.Componentid)
                .HasConstraintName("downloadhistory_componentid_fkey");

            entity.HasOne(d => d.Uhistory).WithMany(p => p.Downloadhistories)
                .HasForeignKey(d => d.Uhistoryid)
                .HasConstraintName("downloadhistory_uhistoryid_fkey");
        });

        modelBuilder.Entity<Entity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_entities");

            entity.ToTable("entities");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreationStamp)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("creation_stamp");
            entity.Property(e => e.Guid).HasColumnName("guid");
            entity.Property(e => e.SourceId).HasColumnName("source_id");
        });

        modelBuilder.Entity<EntityAttribute>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_entity_attributes");

            entity.ToTable("entity_attributes");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AttributeEntityId).HasColumnName("attribute_entity_id");
            entity.Property(e => e.EntityId).HasColumnName("entity_id");
            entity.Property(e => e.ModifyStamp)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modify_stamp");
            entity.Property(e => e.SourceId).HasColumnName("source_id");
            entity.Property(e => e.Value).HasColumnName("value");
            entity.Property(e => e.ValueBlob).HasColumnName("value_blob");
            entity.Property(e => e.ValueStr).HasColumnName("value_str");
        });

        modelBuilder.Entity<EntityAttributesHistory>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.OperationStamp }).HasName("pk_entity_attributes_history");

            entity.ToTable("entity_attributes_history");

            entity.HasIndex(e => new { e.AttributeEntityId, e.OperationStamp }, "ndx_entity_attributes_history_attribute_entity_id");

            entity.HasIndex(e => new { e.EntityId, e.OperationStamp }, "ndx_entity_attributes_history_entity_id");

            entity.HasIndex(e => new { e.OperationStamp, e.EntityId, e.AttributeEntityId }, "ndx_entity_attributes_history_secondary");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.OperationStamp)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("operation_stamp");
            entity.Property(e => e.AttributeEntityId).HasColumnName("attribute_entity_id");
            entity.Property(e => e.CurrentValue).HasColumnName("current_value");
            entity.Property(e => e.CurrentValueStr).HasColumnName("current_value_str");
            entity.Property(e => e.EntityId).HasColumnName("entity_id");
            entity.Property(e => e.SourceId).HasColumnName("source_id");
            entity.Property(e => e.Value).HasColumnName("value");
            entity.Property(e => e.ValueStr).HasColumnName("value_str");
        });

        modelBuilder.Entity<EntityDatum>(entity =>
        {
            entity.HasKey(e => new { e.ParameterId, e.EntityId, e.TimeStamp, e.SourceId }).HasName("pk_entity_data");

            entity.ToTable("entity_data");

            entity.HasIndex(e => new { e.EntityId, e.TimeStamp }, "ndx_entity_data_entity_id_time_stamp");

            entity.Property(e => e.ParameterId).HasColumnName("parameter_id");
            entity.Property(e => e.EntityId).HasColumnName("entity_id");
            entity.Property(e => e.TimeStamp)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("time_stamp");
            entity.Property(e => e.SourceId).HasColumnName("source_id");
            entity.Property(e => e.RcvStamp).HasColumnName("rcv_stamp");
            entity.Property(e => e.StatusesId).HasColumnName("statuses_id");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        modelBuilder.Entity<EntityEvent>(entity =>
        {
            entity.HasKey(e => new { e.EntityId, e.EventId, e.TimeStamp, e.SourceId, e.ParametersHash }).HasName("pk_entity_events");

            entity.ToTable("entity_events");

            entity.HasIndex(e => new { e.EntityId, e.TimeStamp }, "ndx_entity_events_entity_id_time_stamp");

            entity.HasIndex(e => new { e.SourceId, e.EntityId, e.TimeStamp }, "ndx_entity_events_source_id_entity_id_time_stamp");

            entity.Property(e => e.EntityId).HasColumnName("entity_id");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.TimeStamp)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("time_stamp");
            entity.Property(e => e.SourceId).HasColumnName("source_id");
            entity.Property(e => e.ParametersHash).HasColumnName("parameters_hash");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Parameters).HasColumnName("parameters");
            entity.Property(e => e.RcvStamp)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("rcv_stamp");
        });

        modelBuilder.Entity<Releasechangeshistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("releasechangeshistory_pkey");

            entity.ToTable("releasechangeshistory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Componentname).HasColumnName("componentname");
            entity.Property(e => e.Operationtype).HasColumnName("operationtype");
            entity.Property(e => e.Releasedescription).HasColumnName("releasedescription");
            entity.Property(e => e.Releasetimestamp)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("releasetimestamp");
            entity.Property(e => e.Releaseversion).HasColumnName("releaseversion");
            entity.Property(e => e.Uhistoryidcur).HasColumnName("uhistoryidcur");
            entity.Property(e => e.Uhistoryidprev).HasColumnName("uhistoryidprev");

            entity.HasOne(d => d.UhistoryidcurNavigation).WithMany(p => p.ReleasechangeshistoryUhistoryidcurNavigations)
                .HasForeignKey(d => d.Uhistoryidcur)
                .HasConstraintName("releasechangeshistory_uhistoryidcur_fkey");

            entity.HasOne(d => d.UhistoryidprevNavigation).WithMany(p => p.ReleasechangeshistoryUhistoryidprevNavigations)
                .HasForeignKey(d => d.Uhistoryidprev)
                .HasConstraintName("releasechangeshistory_uhistoryidprev_fkey");
        });

        modelBuilder.Entity<Releasesfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("releasesfiles_pkey");

            entity.ToTable("releasesfiles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Checksumfilename).HasColumnName("checksumfilename");
            entity.Property(e => e.Checksumtarget).HasColumnName("checksumtarget");
            entity.Property(e => e.Checksumtype).HasColumnName("checksumtype");
            entity.Property(e => e.Checksumvalue).HasColumnName("checksumvalue");
            entity.Property(e => e.Componentid).HasColumnName("componentid");
            entity.Property(e => e.Releaseid).HasColumnName("releaseid");
            entity.Property(e => e.Uhistoryid).HasColumnName("uhistoryid");

            entity.HasOne(d => d.Component).WithMany(p => p.Releasesfiles)
                .HasForeignKey(d => d.Componentid)
                .HasConstraintName("releasesfiles_componentid_fkey");

            entity.HasOne(d => d.Release).WithMany(p => p.Releasesfiles)
                .HasForeignKey(d => d.Releaseid)
                .HasConstraintName("releasesfiles_releaseid_fkey");

            entity.HasOne(d => d.Uhistory).WithMany(p => p.Releasesfiles)
                .HasForeignKey(d => d.Uhistoryid)
                .HasConstraintName("releasesfiles_uhistoryid_fkey");
        });

        modelBuilder.Entity<SchemeHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_scheme_history");

            entity.ToTable("scheme_history");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreationStamp)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("creation_stamp");
            entity.Property(e => e.Scheme).HasColumnName("scheme");
        });

        modelBuilder.Entity<Uhistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("uhistory_pkey");

            entity.ToTable("uhistory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.D2)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("d2");
            entity.Property(e => e.Info).HasColumnName("info");
            entity.Property(e => e.Ipavailable).HasColumnName("ipavailable");
            entity.Property(e => e.Ipused).HasColumnName("ipused");
            entity.Property(e => e.Sizeooffile).HasColumnName("sizeooffile");
            entity.Property(e => e.Source).HasColumnName("source");
            entity.Property(e => e.Status)
                .HasColumnType("character varying")
                .HasColumnName("status");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
