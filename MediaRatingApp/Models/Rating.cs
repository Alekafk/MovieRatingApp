using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaRatingApp.Models
{
    internal class Rating
    {
        internal int Id { get; set; }
        internal int Stars {  get; set; }
        internal string MediaID { get; set; }
        internal string CreatorID { get; set; }
        internal string? Comment { get; set; }
        internal int LikesCount { get; set; }

        internal Rating(int id, int stars, string mediaID, string creatorID, string? comment, int likesCount)
        {
            Id = id;
            Stars = stars;
            MediaID = mediaID;
            CreatorID = creatorID;
            Comment = comment;
            LikesCount = likesCount;
        }
    }
}
