using HakatonServer.Dtos;
using HakatonServer.Models;

namespace HakatonServer.Hubs.Interfaces
{
    public interface INewsHub
    {
        Task UpdateNews(NewsDto news);
    }
}
