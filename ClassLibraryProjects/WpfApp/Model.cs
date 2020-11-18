using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace WpfApp
{
    public class ImageInformation
    {
        public int ImageInformationId { get; set; }
        public string Path { get; set; }
        public float Confidence { get; set; }
    }

    public class ModelContext: DbContext
    {
        public DbSet<ImageInformation> ImagesInformation { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=model.db");
    }
}
