namespace SoTagsApi.Domain.Interfaces
{
    public interface ISoTagService
    {
        Task<bool> FetchTagsAsync(int tagsToFetch = 1000);
    }
}