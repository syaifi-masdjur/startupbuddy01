using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace startupbuddy01.Data
{
    public class SbDataContext:DbContext
    {
        public DbSet<AdminUserModel> Users { get; set; }
    }
}