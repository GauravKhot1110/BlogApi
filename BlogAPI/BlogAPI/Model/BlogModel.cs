using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Model
{
    [Keyless]
    public class BlogMaster
    {
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            [Key]
            public string UserID { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Filename { get; set; }
            public int CountLikes { get; set; }
            public int CountUnLikes { get; set; }
            public int CountViews { get; set; }
            public int CountComments { get; set; }
            public string CreateBy { get; set; }
            public bool IsActive { get; set; }
            public DateTime CreateDate { get; set; }
      
    }
    [Keyless]
    public class ActionDetails
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string ActionUserID { get; set; }
        public string ActionBy { get; set; }
        public string ActionFor { get; set; }
        public int IsLikeUnlikeComment { get; set; }
        public string Comment { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }

    }

    public class ActionViewDetails
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string FirstName { get; set; }
        public int IsLikeUnlikeComment { get; set; }
        public string Comment { get; set; }
        public string ProfileImg { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
