namespace SoTagsApi.Application.Tags.Queries
{
    public class TagDto
    {
        public string Name { get; set; } = default!;
        public long Count { get; set; }
        public double PercentageShare { get; set; }
    }
}
