using Habitos.Dominio;
using Microsoft.EntityFrameworkCore;

namespace Habitos.Infraestrutura;

public class HabitosDbContext : DbContext
{
    public HabitosDbContext(DbContextOptions<HabitosDbContext> options) : base(options) { }

    public DbSet<Habito> Habitos => Set<Habito>();
    public DbSet<AgendaHabito> AgendasHabito => Set<AgendaHabito>();
    public DbSet<Marcacao> Marcacoes => Set<Marcacao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Tabela Habitos
        modelBuilder.Entity<Habito>(habito =>
        {
            habito.ToTable("Habitos");
            habito.HasKey(h => h.Id);

            habito.Property(h => h.Nome).HasMaxLength(60).IsRequired();
            habito.Property(h => h.NomeNormalizado).HasMaxLength(60).IsRequired();
            habito.Property(h => h.Descricao).HasMaxLength(200);
            habito.Property(h => h.Cor).HasMaxLength(7).IsRequired();
            habito.Property(h => h.DataInicio).IsRequired();
            habito.Property(h => h.CriadoEmUtc).IsRequired();

            // Índice único em NomeNormalizado (1.3)
            habito.HasIndex(h => h.NomeNormalizado).IsUnique();

            // Relacionamento com AgendaHabito (cascata na exclusão)
            habito.HasMany(h => h.Agendas)
                  .WithOne()
                  .HasForeignKey("HabitoId")
                  .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento com Marcacao (cascata na exclusão)
            habito.HasMany(h => h.Marcacoes)
                  .WithOne()
                  .HasForeignKey("HabitoId")
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Tabela AgendasHabito
        modelBuilder.Entity<AgendaHabito>(agenda =>
        {
            agenda.ToTable("AgendasHabito");
            agenda.HasKey(a => a.Id);

            agenda.Property(a => a.HabitoId).IsRequired();
            agenda.Property(a => a.VigenteDesde).IsRequired();

            // Frequencia como complex type (colunas Tipo, Dias, VezesPorSemana)
            agenda.ComplexProperty(a => a.Frequencia, freq =>
            {
                freq.Property(f => f.Tipo)
                    .HasColumnName("Tipo")
                    .HasConversion<int>()
                    .IsRequired();

                freq.Property(f => f.Dias)
                    .HasColumnName("Dias")
                    .HasConversion<int>()
                    .IsRequired();

                freq.Property(f => f.VezesPorSemana)
                    .HasColumnName("VezesPorSemana");
            });

            // Índice único por (HabitoId, VigenteDesde) (garante unicidade da agenda por dia)
            agenda.HasIndex(a => new { a.HabitoId, a.VigenteDesde }).IsUnique();
        });

        // Tabela Marcacoes
        modelBuilder.Entity<Marcacao>(marcacao =>
        {
            marcacao.ToTable("Marcacoes");
            marcacao.HasKey(m => m.Id);

            marcacao.Property(m => m.HabitoId).IsRequired();
            marcacao.Property(m => m.Data).IsRequired();
            marcacao.Property(m => m.CriadaEmUtc).IsRequired();

            // Índice único por (HabitoId, Data) — garante no máximo uma marcação por dia (6.3)
            marcacao.HasIndex(m => new { m.HabitoId, m.Data }).IsUnique();
        });
    }
}
