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
            CreateMap<Ticket, TicketBaseInfoViewModel>()
                .ForMember(target => target.Id, y => y.MapFrom(source => source.Ticketnr))
                .ForMember(target => target.Status, y => y.MapFrom(source => source.Status.ToString()));

            CreateMap<Ticket, TicketDetailInfoViewModel>()
                .ForMember(target => target.Id, y => y.MapFrom(source => source.Ticketnr))
                .ForMember(target => target.Type, y => y.MapFrom(source => source.Type.Name))
                .ForMember(target => target.Title, y => y.MapFrom(source => source.Title))
                .ForMember(target => target.Status, y => y.MapFrom(source => source.Status.ToString()))
                .ForMember(target => target.DateAdded, y => y.MapFrom(source => source.DateAdded))
                .ForMember(target => target.Description, y => y.MapFrom(source => source.Description))
                .ForMember(target => target.Client, y => y.MapFrom(source => source.Client.UserName))
                .ForMember(target => target.Comments, y => y.MapFrom(source => source.Comments));
        }
    }
}
/*
public string AssignedEngineer { get; set; }
public string Type { get; set; }
*/