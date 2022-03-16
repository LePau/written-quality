using System;
using System.Collections.Generic;
using System.Linq;

namespace WrittenQuality.Models
{
    public class UrlMetadata
    {
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string LinkUrl { get; set; } // used by reddit, for example
        public IList<string> MediaUrls { get; set; } = new List<string>();
        public IList<string> Authors { get; set; } = new List<string>();
        public string Description { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public IList<UrlMetadata> Recommendations { get; set; } = new List<UrlMetadata>();
        public string Content { get; set; }
        public string Quotee { get; set; }

    }


}