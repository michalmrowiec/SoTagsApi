namespace SoTagsApi.Domain.Interfaces
{
    public interface ISoTagService
    {
        Task FetchTagsAsync(int tagsToFetch = 1000);
    }
}