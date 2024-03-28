namespace SoTagsApi.Application.Tags.Queries
{
    public class TagDto
    {
        public TagDto(string name, long count, float percentageShare)
        {
            Name = name;
            Count = count;
            PercentageShare = percentageShare;
        }

        public string Name { get; set; } = default!;
        public long Count { get; set; }
        public float PercentageShare { get; set; }
    }
}
