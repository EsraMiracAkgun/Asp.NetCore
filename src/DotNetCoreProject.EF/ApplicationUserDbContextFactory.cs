﻿using DotNetCoreProject.EF.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotNetCoreProject.EF
{
    public class ApplicationUserDbContextFactory :
        IDesignTimeDbContextFactory<ApplicationUserDbContext>
    {
        public ApplicationUserDbContext CreateDbContext(string[] args)
        {
            var dbContext = new ApplicationUserDbContext(
                new DbContextOptionsBuilder<ApplicationUserDbContext>()
                .UseSqlServer(
                    new ConfigurationBuilder()
                    .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory
                    (), "appsettings.json"))
                    .Build().GetConnectionString("DotNetCoreProjectDb")
                    ).Options
                );
            return dbContext;
        }
    }
}
