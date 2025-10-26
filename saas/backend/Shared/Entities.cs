using System;
using System.Collections.Generic;

namespace Shared.Entities
{
    public class Organization
    {
        public Guid OrganizationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }

    public class User
    {
        public Guid UserId { get; set; }
        public Guid OrganizationId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Organization? Organization { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }

    public class Role
    {
        public Guid RoleId { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }

    public class UserRole
    {
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public Guid RoleId { get; set; }
        public Role? Role { get; set; }
    }

    public class Project
    {
        public Guid ProjectId { get; set; }
        public Guid OrganizationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Organization? Organization { get; set; }
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }

    public class TaskItem
    {
        public Guid TaskItemId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? AssignedTo { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Project? Project { get; set; }
        public User? AssignedUser { get; set; }
    }

    public class AuditLog
    {
        public long AuditLogId { get; set; }
        public Guid UserId { get; set; }
        public string Action { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string? Details { get; set; }
        public User? User { get; set; }
    }
}
