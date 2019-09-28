using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace WriteNote.Models
{
    public class WNDatabaseContext : DbContext
    {
        public DbSet<Note> Notes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<NoteBook> NoteBooks { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<NoteDelete> NoteDeletes { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Buy> Buys { get; set; }
        public DbSet<Log> Logs { get; set; }

        public WNDatabaseContext() : base("name=WNConnectionString") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<WNDatabaseContext>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
}