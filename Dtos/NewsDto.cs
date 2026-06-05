namespace HakatonServer.Dtos
{
    public record NewsDto(int Id, string Title, string Description, DateTime DateTime);
    public record CreateNewsDto(string Title, string Description);
}
