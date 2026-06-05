using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace HakatonServer.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Lastname { get; set; }
        public string? Patronymic { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

    }
}
