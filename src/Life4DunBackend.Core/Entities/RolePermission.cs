namespace Life4DunBackend.Core.Entities;

/// <summary>
/// Đại diện cho phân quyền của các vai trò (Role) trên các tài nguyên (Resource)
/// </summary>
public class RolePermission
{
    public Guid Id { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public string Resource { get; set; } = string.Empty;
}
