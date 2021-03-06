using AutoMapper;
using TicketingSystem.Domain.Models;
using TicketingSystem.RazorWebsite.Models.Tickets;
using TicketingSystem.RazorWebsite.Models.Contracts;

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
                .ForMember(target => target.Client, y => y.MapFrom(source => source.Client.Name))
                .ForMember(target => target.Comments, y => y.MapFrom(source => source.Comments));

            CreateMap<Attachment, AttachmentViewModel>()
                .ForMember(target => target.Name, y => y.MapFrom(source => source.Name))
                .ForMember(target => target.Id, y => y.MapFrom(source => source.AttachmentId));

            CreateMap<Contract, ContractBaseInfoViewModel>()
                .ForMember(target => target.Id, y => y.MapFrom(source => source.ContractId))
                .ForMember(target => target.Status, y => y.MapFrom(source => source.Status.ToString()))
                .ForMember(target => target.Type, y => y.MapFrom(source => source.Type.Name));
        }
    }
}