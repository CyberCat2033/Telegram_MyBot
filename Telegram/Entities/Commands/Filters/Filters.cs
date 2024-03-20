namespace Telegramchik.Commands.Filters;

public class Filters : IFilters
{
    public string? Text { get; set; }
    public byte Type { get; set; }
    public string? FileId { get; set; }
    public string Name { get; set; }


}
