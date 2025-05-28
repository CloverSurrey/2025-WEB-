using System;

namespace Music_Shopping.Models
{
    public class RecordProduct : BaseProduct
    {
        public string ReleaseDate { get; set; }
        public string Language { get; set; }
        public string MusicGenre { get; set; }

        public override string GetDetailViewName()
        {
            return "RecordDetail";
        }
    }
} 