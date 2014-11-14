namespace Teller.Web.Areas.Admin.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
 
    using AutoMapper;
    
    using Teller.Models;
    using Teller.Web.Infrastructure.Mapping;

    public class UserViewModel : IMapFrom<AppUser>, IHaveCustomMappings
    {
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        [Editable(false)]
        public string Username { get; set; }

        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Display(Name = "Registered on")]
        [Editable(false)]
        public DateTime RegisteredOn { get; set; }

        public string RoleId { get; set; }

        //public string AvatarPath { get; set; }

        [Editable(false)]
        public int CommentViolations { get; set; }

        [Editable(false)]
        public int StoryViolations { get; set; }

        //public IEnumerable<SelectListItem> Roles { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<AppUser, UserViewModel>()
                .ForMember(m => m.Username, opt => opt.MapFrom(u => u.UserName))
                .ForMember(m => m.RoleId, opt => opt.MapFrom(u => u.Roles.FirstOrDefault().RoleId))
                //.ForMember(m => m.AvatarPath, opt => opt.MapFrom(u => u.UserInfo.AvatarPath))
                .ReverseMap();
        }
    }
}
