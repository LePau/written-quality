using System;

namespace WrittenQuality.Models
{
    public class WrittenDocument
    {
        public string Id { get; set; } = null;
        public DateTime CreatedDate { get; set; }
        public string Markdown { get; set; } = "";
    }
}