using HakatonServer.Data;
using HakatonServer.Dtos.News;
using HakatonServer.Hubs;
using HakatonServer.Hubs.Interfaces;
using HakatonServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace HakatonServer.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class NewsController(AppDbContext db, IHubContext<NewsHub, INewsHub> hub) : ControllerBase
    {
        private readonly AppDbContext _db = db;
        private readonly IHubContext<NewsHub, INewsHub> _hub = hub;


        [HttpGet]
        public async Task<ActionResult<News>> GetNewsAsync()
        {
            try
            {
                List<News> news = await _db.News.ToListAsync();

                return Ok(news);
            }
            catch (Exception ex)
            {
                return StatusCode(501, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<News>> GetNewsById(int Id)
        {
            try
            {
                var news = await _db.News.Where(x => x.Id == Id).FirstAsync();
                if (news != null)
                {
                    return Ok(news);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<News>> AddNewsAsync([FromBody] CreateNewsDto CreateNews)
        {
            try
            {
                News news = new()
                {
                    Title = CreateNews.Title,
                    Description = CreateNews.Description,
                    CreatedAt = DateTime.Now
                };

                _db.News.Add(news);
                await _db.SaveChangesAsync();

                
                var response = new NewsDto(news.Id, news.Title, news.Description, news.CreatedAt);
                await _hub.Clients.All.UpdateNews(response);

                return CreatedAtAction(
                    nameof(GetNewsById),
                    new { id = news.Id },
                    response
                );
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
