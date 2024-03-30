using SoTagsApi.Domain.Models;

namespace SoTagsApi.Domain.Interfaces
{
    public interface ITagsRepository
    {
        Task<IList<Tag>> CreateRangeAsync(IList<Tag> tags);
        Task DeleteAllAsync();
        Task<PagedResult<Tag>> GetPaginedSortedTags(
                            int pageSize = 10,
                            int pageNumber = 1,
                            string sortProperty = "name",
                            string sortOrder = "desc");
    }
}
