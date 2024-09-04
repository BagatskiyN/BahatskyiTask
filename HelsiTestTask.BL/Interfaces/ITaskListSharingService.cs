namespace HelsiTestTask.BL.Interfaces
{
    public interface ITaskListSharingService
    {
        Task ShareWithUserAsync(string id, string userId, string targetUserId);

        Task RemoveUserSharingAsync(string id, string userId, string targetUserId);

        Task<List<string>> GetSharedUsersAsync(string id, string userId);
    }
}
