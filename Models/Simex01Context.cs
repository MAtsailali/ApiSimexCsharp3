using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ApiSimexCsharp.Models;

public partial class Simex01Context : DbContext
{
    public Simex01Context()
    {
    }

    public Simex01Context(DbContextOptions<Simex01Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Aeroport> Aeroports { get; set; }

    public virtual DbSet<Cache> Caches { get; set; }

    public virtual DbSet<CacheLock> CacheLocks { get; set; }

    public virtual DbSet<Ciutat> Ciutats { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Conversations2> Conversations2s { get; set; }

    public virtual DbSet<ConverstionsUser> ConverstionsUsers { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<Divisa> Divisas { get; set; }

    public virtual DbSet<EstatsOferte> EstatsOfertes { get; set; }

    public virtual DbSet<FailedJob> FailedJobs { get; set; }

    public virtual DbSet<IaMessage> IaMessages { get; set; }

    public virtual DbSet<Incoterm> Incoterms { get; set; }

    public virtual DbSet<IncotermPaso> IncotermPasos { get; set; }

    public virtual DbSet<Industrium> Industria { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<JobBatch> JobBatches { get; set; }

    public virtual DbSet<LiniesTransportMaritim> LiniesTransportMaritims { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Migration> Migrations { get; set; }

    public virtual DbSet<NotificacionDestinatario> NotificacionDestinatarios { get; set; }

    public virtual DbSet<Notificacione> Notificaciones { get; set; }

    public virtual DbSet<OfertaSeguimiento> OfertaSeguimientos { get; set; }

    public virtual DbSet<Oferte> Ofertes { get; set; }

    public virtual DbSet<Paisso> Paissos { get; set; }

    public virtual DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

    public virtual DbSet<Payterm> Payterms { get; set; }

    public virtual DbSet<PersonalAccessToken> PersonalAccessTokens { get; set; }

    public virtual DbSet<Port> Ports { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<Tipo> Tipos { get; set; }

    public virtual DbSet<TipusCarrega> TipusCarregas { get; set; }

    public virtual DbSet<TipusContenidor> TipusContenidors { get; set; }

    public virtual DbSet<TipusFlux> TipusFluxes { get; set; }

    public virtual DbSet<TipusIncoterm> TipusIncoterms { get; set; }

    public virtual DbSet<TipusTransport> TipusTransports { get; set; }

    public virtual DbSet<TipusValidacion> TipusValidacions { get; set; }

    public virtual DbSet<TrackingStep> TrackingSteps { get; set; }

    public virtual DbSet<Transportiste> Transportistes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Usuari> Usuaris { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=51.83.192.177;Database=simex01;User Id=simex01;Password=Pepe2026;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aeroport>(entity =>
        {
            entity.ToTable("aeroports");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Codi)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("codi");
            entity.Property(e => e.IdCiutat).HasColumnName("id_ciutat");
            entity.Property(e => e.Nom)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("nom");

            entity.HasOne(d => d.IdCiutatNavigation).WithMany(p => p.Aeroports)
                .HasForeignKey(d => d.IdCiutat)
                .HasConstraintName("FK_aeroports_ciutats");
        });

        modelBuilder.Entity<Cache>(entity =>
        {
            entity.HasKey(e => e.Key).HasName("cache_key_primary");

            entity.ToTable("cache");

            entity.HasIndex(e => e.Expiration, "cache_expiration_index");

            entity.Property(e => e.Key)
                .HasMaxLength(255)
                .HasColumnName("key");
            entity.Property(e => e.Expiration).HasColumnName("expiration");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        modelBuilder.Entity<CacheLock>(entity =>
        {
            entity.HasKey(e => e.Key).HasName("cache_locks_key_primary");

            entity.ToTable("cache_locks");

            entity.HasIndex(e => e.Expiration, "cache_locks_expiration_index");

            entity.Property(e => e.Key)
                .HasMaxLength(255)
                .HasColumnName("key");
            entity.Property(e => e.Expiration).HasColumnName("expiration");
            entity.Property(e => e.Owner)
                .HasMaxLength(255)
                .HasColumnName("owner");
        });

        modelBuilder.Entity<Ciutat>(entity =>
        {
            entity.ToTable("ciutats");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nom");
            entity.Property(e => e.PaisId).HasColumnName("pais_id");

            entity.HasOne(d => d.Pais).WithMany(p => p.Ciutats)
                .HasForeignKey(d => d.PaisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ciutats_paissos");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("companies");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("company_name");
            entity.Property(e => e.IndustriaId).HasColumnName("industria_id");

            entity.HasOne(d => d.Industria).WithMany(p => p.Companies)
                .HasForeignKey(d => d.IndustriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_companies_industria");
        });

        modelBuilder.Entity<Conversations2>(entity =>
        {
            entity.ToTable("conversations2");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AgentId).HasColumnName("agent_id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.LastMessageAt).HasColumnName("last_message_at");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("status");
        });

        modelBuilder.Entity<ConverstionsUser>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("converstions_user");

            entity.Property(e => e.IdConversation).HasColumnName("id_conversation");
            entity.Property(e => e.IdUsuaris).HasColumnName("id_usuaris");

            entity.HasOne(d => d.IdConversationNavigation).WithMany()
                .HasForeignKey(d => d.IdConversation)
                .HasConstraintName("FK_converstions_user_conversations2");

            entity.HasOne(d => d.IdUsuarisNavigation).WithMany()
                .HasForeignKey(d => d.IdUsuaris)
                .HasConstraintName("FK_converstions_user_usuaris");
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("currency");

            entity.Property(e => e.Currency1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("currency");
            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("id");
        });

        modelBuilder.Entity<Divisa>(entity =>
        {
            entity.ToTable("divisas");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Tipus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipus");
        });

        modelBuilder.Entity<EstatsOferte>(entity =>
        {
            entity.ToTable("estats_ofertes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estat)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("estat");
        });

        modelBuilder.Entity<FailedJob>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__failed_j__3213E83FA2FB4356");

            entity.ToTable("failed_jobs");

            entity.HasIndex(e => e.Uuid, "failed_jobs_uuid_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Connection).HasColumnName("connection");
            entity.Property(e => e.Exception).HasColumnName("exception");
            entity.Property(e => e.FailedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("failed_at");
            entity.Property(e => e.Payload).HasColumnName("payload");
            entity.Property(e => e.Queue).HasColumnName("queue");
            entity.Property(e => e.Uuid)
                .HasMaxLength(255)
                .HasColumnName("uuid");
        });

        modelBuilder.Entity<IaMessage>(entity =>
        {
            entity.ToTable("ia_messages");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IaMessage1)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ia_message");
            entity.Property(e => e.IdUser).HasColumnName("id_user");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.IaMessages)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ia_messages_usuaris");
        });

        modelBuilder.Entity<Incoterm>(entity =>
        {
            entity.ToTable("incoterms");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TipusIncontermId).HasColumnName("tipus_inconterm_id");
            entity.Property(e => e.TrackingStepsId).HasColumnName("tracking_steps_id");

            entity.HasOne(d => d.TipusInconterm).WithMany(p => p.Incoterms)
                .HasForeignKey(d => d.TipusIncontermId)
                .HasConstraintName("FK_incoterms_tipus_incoterms");
        });

        modelBuilder.Entity<IncotermPaso>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__incoterm__3213E83FEED1EE5A");

            entity.ToTable("incoterm_pasos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IncotermId).HasColumnName("incoterm_id");
            entity.Property(e => e.Orden).HasColumnName("orden");
            entity.Property(e => e.TrackingStepId).HasColumnName("tracking_step_id");

            entity.HasOne(d => d.Incoterm).WithMany(p => p.IncotermPasos)
                .HasForeignKey(d => d.IncotermId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_inco_paso_main");

            entity.HasOne(d => d.TrackingStep).WithMany(p => p.IncotermPasos)
                .HasForeignKey(d => d.TrackingStepId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_inco_paso_step");
        });

        modelBuilder.Entity<Industrium>(entity =>
        {
            entity.ToTable("industria");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Categoria)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("categoria");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__jobs__3213E83FE609B8DA");

            entity.ToTable("jobs");

            entity.HasIndex(e => e.Queue, "jobs_queue_index");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Attempts).HasColumnName("attempts");
            entity.Property(e => e.AvailableAt).HasColumnName("available_at");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Payload).HasColumnName("payload");
            entity.Property(e => e.Queue)
                .HasMaxLength(255)
                .HasColumnName("queue");
            entity.Property(e => e.ReservedAt).HasColumnName("reserved_at");
        });

        modelBuilder.Entity<JobBatch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("job_batches_id_primary");

            entity.ToTable("job_batches");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.CancelledAt).HasColumnName("cancelled_at");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.FailedJobIds).HasColumnName("failed_job_ids");
            entity.Property(e => e.FailedJobs).HasColumnName("failed_jobs");
            entity.Property(e => e.FinishedAt).HasColumnName("finished_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Options).HasColumnName("options");
            entity.Property(e => e.PendingJobs).HasColumnName("pending_jobs");
            entity.Property(e => e.TotalJobs).HasColumnName("total_jobs");
        });

        modelBuilder.Entity<LiniesTransportMaritim>(entity =>
        {
            entity.ToTable("linies_transport_maritim");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CiutatId).HasColumnName("ciutat_id");
            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nom");

            entity.HasOne(d => d.Ciutat).WithMany(p => p.LiniesTransportMaritims)
                .HasForeignKey(d => d.CiutatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_linies_transport_maritim_ciutats");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_user_message");

            entity.ToTable("messages");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdConversations).HasColumnName("id_conversations");
            entity.Property(e => e.Message1)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("message");

            entity.HasOne(d => d.IdConversationsNavigation).WithMany(p => p.Messages)
                .HasForeignKey(d => d.IdConversations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_messages_conversations2");
        });

        modelBuilder.Entity<Migration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__migratio__3213E83F29352364");

            entity.ToTable("migrations");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Batch).HasColumnName("batch");
            entity.Property(e => e.Migration1)
                .HasMaxLength(255)
                .HasColumnName("migration");
        });

        modelBuilder.Entity<NotificacionDestinatario>(entity =>
        {
            entity.ToTable("notificacion_destinatarios");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.Leida).HasColumnName("leida");
            entity.Property(e => e.NotificacionId).HasColumnName("notificacion_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Company).WithMany(p => p.NotificacionDestinatarios)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_notificacion_destinatarios_companies");

            entity.HasOne(d => d.Notificacion).WithMany(p => p.NotificacionDestinatarios)
                .HasForeignKey(d => d.NotificacionId)
                .HasConstraintName("FK_notificacion_destinatarios_notificaciones");

            entity.HasOne(d => d.User).WithMany(p => p.NotificacionDestinatarios)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_notificacion_destinatarios_usuaris");
        });

        modelBuilder.Entity<Notificacione>(entity =>
        {
            entity.ToTable("notificaciones");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmisorId).HasColumnName("emisor_id");
            entity.Property(e => e.Mensaje)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("mensaje");
            entity.Property(e => e.TipoId).HasColumnName("tipo_id");
            entity.Property(e => e.Titulo)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("titulo");

            entity.HasOne(d => d.Emisor).WithMany(p => p.Notificaciones)
                .HasForeignKey(d => d.EmisorId)
                .HasConstraintName("FK_notificaciones_usuaris");

            entity.HasOne(d => d.Tipo).WithMany(p => p.Notificaciones)
                .HasForeignKey(d => d.TipoId)
                .HasConstraintName("FK_notificaciones_tipo");
        });

        modelBuilder.Entity<OfertaSeguimiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__oferta_s__3213E83F640115C3");

            entity.ToTable("oferta_seguimiento");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DocumentoPath)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("documento_path");
            entity.Property(e => e.EstaCompletado)
                .HasDefaultValue(0)
                .HasColumnName("esta_completado");
            entity.Property(e => e.FechaCompletado)
                .HasColumnType("datetime")
                .HasColumnName("fecha_completado");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("observaciones");
            entity.Property(e => e.OfertaId).HasColumnName("oferta_id");
            entity.Property(e => e.Orden).HasColumnName("orden");
            entity.Property(e => e.TrackingStepId).HasColumnName("tracking_step_id");

            entity.HasOne(d => d.Oferta).WithMany(p => p.OfertaSeguimientos)
                .HasForeignKey(d => d.OfertaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_seguimiento_oferta");

            entity.HasOne(d => d.TrackingStep).WithMany(p => p.OfertaSeguimientos)
                .HasForeignKey(d => d.TrackingStepId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_seguimiento_step");
        });

        modelBuilder.Entity<Oferte>(entity =>
        {
            entity.ToTable("ofertes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AeroportDestiId).HasColumnName("aeroport_desti_id");
            entity.Property(e => e.AeroportOrigenId).HasColumnName("aeroport_origen_id");
            entity.Property(e => e.AgentComercialId).HasColumnName("agent_comercial_id");
            entity.Property(e => e.Bultos)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("bultos");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.ComentariosImprimir)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("comentarios_imprimir");
            entity.Property(e => e.ComentariosInternos)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("comentarios_internos");
            entity.Property(e => e.Concepto)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("concepto");
            entity.Property(e => e.DataCreacio).HasColumnName("data_creacio");
            entity.Property(e => e.DataValidessaFina).HasColumnName("data_validessa_fina");
            entity.Property(e => e.DataValidessaInicial).HasColumnName("data_validessa_inicial");
            entity.Property(e => e.DescripMercancia)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descrip_mercancia");
            entity.Property(e => e.DivisasId).HasColumnName("divisas_id");
            entity.Property(e => e.EstatOfertaId).HasColumnName("estat_oferta_id");
            entity.Property(e => e.IncotermId).HasColumnName("incoterm_id");
            entity.Property(e => e.LiniaTransportMaritimId).HasColumnName("linia_transport_maritim_id");
            entity.Property(e => e.OperadorId).HasColumnName("operador_id");
            entity.Property(e => e.PesBrut)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("pes_brut");
            entity.Property(e => e.PortDestiId).HasColumnName("port_desti_id");
            entity.Property(e => e.PortOrigenId).HasColumnName("port_origen_id");
            entity.Property(e => e.RaoRebuig)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("rao_rebuig");
            entity.Property(e => e.TipusCarregaId).HasColumnName("tipus_carrega_id");
            entity.Property(e => e.TipusContenidorId).HasColumnName("tipus_contenidor_id");
            entity.Property(e => e.TipusFluxeId).HasColumnName("tipus_fluxe_id");
            entity.Property(e => e.TipusTransportId).HasColumnName("tipus_transport_id");
            entity.Property(e => e.TipusValidacioId).HasColumnName("tipus_validacio_id");
            entity.Property(e => e.TransportistaId).HasColumnName("transportista_id");
            entity.Property(e => e.Valor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("valor");
            entity.Property(e => e.Volum)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("volum");

            entity.HasOne(d => d.AeroportOrigen).WithMany(p => p.Ofertes)
                .HasForeignKey(d => d.AeroportOrigenId)
                .HasConstraintName("FK_ofertes_aeroports");

            entity.HasOne(d => d.Divisas).WithMany(p => p.Ofertes)
                .HasForeignKey(d => d.DivisasId)
                .HasConstraintName("FK_ofertes_divisas");

            entity.HasOne(d => d.EstatOferta).WithMany(p => p.Ofertes)
                .HasForeignKey(d => d.EstatOfertaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ofertes_estats_ofertes");

            entity.HasOne(d => d.Incoterm).WithMany(p => p.Ofertes)
                .HasForeignKey(d => d.IncotermId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ofertes_incoterms");

            entity.HasOne(d => d.LiniaTransportMaritim).WithMany(p => p.Ofertes)
                .HasForeignKey(d => d.LiniaTransportMaritimId)
                .HasConstraintName("FK_ofertes_linies_transport_maritim");

            entity.HasOne(d => d.Operador).WithMany(p => p.Ofertes)
                .HasForeignKey(d => d.OperadorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ofertes_usuaris");

            entity.HasOne(d => d.PortOrigen).WithMany(p => p.Ofertes)
                .HasForeignKey(d => d.PortOrigenId)
                .HasConstraintName("FK_ofertes_ports");

            entity.HasOne(d => d.TipusCarrega).WithMany(p => p.Ofertes)
                .HasForeignKey(d => d.TipusCarregaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ofertes_tipus_carrega");

            entity.HasOne(d => d.TipusContenidor).WithMany(p => p.Ofertes)
                .HasForeignKey(d => d.TipusContenidorId)
                .HasConstraintName("FK_ofertes_tipus_contenidors");

            entity.HasOne(d => d.TipusFluxe).WithMany(p => p.Ofertes)
                .HasForeignKey(d => d.TipusFluxeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ofertes_tipus_fluxes");

            entity.HasOne(d => d.TipusTransport).WithMany(p => p.Ofertes)
                .HasForeignKey(d => d.TipusTransportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ofertes_tipus_transports");

            entity.HasOne(d => d.TipusValidacio).WithMany(p => p.Ofertes)
                .HasForeignKey(d => d.TipusValidacioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ofertes_tipus_validacions");

            entity.HasOne(d => d.Transportista).WithMany(p => p.Ofertes)
                .HasForeignKey(d => d.TransportistaId)
                .HasConstraintName("FK_ofertes_transportistes");
        });

        modelBuilder.Entity<Paisso>(entity =>
        {
            entity.ToTable("paissos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nom");
        });

        modelBuilder.Entity<PasswordResetToken>(entity =>
        {
            entity.HasKey(e => e.Email).HasName("password_reset_tokens_email_primary");

            entity.ToTable("password_reset_tokens");

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Token)
                .HasMaxLength(255)
                .HasColumnName("token");
        });

        modelBuilder.Entity<Payterm>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("payterms");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("id");
            entity.Property(e => e.Payterms)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("payterms");
        });

        modelBuilder.Entity<PersonalAccessToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__personal__3213E83FEE89B1AD");

            entity.ToTable("personal_access_tokens");

            entity.HasIndex(e => e.ExpiresAt, "personal_access_tokens_expires_at_index");

            entity.HasIndex(e => e.Token, "personal_access_tokens_token_unique").IsUnique();

            entity.HasIndex(e => new { e.TokenableType, e.TokenableId }, "personal_access_tokens_tokenable_type_tokenable_id_index");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Abilities).HasColumnName("abilities");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ExpiresAt)
                .HasColumnType("datetime")
                .HasColumnName("expires_at");
            entity.Property(e => e.LastUsedAt)
                .HasColumnType("datetime")
                .HasColumnName("last_used_at");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Token)
                .HasMaxLength(64)
                .HasColumnName("token");
            entity.Property(e => e.TokenableId).HasColumnName("tokenable_id");
            entity.Property(e => e.TokenableType)
                .HasMaxLength(255)
                .HasColumnName("tokenable_type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Port>(entity =>
        {
            entity.ToTable("ports");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdCiutat).HasColumnName("id_ciutat");
            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nom");

            entity.HasOne(d => d.IdCiutatNavigation).WithMany(p => p.Ports)
                .HasForeignKey(d => d.IdCiutat)
                .HasConstraintName("FK_ports_ciutats");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.ToTable("rols");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Rol1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("rol");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sessions_id_primary");

            entity.ToTable("sessions");

            entity.HasIndex(e => e.LastActivity, "sessions_last_activity_index");

            entity.HasIndex(e => e.UserId, "sessions_user_id_index");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(45)
                .HasColumnName("ip_address");
            entity.Property(e => e.LastActivity).HasColumnName("last_activity");
            entity.Property(e => e.Payload).HasColumnName("payload");
            entity.Property(e => e.UserAgent).HasColumnName("user_agent");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<Tipo>(entity =>
        {
            entity.ToTable("tipo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nom)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("nom");
        });

        modelBuilder.Entity<TipusCarrega>(entity =>
        {
            entity.ToTable("tipus_carrega");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Tipus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipus");
        });

        modelBuilder.Entity<TipusContenidor>(entity =>
        {
            entity.ToTable("tipus_contenidors");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Tipus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipus");
        });

        modelBuilder.Entity<TipusFlux>(entity =>
        {
            entity.ToTable("tipus_fluxes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Tipus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipus");
        });

        modelBuilder.Entity<TipusIncoterm>(entity =>
        {
            entity.ToTable("tipus_incoterms");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Codi)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("codi");
            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nom");
        });

        modelBuilder.Entity<TipusTransport>(entity =>
        {
            entity.ToTable("tipus_transports");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Tipus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipus");
        });

        modelBuilder.Entity<TipusValidacion>(entity =>
        {
            entity.ToTable("tipus_validacions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Tipus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipus");
        });

        modelBuilder.Entity<TrackingStep>(entity =>
        {
            entity.ToTable("tracking_steps");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nom");
            entity.Property(e => e.Ordre).HasColumnName("ordre");
        });

        modelBuilder.Entity<Transportiste>(entity =>
        {
            entity.ToTable("transportistes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CiutatId).HasColumnName("ciutat_id");
            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nom");

            entity.HasOne(d => d.Ciutat).WithMany(p => p.Transportistes)
                .HasForeignKey(d => d.CiutatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_transportistes_ciutats");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F6DCD67D8");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.EmailVerifiedAt)
                .HasColumnType("datetime")
                .HasColumnName("email_verified_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.RememberToken)
                .HasMaxLength(100)
                .HasColumnName("remember_token");
            entity.Property(e => e.TwoFactorConfirmedAt)
                .HasColumnType("datetime")
                .HasColumnName("two_factor_confirmed_at");
            entity.Property(e => e.TwoFactorRecoveryCodes).HasColumnName("two_factor_recovery_codes");
            entity.Property(e => e.TwoFactorSecret).HasColumnName("two_factor_secret");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Usuari>(entity =>
        {
            entity.ToTable("usuaris");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cognoms)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cognoms");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.Contrasenya)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("contrasenya");
            entity.Property(e => e.Correu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("correu");
            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nom");
            entity.Property(e => e.RolId).HasColumnName("rol_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Tlfn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tlfn");
            entity.Property(e => e.UltimaConex)
                .HasColumnType("datetime")
                .HasColumnName("ultima_conex");

            entity.HasOne(d => d.Company).WithMany(p => p.Usuaris)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_usuaris_companies");

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuaris)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_usuaris_rols");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
