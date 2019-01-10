using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SerenBot.Entities
{
    public class SerenDbContextFactory : IDesignTimeDbContextFactory<SerenDbContext>
    {
        public SerenDbContext CreateDbContext(string[] args)
        {
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (string.IsNullOrWhiteSpace(env))
            {
                env = "Development";
            }

            IConfiguration Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var dbContexBuilder = new DbContextOptionsBuilder<SerenDbContext>();
            dbContexBuilder.UseMySql(Configuration.GetValue<string>("Database:ConnectionString"));

            return new SerenDbContext(dbContexBuilder.Options);
        }
    }
}
