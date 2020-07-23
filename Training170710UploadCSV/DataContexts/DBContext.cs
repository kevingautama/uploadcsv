using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Training170710UploadCSV.Models;

namespace Training170710UploadCSV.DataContexts
{
    public class DBContext : DbContext
    {
        public DBContext()
        : base("FlatFileDB")
        { }

        public DbSet<FlatFile> FlatFiles { get; set; }
        public DbSet<ViewModel> UploadedDatas { get; set; }
    }
}