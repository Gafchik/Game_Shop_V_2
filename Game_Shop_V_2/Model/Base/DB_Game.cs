using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Game_Shop_V_2.Model.Base
{
    public partial class DB_Game : DbContext
    {
        public DB_Game()
            : base("name=DB_Game")
        {
        }

        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Mod_Game> Mod_Game { get; set; }
        public virtual DbSet<Studio> Studios { get; set; }
        public virtual DbSet<Style> Styles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mod_Game>()
                .HasMany(e => e.Games)
                .WithRequired(e => e.Mod_Game)
                .HasForeignKey(e => e.Game_Mod_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Studio>()
                .HasMany(e => e.Games)
                .WithRequired(e => e.Studio)
                .HasForeignKey(e => e.Game_Studio_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Style>()
                .HasMany(e => e.Games)
                .WithRequired(e => e.Style)
                .HasForeignKey(e => e.Game_Style_id)
                .WillCascadeOnDelete(false);
        }
    }
}
