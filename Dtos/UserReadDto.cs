using System.ComponentModel.DataAnnotations;
using task_management_system_api.Models;
namespace task_management_system_api.Dtos;

public class UserReadDto
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public bool Isactive { get; set; }

    public DateTime Createdat { get; set; }

    public DateTime Updatedat { get; set; }

    // Optionally include related data as needed
    // public ICollection<FileDTO> Files { get; set; }
    // public ICollection<ProjectDTO> Projects { get; set; }
    public virtual ICollection<UserRoleReadDto> Userroles { get; set; } = new List<UserRoleReadDto>();
}