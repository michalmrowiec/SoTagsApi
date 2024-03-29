using Microsoft.EntityFrameworkCore;
using SoTagsApi.Domain.Interfaces;

namespace SoTagsApi.Infrastructure.Repositories
{
    public class TagsRepository : ITagsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TagsRepository> _logger;
        public TagsRepository(ApplicationDbContext context, ILogger<TagsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IList<Tag>> CreateRangeAsync(IList<Tag> tags)
        {
            await _context.Tags.AddRangeAsync(tags);
            await _context.SaveChangesAsync();

            return tags;
        }

        public async Task DeleteAllAsync()
        {
            await _context.Tags.ExecuteDeleteAsync();
        }
    }
}
