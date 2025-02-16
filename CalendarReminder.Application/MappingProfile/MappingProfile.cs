using AutoMapper;
using CalendarReminder.Application.Dtos;
using CalendarReminder.Domain.Entities;

namespace CalendarReminder.Application.MappingProfile;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CalendarEventDto, CalendarEvent>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Reminders, opt => opt.MapFrom(src => src.Reminders));
        
        CreateMap<CalendarEventCsvDto, CalendarEvent>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Reminders, opt => opt.MapFrom(src => src.Reminders));
        
        CreateMap<ReminderDto, Reminder>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IsSent, opt => opt.MapFrom(_ => false));

        CreateMap<CalendarEvent, CalendarEventDto>();
        CreateMap<CalendarEvent, CalendarEventCsvDto>();
        CreateMap<Reminder, ReminderDto>();

        CreateMap<ReturnUserDto, User>();
        CreateMap<User, ReturnUserDto>();

        CreateMap<CreateUserDto, User>();
        CreateMap<User, CreateUserDto>();
        
    }
}