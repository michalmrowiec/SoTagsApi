namespace SoTagsApi.Domain.Interfaces
{
    public interface ITagsRepository
    {
        Task<IList<Tag>> CreateRangeAsync(IList<Tag> tags);
        Task DeleteAllAsync();
    }
}
