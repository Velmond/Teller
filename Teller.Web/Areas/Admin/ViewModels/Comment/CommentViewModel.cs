namespace Teller.Web.Areas.Admin.ViewModels.Comment
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    ////using AutoMapper;
    
    using Teller.Models;
    using Teller.Web.Infrastructure.Mapping;
    
    public class CommentViewModel : IMapFrom<Comment>//, IHaveCustomMappings
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [DataType("TextArea")]
        [Required]
        [StringLength(1000, MinimumLength = 2)]
        [UIHint("TextArea")]
        public string Content { get; set; }

        [Required]
        public DateTime Published { get; set; }

        [Display(Name = "Flagged")]
        [Required]
        public bool IsFlagged { get; set; }

        [Required]
        public string Author { get; set; }
        
        [Display(Name = "Story")]
        [Required]
        public string StoryTitle { get; set; }

        [Display(Name = "Likes")]
        public int? LikesCount { get; set; }

        ////public void CreateMappings(IConfiguration configuration)
        ////{
        ////    configuration.CreateMap<Comment, CommentViewModel>()
        ////        .ForMember(v => v.Author,
        ////                   opt => opt.MapFrom(c => c.Author.UserName))
        ////        .ForMember(v => v.StoryTitle,
        ////                   opt => opt.MapFrom(c => c.Story.Title))
        ////        .ForMember(v => v.LikesCount,
        ////                   opt => opt.MapFrom(c => c.Likes != null ? c.Likes.Count() : 0))
        ////        .ReverseMap();
        ////}
    }
}