using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataAccessLibrary.Models;

namespace LunhuiApp.Models
{
    public class CoreDbContext : DbContext
    {
        public DbSet<Todos> Todos { get; set; }
        public DbSet<Species> Species { get; set; }
        public DbSet<Deads> Deads { get; set; }
        public DbSet<Gongde> Gongde { get; set; }
        public DbSet<Deads> Zhuanshengs { get; set; }
        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        {

        }

        public CoreDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 添加组合主键
            modelBuilder.Entity<Gongde>().HasKey(g => new { g.ObjID, g.eventID });
        }
    }
}
