using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystem.Domain.Models;
using TicketingSystem.RazorWebsite.Models.Tickets;

namespace TicketingSystem.RazorWebsite.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Ticket, TicketIndexDTO>()
                .ForMember(target => target.Id, y => y.MapFrom(source => source.Ticketnr))
                .ForMember(target => target.Status, y => y.MapFrom(source => source.Status.ToString()));
        }
    }
}
