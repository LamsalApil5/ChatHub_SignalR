using Entity.Tables;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class MyDBContext : IdentityDbContext<Users, IdentityRole<int>, int>, IMyDBContext
    {
        public MyDBContext(DbContextOptions<MyDBContext> options)
            : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Chats> Chats { get; set; }
        public DbSet<GroupChats> GroupChats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Add additional configuration here if needed
        }
    }
}
