public class ResponseModel
{
    public List<Tag> Items { get; set; }
    public bool HasMore { get; set; }
    public int QuotaMax { get; set; }
    public int QuotaRemaining { get; set; }
}
