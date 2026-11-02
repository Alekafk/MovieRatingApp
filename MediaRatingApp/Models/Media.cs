using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MediaRatingApp.Models
{
    internal class Media
    {
        internal int Id { get; set; }
        internal string MediaType { get; set; } // movie, series, game
        internal string Title { get; set; }
        internal string Description { get; set; }
        internal string Genre {  get; set; } // .pdf -> media type
        internal int ReleaseYear { get; set; }
        internal string AgeRestriction { get; set; }
        internal Media(int id, string mediaType, string title, string description, string genre, int releaseYear, string ageRestriction)
        {
            Id = id;
            MediaType = mediaType;
            Title = title;
            Description = description;
            Genre = genre;
            ReleaseYear = releaseYear;
            AgeRestriction = ageRestriction;
        }
    }
}
