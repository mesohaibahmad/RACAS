using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

#nullable disable

namespace RACAS.Models
{
    public partial class RACASContext : DbContext
    {
        public RACASContext()
        {
        }

        public RACASContext(DbContextOptions<RACASContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Branches> Branches { get; set; }
        public virtual DbSet<CausedBy> CausedBy { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<Logs> Logs { get; set; }
        public virtual DbSet<MainLedger> MainLedger { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<ModuleAction> ModuleActions { get; set; }
        public virtual DbSet<Partners> Partners { get; set; }
        public virtual DbSet<TaskLedger> TaskLedgers { get; set; }
        public virtual DbSet<TaskLevel> TaskLevels { get; set; }
        public virtual DbSet<TaskType> TaskTypes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserBranch> UserBranches { get; set; }
        public virtual DbSet<UserModule> UserModules { get; set; }
        public virtual DbSet<UserTypes> UserTypes { get; set; }
        public virtual DbSet<Descriptions> Descriptions { get; set; }



    }
}
