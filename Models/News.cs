using System.ComponentModel.DataAnnotations;

namespace HakatonServer.Models
{
    public class News
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
