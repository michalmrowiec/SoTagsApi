namespace SoTagsApi.Domain.Interfaces
{
    public interface ISoTagService
    {
        /// <summary>
        /// Fetches a list of tags into the database from the SO API.
        /// </summary>
        /// <param name="tagsToFetch">The passed parameter is automatically rounded up to 100.</param>
        /// <returns>Returns true if the operation was successful, otherwise returns false.</returns>
        Task<bool> FetchTagsAsync(int tagsToFetch = 1000);
    }
}