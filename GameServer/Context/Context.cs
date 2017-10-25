using GameServer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Context
{
    public class ServerContext : DbContext
    {

        public ServerContext() : base() { }

        public DbSet<Result> Results { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Properties<string>().Configure(p => p.HasColumnType("varchar"));
            modelBuilder.Properties<string>().Configure(p => p.HasMaxLength(150));
         
            base.OnModelCreating(modelBuilder);
        }
    }
}
