using Microsoft.EntityFrameworkCore;
using TaskManagerApplication.Models;
using TaskManagerApplication.Data;

namespace TaskManagerApplication.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // Apply migrations
            await context.Database.MigrateAsync();

            // ================= USERS =================
            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                    new User { Id = 1, Email = "user1@test.com", Name = "Alice", Surname = "Smith" },
                    new User { Id = 2, Email = "user2@test.com", Name = "Bob", Surname = "Johnson" },
                    new User { Id = 3, Email = "user3@test.com", Name = "Charlie", Surname = "Brown" },
                    new User { Id = 4, Email = "user4@test.com", Name = "Diana", Surname = "Lee" },
                    new User { Id = 5, Email = "user5@test.com", Name = "Eve", Surname = "Davis" }
                };

                context.Users.AddRange(users);
                await context.SaveChangesAsync();
            }

            // ================= PROJECTS =================
            if (!context.Projects.Any())
            {
                var projects = new List<Project>
                {
                    new Project { Name = "Project A", Description = "Sample project A", Status = ProjectStatus.NotStarted, CreatedAt = DateTime.UtcNow, UserId = 1 },
                    new Project { Name = "Project B", Description = "Sample project B", Status = ProjectStatus.InProgress, CreatedAt = DateTime.UtcNow, UserId = 2 },
                    new Project { Name = "Project C", Description = "Sample project C", Status = ProjectStatus.Completed, CreatedAt = DateTime.UtcNow, UserId = 3 },
                    new Project { Name = "Project D", Description = "Sample project D", Status = ProjectStatus.NotStarted, CreatedAt = DateTime.UtcNow, UserId = 4 },
                    new Project { Name = "Project E", Description = "Sample project E", Status = ProjectStatus.InProgress, CreatedAt = DateTime.UtcNow, UserId = 5 }
                };

                context.Projects.AddRange(projects);
                await context.SaveChangesAsync();
            }

            // ================= TASKS =================
            if (!context.Tasks.Any())
            {
                var allProjects = context.Projects.ToList();
                var tasks = new List<TaskItem>
                {
                    new TaskItem { Title = "Task 1", Description = "Task 1 for Project A", Priority = TaskPriority.High, Status = Status.Open, DueDate = DateTime.UtcNow.AddDays(3), ProjectId = allProjects[0].Id },
                    new TaskItem { Title = "Task 2", Description = "Task 2 for Project A", Priority = TaskPriority.Medium, Status = Status.InProgress, DueDate = DateTime.UtcNow.AddDays(5), ProjectId = allProjects[0].Id },

                    new TaskItem { Title = "Task 3", Description = "Task 1 for Project B", Priority = TaskPriority.Low, Status = Status.Open, DueDate = DateTime.UtcNow.AddDays(2), ProjectId = allProjects[1].Id },
                    new TaskItem { Title = "Task 4", Description = "Task 2 for Project B", Priority = TaskPriority.Medium, Status = Status.Done, DueDate = DateTime.UtcNow.AddDays(4), ProjectId = allProjects[1].Id },

                    new TaskItem { Title = "Task 5", Description = "Task 1 for Project C", Priority = TaskPriority.High, Status = Status.InProgress, DueDate = DateTime.UtcNow.AddDays(1), ProjectId = allProjects[2].Id },
                    new TaskItem { Title = "Task 6", Description = "Task 2 for Project C", Priority = TaskPriority.Low, Status = Status.Open, DueDate = DateTime.UtcNow.AddDays(6), ProjectId = allProjects[2].Id },

                    new TaskItem { Title = "Task 7", Description = "Task 1 for Project D", Priority = TaskPriority.Medium, Status = Status.Open, DueDate = DateTime.UtcNow.AddDays(2), ProjectId = allProjects[3].Id },
                    new TaskItem { Title = "Task 8", Description = "Task 2 for Project D", Priority = TaskPriority.High, Status = Status.Done, DueDate = DateTime.UtcNow.AddDays(3), ProjectId = allProjects[3].Id },

                    new TaskItem { Title = "Task 9", Description = "Task 1 for Project E", Priority = TaskPriority.Low, Status = Status.Open, DueDate = DateTime.UtcNow.AddDays(5), ProjectId = allProjects[4].Id },
                    new TaskItem { Title = "Task 10", Description = "Task 2 for Project E", Priority = TaskPriority.Medium, Status = Status.InProgress, DueDate = DateTime.UtcNow.AddDays(4), ProjectId = allProjects[4].Id }
                };

                context.Tasks.AddRange(tasks);
                await context.SaveChangesAsync();
            }
        }
    }
}