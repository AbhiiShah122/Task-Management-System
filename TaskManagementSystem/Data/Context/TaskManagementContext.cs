using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaskManagementSystem.Data.Helper;

namespace TaskManagementSystem.Data.Context
{
    public class TaskManagementContext : DbContext
    {
        public TaskManagementContext(DbContextOptions<TaskManagementContext> options) : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<EmployeeEntity> Employees { get; set; }
        public DbSet<ManagerEntity> Managers { get; set; }
        public DbSet<AdminEntity> Admins { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<TaskAttachmentEntity> TaskAttachments { get; set; }
        public DbSet<TaskNoteEntity> TaskNotes { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<TeamEntity> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User, Employee, Manager, Admin relationship
            modelBuilder.Entity<UserEntity>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<EmployeeEntity>("Employee")
                .HasValue<ManagerEntity>("Manager")
                .HasValue<AdminEntity>("Admin");

            // User to Team relationship
            modelBuilder.Entity<UserEntity>()
                .HasOne(u => u.Team)
                .WithMany(t => t.Users)
                .HasForeignKey(u => u.TeamId)
                .OnDelete(DeleteBehavior.SetNull);

            // Convert Role enum to it's value
            modelBuilder.Entity<RoleEntity>()
                .Property(r => r.RoleName)
                .HasConversion(
                    v => v.ToString(), //Convert From
                    v => (UserRole)Enum.Parse(typeof(UserRole), v)); //Convert To

            modelBuilder.Entity<RoleEntity>()
                .HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId);

            // Employee to Manager relationship
            modelBuilder.Entity<EmployeeEntity>()
                .HasOne(e => e.Manager)
                .WithMany(m => m.Employees)
                .HasForeignKey(e => e.ManagerId);

            // Employee to TaskEntity relationship
            modelBuilder.Entity<EmployeeEntity>()
                .HasMany(e => e.Tasks)
                .WithOne(t => t.Employee)
                .HasForeignKey(t => t.EmployeeId);

            // Employee to Manager relationship
            modelBuilder.Entity<EmployeeEntity>()
                .HasOne(e => e.Manager)
                .WithMany(m => m.Employees)
                .HasForeignKey(e => e.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            // TaskEntity to TaskAttachment relationship
            modelBuilder.Entity<TaskEntity>()
                .HasMany(t => t.Attachments)
                .WithOne(a => a.Task)
                .HasForeignKey(a => a.TaskId);

            // TaskEntity to TaskNote relationship
            modelBuilder.Entity<TaskEntity>()
                .HasMany(t => t.Notes)
                .WithOne(n => n.Task)
                .HasForeignKey(n => n.TaskId);

            PopulateDate(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        public static void PopulateDate(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminEntity>().HasData(
                new AdminEntity { UserId = 1, FirstName = "Admin", LastName = "Admin", Email = "admin@gmail.com", Password = new PasswordGenerator().GeneratePassword(8), RoleId = 3 }
             );

            modelBuilder.Entity<EmployeeEntity>().HasData(
                new EmployeeEntity { UserId = 2, FirstName = "Abhi", LastName = "Shah", Email = "abhi.shah@gmail.com", Password = new PasswordGenerator().GeneratePassword(8), RoleId = 1, ManagerId = 6, TeamId = 1 },
                new EmployeeEntity { UserId = 3, FirstName = "Parth", LastName = "Shah", Email = "parth.shah@gmail.com", Password = new PasswordGenerator().GeneratePassword(8), RoleId = 1, ManagerId = 6, TeamId = 1 },
                new EmployeeEntity { UserId = 4, FirstName = "Jay", LastName = "Shah", Email = "jay.shah@gmail.com", Password = new PasswordGenerator().GeneratePassword(8), RoleId = 1, ManagerId = 7, TeamId = 2 },
                new EmployeeEntity { UserId = 5, FirstName = "Smit", LastName = "Shah", Email = "smit.shah@gmail.com", Password = new PasswordGenerator().GeneratePassword(8), RoleId = 1, ManagerId = 7, TeamId = 2 }
            );

            modelBuilder.Entity<ManagerEntity>().HasData(
                new ManagerEntity { UserId = 6, FirstName = "Chiman", LastName = "Patel", Email = "chiman.patel@gmail.com", Password = new PasswordGenerator().GeneratePassword(8), RoleId = 2, TeamId = 1 },
                new ManagerEntity { UserId = 7, FirstName = "Prakash", LastName = "Shah", Email = "prakash.shah@gmail.com", Password = new PasswordGenerator().GeneratePassword(8), RoleId = 2, TeamId = 2 }
            );

            modelBuilder.Entity<RoleEntity>().HasData(
                new RoleEntity { RoleId = 1, RoleName = UserRole.Employee },
                new RoleEntity { RoleId = 2, RoleName = UserRole.Manager },
                new RoleEntity { RoleId = 3, RoleName = UserRole.Admin }
            );

            modelBuilder.Entity<TaskAttachmentEntity>().HasData(
                new TaskAttachmentEntity { TaskAttachmentId = 1, TaskId = 1, FileName = "Attachment1.txt", FilePath = "Desktop" },
                new TaskAttachmentEntity { TaskAttachmentId = 2, TaskId = 1, FileName = "Attachment2.jpg", FilePath = "Desktop" },
                new TaskAttachmentEntity { TaskAttachmentId = 3, TaskId = 2, FileName = "Attachment3.jpg", FilePath = "Desktop" },
                new TaskAttachmentEntity { TaskAttachmentId = 4, TaskId = 2, FileName = "Attachment4.jpg", FilePath = "Desktop" },
                new TaskAttachmentEntity { TaskAttachmentId = 5, TaskId = 3, FileName = "Attachment5.jpg", FilePath = "Desktop" },
                new TaskAttachmentEntity { TaskAttachmentId = 6, TaskId = 4, FileName = "Attachment6.jpg", FilePath = "Desktop" }
            );


            modelBuilder.Entity<TaskEntity>().HasData(
                new TaskEntity { TaskId = 1, Title = "Task 1", Description = "Develop backend", DueDate = DateTime.UtcNow.AddDays(2), EmployeeId = 2 },
                new TaskEntity { TaskId = 2, Title = "Task 2", Description = "Develop frontend (optional)", DueDate = DateTime.UtcNow.AddDays(3), EmployeeId = 3 },
                new TaskEntity { TaskId = 3, Title = "Task 1", Description = "Add Authentication", DueDate = DateTime.UtcNow.AddDays(2), EmployeeId = 2 },
                new TaskEntity { TaskId = 4, Title = "Task 2", Description = "Add Unit Test", DueDate = DateTime.UtcNow.AddDays(3), EmployeeId = 3 }
            );

            modelBuilder.Entity<TaskNoteEntity>().HasData(
                new TaskNoteEntity { TaskNoteId = 1, TaskId = 1, Note = "Note 1 for Task 1" },
                new TaskNoteEntity { TaskNoteId = 2, TaskId = 1, Note = "Note 2 for Task 1" },
                new TaskNoteEntity { TaskNoteId = 3, TaskId = 2, Note = "Note 2 for Task 1" },
                new TaskNoteEntity { TaskNoteId = 4, TaskId = 2, Note = "Note 2 for Task 1" },
                new TaskNoteEntity { TaskNoteId = 5, TaskId = 3, Note = "Note 2 for Task 1" },
                new TaskNoteEntity { TaskNoteId = 6, TaskId = 4, Note = "Note 2 for Task 1" }

            );

            modelBuilder.Entity<TeamEntity>().HasData(
                new TeamEntity { TeamId = 1, TeamName = "Development" },
                new TeamEntity { TeamId = 2, TeamName = "Marketing" },
                new TeamEntity { TeamId = 3, TeamName = "Sales" }
            );
        }
    }
}
