using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ElibraryParserWeb.Models
{
    public class Publication
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Authors { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
