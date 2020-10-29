using AutoMapper;
using HRM.API.ViewModels;
using HRM.Core.Models.Users;
using HRM_BE.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.API.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserViewModel>()
                .ForMember(vm => vm.Teams, o => o.MapFrom(u => u.TeamUsers.Select(tu => tu.Team)))
                .ForMember(vm => vm.Projects, o => o.MapFrom(u => u.UserAccountDomains.Select(ur => new ProjectViewModel
                {
                    Id = ur.AccountDomainId,
                    Name = ur.AccountDomain.Name,
                    Client = ur.AccountDomain.Client,
                    StartDate = ur.StartDate,
                    EndDate = ur.EndDate,
                    IsActive = ur.IsActive
                })));
            CreateMap<Team, TeamViewModel>();
            CreateMap<User, SimpleUserViewModel>();
        }
    }
}
