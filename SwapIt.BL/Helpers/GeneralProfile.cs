using AutoMapper;
using SwapIt.BL.DTOs;
using SwapIt.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.BL.Helpers
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<ServiceDto, Service>().ReverseMap();
            CreateMap<DropDownDto, Service>().ReverseMap();
            CreateMap<DropDownDto, Category>().ReverseMap();
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<ServiceRequestDto, ServiceRequest>().ReverseMap();
            CreateMap<RateDto, Rate>().ReverseMap();
            CreateMap<NotificationDto, Notification>().ReverseMap();
            CreateMap<UserNotificationDto, UserNotification>().ReverseMap();


        }
    }
}
