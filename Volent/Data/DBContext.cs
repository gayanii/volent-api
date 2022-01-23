using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using Volent.Models;

namespace Volent.Data
{
    public class DBContext : DbContext  
    {
        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public DbSet<Volunteer> Volunteers { get; set; } = null;
    }
}
