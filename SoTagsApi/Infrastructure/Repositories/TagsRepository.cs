using Microsoft.EntityFrameworkCore;
using SoTagsApi.Domain.Interfaces;
using SoTagsApi.Domain.Models;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

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
            _logger.LogInformation($"Createing {tags.Count} new tags in db");
            await _context.Tags.AddRangeAsync(tags);
            await _context.SaveChangesAsync();

            return tags;
        }

        public async Task DeleteAllAsync()
        {
            _logger.LogInformation("Deleting tags from db");
            _context.Tags.RemoveRange(_context.Tags);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<Tag>> GetPaginedSortedTags(
            string sortProperty = "",
            string sortOrder = "",
            int pageSize = 10,
            int pageNumber = 1)
        {
            if (!Regex.IsMatch(sortProperty, "^(name|count|percentageshare|)$")
                || !Regex.IsMatch(sortOrder, "^(asc|desc|)$")
                || pageSize <= 0
                || pageSize > 100
                || pageNumber <= 0
                || pageSize > 10_000)
            {
                _logger.LogError($"Invalid parameters | sort property: {sortProperty} | sort order: {sortOrder} | page size: {pageSize} | page number: {pageNumber}");
                return new();
            }

            try
            {
                _logger.LogInformation($"Fetching paginated and sorted tags with pageSize: {pageSize}, pageNumber: {pageNumber}, sortProperty: {sortProperty}, sortOrder: {sortOrder}");

                var tagsQuery = _context.Tags
                    .AsNoTracking()
                    .AsQueryable();

                var totalCount = tagsQuery.Count();

                if (sortOrder == "desc")
                {
                    tagsQuery = tagsQuery.OrderByDescending(SelectProperty(sortProperty));
                }
                else
                {
                    tagsQuery = tagsQuery.OrderBy(SelectProperty(sortProperty));
                }

                var tags = await tagsQuery
                    .Skip(pageSize * (pageNumber - 1))
                    .Take(pageSize)
                    .ToListAsync();

                _logger.LogInformation($"Fetched {tags.Count} tags from database");

                var pagedResult = totalCount == 0 ? 
                    new() : new PagedResult<Tag>(tags, totalCount, pageSize, pageNumber);

                _logger.LogInformation($"Created paged result with {pagedResult.Items.Count} items");

                return pagedResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching paginated and sorted tags with pageSize: {pageSize}, pageNumber: {pageNumber}, sortProperty: {sortProperty}, sortOrder: {sortOrder}");
            }
            return new();
        }

        private static Expression<Func<Tag, object>> SelectProperty(string? sortProperty)
        {
            return sortProperty?.ToLower() switch
            {
                "count" => tag => tag.Count,
                "name" => tag => tag.Name,
                _ => tag => tag.PercentageShare,
            };
        }
    }
}
