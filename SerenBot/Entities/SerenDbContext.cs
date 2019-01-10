using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SerenBot.Entities.Models;

namespace SerenBot.Entities
{
    public class SerenDbContext : DbContext
    {
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<NotificationUser> NotificationUsers { get; set; }

        public SerenDbContext(DbContextOptions<SerenDbContext> options) : base(options)
        {

        }
    }
}
