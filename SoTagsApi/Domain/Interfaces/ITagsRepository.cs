using SoTagsApi.Domain.Models;

namespace SoTagsApi.Domain.Interfaces
{
    public interface ITagsRepository
    {
        Task<IList<Tag>> CreateRangeAsync(IList<Tag> tags);
        Task DeleteAllAsync();
        Task<PagedResult<Tag>> GetPaginedSortedTags(
                            string sortProperty,
                            string sortOrder,
                            int pageSize = 10,
                            int pageNumber = 1);
    }
}
