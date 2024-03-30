using Microsoft.EntityFrameworkCore;
using SoTagsApi.Domain.Interfaces;
using SoTagsApi.Domain.Models;
using System.Linq.Expressions;

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

        public async Task<PagedResult<Tag>> GetPaginedSortedTags(
            int pageSize = 10,
            int pageNumber = 1,
            string sortProperty = "name",
            string sortOrder = "desc")
        {
            IQueryable<Tag> tagsQuery = _context.Tags;

            Expression<Func<Tag, object>> propSelector = sortProperty.ToLower() switch
            {
                "count" => tag => tag.Count,
                "percentageshare" => tag => tag.PercentageShare,
                _ => tag => tag.Name,
            };

            if (sortOrder == "desc")
            {
                tagsQuery = tagsQuery.OrderByDescending(propSelector);
            }
            else
            {
                tagsQuery = tagsQuery.OrderBy(propSelector);
            }

            var tags = await tagsQuery
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            var pagedResult = new PagedResult<Tag>(tags, tagsQuery.Count(), pageSize, pageNumber);

            return pagedResult;
        }
    }
}
