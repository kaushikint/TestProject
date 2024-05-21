using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FileUploadDownloadAPI.Models
{
    public partial class FileIOContext : DbContext
    {
        public FileIOContext()
        {
        }

        public FileIOContext(DbContextOptions<FileIOContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblFile> TblFiles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Server=INTLAP-037-ANAD\\MSSQL2019;Database=FileIO;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblFile>(entity =>
            {
                entity.ToTable("tblFiles");

                entity.Property(e => e.FileName).HasMaxLength(50);

                entity.Property(e => e.FilePath).HasMaxLength(200);

                entity.Property(e => e.FileType).HasMaxLength(10);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
