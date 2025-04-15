using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace task_management_system_api.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<File> Files { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Projectuser> Projectusers { get; set; }

    public virtual DbSet<Refreshtoken> Refreshtokens { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Rolepermission> Rolepermissions { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<Taskcomment> Taskcomments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userrole> Userroles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=tmapi_db;Username=tmapi_root;Password=546258");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<File>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("files_pkey");

            entity.ToTable("files");

            entity.HasIndex(e => e.Projectid, "idx_files_projectid");

            entity.HasIndex(e => e.Taskid, "idx_files_taskid");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Filename)
                .HasMaxLength(255)
                .HasColumnName("filename");
            entity.Property(e => e.Filepath)
                .HasMaxLength(500)
                .HasColumnName("filepath");
            entity.Property(e => e.Projectid).HasColumnName("projectid");
            entity.Property(e => e.Taskid).HasColumnName("taskid");
            entity.Property(e => e.Uploadedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("uploadedat");
            entity.Property(e => e.Uploadedby).HasColumnName("uploadedby");

            entity.HasOne(d => d.Project).WithMany(p => p.Files)
                .HasForeignKey(d => d.Projectid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("files_projectid_fkey");

            entity.HasOne(d => d.Task).WithMany(p => p.Files)
                .HasForeignKey(d => d.Taskid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("files_taskid_fkey");

            entity.HasOne(d => d.UploadedbyNavigation).WithMany(p => p.Files)
                .HasForeignKey(d => d.Uploadedby)
                .HasConstraintName("files_uploadedby_fkey");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("notifications_pkey");

            entity.ToTable("notifications");

            entity.HasIndex(e => e.Userid, "idx_notifications_userid");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Content)
                .HasMaxLength(1024)
                .HasColumnName("content");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Isread)
                .HasDefaultValue(false)
                .HasColumnName("isread");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("notifications_userid_fkey");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("permissions_pkey");

            entity.ToTable("permissions");

            entity.HasIndex(e => e.Name, "permissions_name_key").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("projects_pkey");

            entity.ToTable("projects");

            entity.HasIndex(e => e.Name, "idx_projects_name");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Description)
                .HasMaxLength(1024)
                .HasColumnName("description");
            entity.Property(e => e.Enddate).HasColumnName("enddate");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Startdate).HasColumnName("startdate");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");

            entity.HasOne(d => d.CreatedbyNavigation).WithMany(p => p.Projects)
                .HasForeignKey(d => d.Createdby)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("projects_createdby_fkey");
        });

        modelBuilder.Entity<Projectuser>(entity =>
        {
            entity.HasKey(e => new { e.Projectid, e.Userid }).HasName("projectusers_pkey");

            entity.ToTable("projectusers");

            entity.Property(e => e.Projectid).HasColumnName("projectid");
            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Assignedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("assignedat");
            entity.Property(e => e.Roleid).HasColumnName("roleid");

            entity.HasOne(d => d.Project).WithMany(p => p.Projectusers)
                .HasForeignKey(d => d.Projectid)
                .HasConstraintName("projectusers_projectid_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.Projectusers)
                .HasForeignKey(d => d.Roleid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("projectusers_roleid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Projectusers)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("projectusers_userid_fkey");
        });

        modelBuilder.Entity<Refreshtoken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("refreshtokens_pkey");

            entity.ToTable("refreshtokens");

            entity.HasIndex(e => e.Userid, "idx_refreshtokens_userid");

            entity.HasIndex(e => e.Token, "refreshtokens_token_key").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Expiresat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("expiresat");
            entity.Property(e => e.Token)
                .HasMaxLength(500)
                .HasColumnName("token");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Refreshtokens)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("refreshtokens_userid_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.HasIndex(e => e.Name, "roles_name_key").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");
        });

        modelBuilder.Entity<Rolepermission>(entity =>
        {
            entity.HasKey(e => new { e.Roleid, e.Permissionid }).HasName("rolepermissions_pkey");

            entity.ToTable("rolepermissions");

            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Permissionid).HasColumnName("permissionid");
            entity.Property(e => e.Assignedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("assignedat");

            entity.HasOne(d => d.Permission).WithMany(p => p.Rolepermissions)
                .HasForeignKey(d => d.Permissionid)
                .HasConstraintName("rolepermissions_permissionid_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.Rolepermissions)
                .HasForeignKey(d => d.Roleid)
                .HasConstraintName("rolepermissions_roleid_fkey");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tasks_pkey");

            entity.ToTable("tasks");

            entity.HasIndex(e => e.Priority, "idx_tasks_priority");

            entity.HasIndex(e => e.Status, "idx_tasks_status");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Assigneeid).HasColumnName("assigneeid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Description)
                .HasMaxLength(1024)
                .HasColumnName("description");
            entity.Property(e => e.Duedate).HasColumnName("duedate");
            entity.Property(e => e.Priority)
                .HasMaxLength(20)
                .HasColumnName("priority");
            entity.Property(e => e.Projectid).HasColumnName("projectid");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'To Do'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");

            entity.HasOne(d => d.Assignee).WithMany(p => p.TaskAssignees)
                .HasForeignKey(d => d.Assigneeid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("tasks_assigneeid_fkey");

            entity.HasOne(d => d.CreatedbyNavigation).WithMany(p => p.TaskCreatedbyNavigations)
                .HasForeignKey(d => d.Createdby)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("tasks_createdby_fkey");

            entity.HasOne(d => d.Project).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.Projectid)
                .HasConstraintName("tasks_projectid_fkey");
        });

        modelBuilder.Entity<Taskcomment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("taskcomments_pkey");

            entity.ToTable("taskcomments");

            entity.HasIndex(e => e.Taskid, "idx_taskcomments_taskid");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Comment)
                .HasMaxLength(1024)
                .HasColumnName("comment");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Taskid).HasColumnName("taskid");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Task).WithMany(p => p.Taskcomments)
                .HasForeignKey(d => d.Taskid)
                .HasConstraintName("taskcomments_taskid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Taskcomments)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("taskcomments_userid_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "idx_users_email");

            entity.HasIndex(e => e.Username, "idx_users_username");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .HasColumnName("firstname");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(false)
                .HasColumnName("isactive");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .HasColumnName("lastname");
            entity.Property(e => e.Passwordhash)
                .HasMaxLength(255)
                .HasColumnName("passwordhash");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Userrole>(entity =>
        {
            entity.HasKey(e => new { e.Userid, e.Roleid }).HasName("userroles_pkey");

            entity.ToTable("userroles");

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Assignedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("assignedat");

            entity.HasOne(d => d.Role).WithMany(p => p.Userroles)
                .HasForeignKey(d => d.Roleid)
                .HasConstraintName("userroles_roleid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Userroles)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("userroles_userid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
