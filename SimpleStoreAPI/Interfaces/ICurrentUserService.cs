namespace SimpleStoreAPI.Interfaces;

public interface ICurrentUserService
{
    public string? GetUserId();
    public bool IsInRole(string role);
}
